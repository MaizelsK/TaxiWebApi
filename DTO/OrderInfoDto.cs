using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class OrderInfoDto
    {
        public int OrderId { get; set; }
        public string Username { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
    }
}
