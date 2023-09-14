using System;
using Application.Abstractions;
using Application.Person.Commands;
using MediatR;

namespace Application.Person.CommandHandlers
{
    using Domain.Entities;

	public class CreatePersonHandler : IRequestHandler<CreatePerson, Person>
	{
        private readonly IPersonRepository _personRepo;

        public CreatePersonHandler(IPersonRepository repository)
		{
            _personRepo = repository;
		}

        public async Task<Person> Handle(CreatePerson request, CancellationToken cancellationToken)
        {
            var person = new Person(request.Title ?? string.Empty)
            {
                Email = request.Description ?? string.Empty
            };

            return await _personRepo.AddPerson(person);
        }
    }
}

