using Application;
using Application.Abstractions;
using Application.Person.Commands;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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



var app = builder.
    Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

