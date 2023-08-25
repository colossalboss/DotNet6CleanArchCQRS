using System;
namespace Domain.Entities
{
	public sealed class Person
	{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

