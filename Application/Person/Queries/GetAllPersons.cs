using System;
using Application.MetaData;
using Application.ViewModels;
using MediatR;

namespace Application.Person.Queries
{
	public class GetAllPersons : IRequest<(IEnumerable<PersonViewModel>, PaginationMetaData)>
	{
		public string? searchQuery { get; set; }
		public string? Name { get; set; }
		public int PageSize { get; set; }
		public int CurrentPage { get; set; }
	}
}

