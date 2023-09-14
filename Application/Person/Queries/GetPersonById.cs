using System;
using MediatR;

namespace Application.Person.Queries
{
    using Application.ViewModels;
    using Domain.Entities;

	public class GetPersonById : IRequest<PersonViewModel>
	{
		public int Id { get; set; }
	}
}

