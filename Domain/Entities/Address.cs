using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	public class Address
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string City { get; set; }
		[ForeignKey("PersonId")]
		public Person? Person { get; set; }
		public int PersonId { get; set; }
	}
}

