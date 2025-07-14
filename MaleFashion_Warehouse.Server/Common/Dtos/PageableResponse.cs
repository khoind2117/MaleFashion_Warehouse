namespace MaleFashion_Warehouse.Server.Common.Dtos
{
    public class PageableResponse<T>
    {
        public List<T>? Content { get; set; } // The list of items on the current page
        public bool Empty { get; set; } // True if no items on the current page
        public bool First { get; set; } // True if this is the first page
        public bool Last { get; set; } // True if this is the last page
        public int Size { get; set; } 
        public int Number { get; set; } // Current page number
        public int NumberOfElements { get; set; } // Number of items on the current page
        public Pageable? Pageable { get; set; } // Pagination info
        public Sort? Sort { get; set; } // Sort info
        public int TotalElements { get; set; } // Total number of items in all pages
        public int TotalPages { get; set; } // Total number of page available
    }
}
