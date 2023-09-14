using System;
using AutoMapper;

namespace Application.Profiles
{
    using Application.ViewModels;

    public class PersonProfile : Profile
	{
		public PersonProfile()
		{
			CreateMap<Domain.Entities.Person, PersonViewModel>();
		}
	}
}

