using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderAddress Address { get; set; } = default!;
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; } // FK
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];
        public decimal SubTotal { get; set; }

        //[NotMapped]
        //public decimal Total { get => SubTotal + DeliveryMethod.Price; }

        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;
    }
}
