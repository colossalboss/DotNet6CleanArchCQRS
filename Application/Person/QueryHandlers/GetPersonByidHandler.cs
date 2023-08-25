using System;
using Application.Person.Queries;
using MediatR;

namespace Application.Person.QueryHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Abstractions;
    using Domain.Entities;

	public class GetPersonByidHandler : IRequestHandler<GetPersonById, Person>
	{
        private readonly IPersonRepository _personRepo;

        public GetPersonByidHandler(IPersonRepository personRepository)
		{
            _personRepo = personRepository;
		}

        public async Task<Person> Handle(GetPersonById request, CancellationToken cancellationToken)
        {
            return await _personRepo.GetPersonById(request.Id);
        }
    }
}

