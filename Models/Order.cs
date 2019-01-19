using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public OrderStatus Status { get; set; }
    }
}
