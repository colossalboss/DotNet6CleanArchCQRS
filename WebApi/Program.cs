using System.Reflection;
using Application;
using Application.Abstractions;
using Application.Person.Commands;
using Hangfire;
using Hangfire.SQLite;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/loginfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
});//.AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{ Assembly.GetExecutingAssembly().GetName().Name }.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    setupAction.IncludeXmlComments(xmlCommentsFullPath);

    setupAction.AddSecurityDefinition("WebApi", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });
});

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

//var serverVersion = new MySqlServerVersion(new Version(8, 0, 25));
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<PersonDbContext>(opt => opt.UseSqlServer(cs));
//builder.Services.AddDbContext<PersonDbContext>(opt => opt.UseMySql(connectionString: cs, serverVersion: ServerVersion.AutoDetect(cs)));
//builder.Services.AddDbContext<PersonDbContext>(opt => opt.UseSqlite(cs));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(CreatePerson).Assembly);
    //configuration.Using<CreatePerson>();
    //configuration.AddBehavior<CreatePerson>();
});

builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage("Data Source=hangfire.db;"));
    //.UseSqlServerStorage(builder.Configuration.GetConnectionString("Data Source=hangfire.db;"), new SqlServerStorageOptions
    //{
    //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
    //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
    //    QueuePollInterval = TimeSpan.Zero,
    //    UseRecommendedIsolationLevel = true,
    //    DisableGlobalLocks = true
    //}));

builder.Services.AddHangfireServer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});

#if DEBUG

#else
#endif



var app = builder.
    Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

