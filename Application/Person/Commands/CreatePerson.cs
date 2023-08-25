
namespace Application.Person.Commands
{
	using Domain.Entities;
    using MediatR;

    public class CreatePerson : IRequest<Person>
	{
		public string? Title { get; set; }
		public string? Description { get; set; }
	}
}
