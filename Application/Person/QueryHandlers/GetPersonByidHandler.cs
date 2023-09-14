using System;
using Application.Person.Queries;
using MediatR;

namespace Application.Person.QueryHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Abstractions;
    using Application.ViewModels;
    using AutoMapper;
    using Domain.Entities;

	public class GetPersonByidHandler : IRequestHandler<GetPersonById, PersonViewModel>
	{
        private readonly IPersonRepository _personRepo;
        private readonly IMapper _mapper;

        public GetPersonByidHandler(IPersonRepository personRepository, IMapper mapper)
		{
            _personRepo = personRepository;
            _mapper = mapper;
		}

        public async Task<PersonViewModel> Handle(GetPersonById request, CancellationToken cancellationToken)
        {
            var person = await _personRepo.GetPersonById(request.Id);
            return _mapper.Map<PersonViewModel>(person);
        }
    }
}

