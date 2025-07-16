using System.Text.Json.Serialization;

namespace MaleFashion_Warehouse.Server.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentMethod
    {
        COD = 1,
        VnPay = 2,
        Momo = 3,
        PayPal = 4,
    }
}
