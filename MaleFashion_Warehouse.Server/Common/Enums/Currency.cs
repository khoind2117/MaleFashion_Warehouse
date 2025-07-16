using System.Text.Json.Serialization;

namespace MaleFashion_Warehouse.Server.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        VND = 1,
        USD = 2,
    }
}
