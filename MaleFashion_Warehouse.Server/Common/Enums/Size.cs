using System.Text.Json.Serialization;

namespace MaleFashion_Warehouse.Server.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Size
    {
        S = 1,
        M = 2,
        L = 3,
        XL = 4,
        XXL = 5,
    }
}
