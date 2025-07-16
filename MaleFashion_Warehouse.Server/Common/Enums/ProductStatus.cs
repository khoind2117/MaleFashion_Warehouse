using System.Text.Json.Serialization;

namespace MaleFashion_Warehouse.Server.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductStatus
    {
        Active = 1,
        InActive = 2,
    }
}
