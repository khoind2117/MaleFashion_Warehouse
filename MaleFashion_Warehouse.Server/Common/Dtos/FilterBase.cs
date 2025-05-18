namespace MaleFashion_Warehouse.Server.Common.Dtos
{
    public class FilterBase
    {
        public string? Keyword { get; set; }
        public string? OrderBy { get; set; }
        public bool IsDescending { get; set; } = false;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string GetOrderDirection()
        {
            return IsDescending ? "DESC" : "ASC";
        }

        public int GetSkip()
        {
            return (CurrentPage - 1) * PageSize;
        }

        public int GetTake()
        {
            return PageSize;
        }
    }
}
