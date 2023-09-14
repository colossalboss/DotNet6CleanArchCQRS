using System;
using System.Diagnostics;
using Application.Abstractions;

namespace Application.Jobs.TestJob
{
	public class GenerateNothingJob
	{
        private readonly IPersonRepository _personRepository;

        public GenerateNothingJob(IPersonRepository personRepository)
		{
			_personRepository = personRepository;
		}

		public void DoTheWork(int num)
		{
			var count = num;
			while (count > 0)
			{
				Debug.WriteLine(count);
				count = count - 1;
			}
		}
	}
}

