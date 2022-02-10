using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Payment in finance")]
        PaymentReceived,

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,
       [EnumMember(Value = "Payment pending recive")]
        PendingRecive,
        [EnumMember(Value = "Payment Completed")]
        PaymentCompleted,
        [EnumMember(Value = "Inventory")]
        Inventory,

    }
}