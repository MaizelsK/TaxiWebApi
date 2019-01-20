﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public double Price { get; set; }
        public OrderStatus Status { get; set; }

        [ForeignKey("Creator")]
        public int UserId { get; set; }
        public User Creator { get; set; }
    }
}
