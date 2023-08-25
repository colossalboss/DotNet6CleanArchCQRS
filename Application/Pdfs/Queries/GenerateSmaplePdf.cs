using System;
using MediatR;

namespace Application.Pdfs.Queries
{
	public class GenerateSmaplePdf : IRequest<MemoryStream>
	{
		public string? FilePath { get; set; }
	}
}

