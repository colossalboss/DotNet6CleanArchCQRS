using System;

namespace Application.Abstractions
{
    using Domain.Entities;

    public interface IPersonRepository
	{
        Task<ICollection<Person>> GetAll();

        IQueryable<Person> GetAllQueryable();

        Task<Person> GetPersonById(int personId);

        Task<Person> AddPerson(Person toCreate);

        Task<Person> UpdatePerson(int personId, string name, string email);

        Task DeletePerson(int personId);

        Task<MemoryStream> GeneratePdf();
    }
}

