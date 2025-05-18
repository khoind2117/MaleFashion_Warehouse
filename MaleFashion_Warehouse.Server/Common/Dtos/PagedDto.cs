namespace MaleFashion_Warehouse.Server.Common.Dtos
{
    public class PagedDto<T>
    {
        public List<T>? Items { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; } = 12;
        public int PageNumber { get; set; } = 1;
        public bool HasNextPage => PageNumber * PageSize < TotalRecords;
        public bool HasPreviousPage => PageNumber > 1;

        public PagedDto(int totalRecords, List<T> items)
        {
            TotalRecords = totalRecords;
            Items = items;
        }
    }
}
