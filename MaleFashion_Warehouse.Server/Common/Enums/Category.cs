using System.Text.Json.Serialization;

namespace MaleFashion_Warehouse.Server.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        TShirt = 1,
        Shirt = 2,
        Trousers = 3,
        Jacket = 4,
        Accessories = 5
    }
}
