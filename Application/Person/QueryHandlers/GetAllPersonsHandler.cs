using Application.Abstractions;
using Application.MetaData;
using Application.Person.Queries;
using Application.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Person.QueryHandlers
{
    public class GetAllPersonsHandler : IRequestHandler<GetAllPersons, (IEnumerable<PersonViewModel>, PaginationMetaData)>
	{
        private readonly IPersonRepository _personRepositroy;
        private readonly IMapper _mapper;

        public GetAllPersonsHandler(IPersonRepository personRepository, IMapper mapper)
		{
            _personRepositroy = personRepository;
            _mapper = mapper;
		}

        public async Task<(IEnumerable<PersonViewModel>, PaginationMetaData)> Handle(GetAllPersons request, CancellationToken cancellationToken)
        {
            var collection = _personRepositroy.GetAllQueryable();

            var totalItemCount = collection.Count();
            var pageMetaData = new PaginationMetaData(totalItemCount, request.PageSize, request.CurrentPage);

            var collectionToReturn = collection
                .Skip(request.PageSize * (request.CurrentPage - 1))
                .Take(request.PageSize)
                .ToList();

            return (_mapper.Map<IEnumerable<PersonViewModel>>(collectionToReturn), pageMetaData);
        }
    }
}

