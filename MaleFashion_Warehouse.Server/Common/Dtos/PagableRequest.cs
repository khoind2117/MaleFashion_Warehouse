using MaleFashion_Warehouse.Server.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace MaleFashion_Warehouse.Server.Common.Dtos
{
    public class PagableRequest<T>
    {
        public T? Criteria { get; set; }
        [Range(0, int.MaxValue)]
        public int Page { get; set; } = 0;

        [Range(1, int.MaxValue)]
        public int Size { get; set; } = 10;
        public string? SortBy { get; set; } = null;
        public SortEnum SortDirection { get; set; } = SortEnum.DESC;
    }
}
