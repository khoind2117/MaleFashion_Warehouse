using System.Text.Json.Serialization;

namespace MaleFashion_Warehouse.Server.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InventoryStatus
    {
        InStock = 1,
        OutOfStock = 2,
        LowStock = 3,
        NotAvailable = 4, // Hidden from Frontend
    }
}
