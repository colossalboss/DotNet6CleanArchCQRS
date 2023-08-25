using System;
using Domain.Entities;
using MediatR;

namespace Application.Person.CommandHandlers
{
	public class UpdatePersonHandler : IRequest<Domain.Entities.Person>
	{
		public string? Name { get; set; }
		public string? Email { get; set; }
	}
}

