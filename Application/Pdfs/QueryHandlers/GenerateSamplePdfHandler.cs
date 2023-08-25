using System;
using System.Reflection;
using Application.Abstractions;
using Application.Pdfs.Queries;
using MediatR;
using RazorEngineCore;
using WkHtmlToPdfDotNet;

namespace Application.Pdfs.QueryHandlers
{
	public class GenerateSamplePdfHandler : IRequestHandler<GenerateSmaplePdf, MemoryStream>
	{
        private readonly IPersonRepository _personRepository;

        public GenerateSamplePdfHandler(IPersonRepository personRepository)
		{
            _personRepository = personRepository;
		}

        public async Task<MemoryStream> Handle(GenerateSmaplePdf request, CancellationToken cancellationToken)
        {
            var razorEngine = new RazorEngine();
            var template = razorEngine.Compile(File.ReadAllText(Path.Join(GetTemplateLocation(request.FilePath ?? string.Empty), "wwwroot/sample.html")));

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
        }

        private string? GetTemplateLocation(string filePath)
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            //UriBuilder uri = new UriBuilder(codeBase);

            //var path = Uri.UnescapeDataString(uri.Path);
            var directoryPath = Path.GetDirectoryName(filePath);

            if (directoryPath != null)
                return Directory.GetParent(directoryPath)?.FullName;

            return null;
        }
    }
}

