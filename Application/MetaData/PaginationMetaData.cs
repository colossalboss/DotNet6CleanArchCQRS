using System;
namespace Application.MetaData
{
	public class PaginationMetaData
	{
		public int TotalItemCount { get; set; }
		public int TotalPageCount { get; set; }
		public int PageSize { get; set; }
		public int CurrentPage { get; set; }

		public PaginationMetaData(int totalItemCount, int pageSize, int currentPaage)
		{
			TotalItemCount = totalItemCount;
			PageSize = pageSize;
			CurrentPage = currentPaage;
			TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)PageSize);
		}
	}
}

