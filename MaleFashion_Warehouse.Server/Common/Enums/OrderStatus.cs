using System.Text.Json.Serialization;

namespace MaleFashion_Warehouse.Server.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Processing = 3,
        OutForDelivery = 4,
        Delivered = 5,
        Cancelled = 6,
        Rejected = 7,
    }
}
