namespace MaleFashion_Warehouse.Server.Common.Dtos
{
    public class Pageable
    {
        public Sort? Sort { get; set; } = new Sort();
        public int Offset { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool Paged { get; set; }
        public bool Unpaged { get; set; }
    }
}