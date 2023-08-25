using System;
using Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    using System.Reflection;
    using Domain.Entities;
    using Infrastructure.Persistence;
    using RazorEngineCore;
    using WkHtmlToPdfDotNet;

    public class PersonRepository : IPersonRepository
	{
        private readonly PersonDbContext _context;

        public PersonRepository(PersonDbContext context)
		{
            _context = context;
		}

        public async  Task<Person> AddPerson(Person toCreate)
        {
            _context.Persons.Add(toCreate);

            await _context.SaveChangesAsync();

            return toCreate;
        }

        public async Task DeletePerson(int personId)
        {
            var person = _context.Persons
            .FirstOrDefault(p => p.Id == personId);

            if (person is null) return;

            _context.Persons.Remove(person);

            await _context.SaveChangesAsync();
        }

        public async Task<MemoryStream> GeneratePdf()
        {
            var razorEngine = new RazorEngine();
            var template = razorEngine.Compile(File.ReadAllText(Path.Join(GetTemplateLocation(), "wwwroot/sample.html")));

            var result = template.Run(new { Name = "Example Name" });

            var memmoryStream = new MemoryStream();
            using (var pdfTool = new PdfTools())
                using (var converter = new BasicConverter(pdfTool))
            {
                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings =
                    {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.Letter
                    },
                    Objects =
                    {
                        new ObjectSettings()
                        {
                            PagesCount = true,
                            HtmlContent = result,
                            WebSettings = { DefaultEncoding = "utf-8"},
                        }
                    }
                };

                var bytes = converter.Convert(doc);
                await memmoryStream.WriteAsync(bytes, 0, bytes.Length);
                memmoryStream.Seek(0, SeekOrigin.Begin);

                return memmoryStream;
            }
            //throw new NotImplementedException();
        }

        public async Task<ICollection<Person>> GetAll()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person> GetPersonById(int personId)
        {
            return await _context.Persons.FirstOrDefaultAsync(p => p.Id == personId);
        }

        public async Task<Person> UpdatePerson(int personId, string name, string email)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            person.Name = name;
            person.Email = email;

            await _context.SaveChangesAsync();

            return person;
        }

        private string? GetTemplateLocation()
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            UriBuilder uri = new UriBuilder(codeBase);

            var path = Uri.UnescapeDataString(uri.Path);
            var directoryPath = Path.GetDirectoryName(path);

            if (directoryPath != null)
                return Directory.GetParent(directoryPath)?.FullName;

            return null;
        }
    }
}

