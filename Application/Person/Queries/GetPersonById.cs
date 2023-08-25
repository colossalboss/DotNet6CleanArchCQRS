using System;
using MediatR;

namespace Application.Person.Queries
{
	using Domain.Entities;

	public class GetPersonById : IRequest<Person>
	{
		public int Id { get; set; }
	}
}

