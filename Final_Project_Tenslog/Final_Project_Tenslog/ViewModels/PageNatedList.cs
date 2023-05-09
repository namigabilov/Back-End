namespace Final_Project_Tenslog.ViewModels
{
    public class PageNatedList<T> : List<T>
    {
        public PageNatedList(List<T> queries, int pageindex, int totalPage)
        {
            PageIndex = pageindex;
            TotalPage = totalPage;

            int start;

            if (pageindex == 0)
            {
                start = PageIndex = 0;
            }
            else
            {
                start = PageIndex - 1;
            }
            int end = PageIndex + 1;

            if (start <= 0)
            {
                end = end - (start - 1);
                start = 1;
            }

            if (end > TotalPage)
            {
                end = TotalPage;
            }

            StartPage = start;
            EndPage = end;

            this.AddRange(queries);
        }

        public int PageIndex { get; }
        public int TotalPage { get; }
        public int StartPage { get; }
        public int EndPage { get; }
        public bool HasPrev => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPage;

        public static PageNatedList<T> Create(IQueryable<T> query, int pageIndex, int itemCount)
        {
            int totalPage = (int)Math.Ceiling((decimal)query.Count() / itemCount);
            query = query.Skip((pageIndex - 1) * itemCount).Take(itemCount);

            return new PageNatedList<T>(query.ToList(), pageIndex, totalPage);
        }
    }
}
