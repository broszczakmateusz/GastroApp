﻿namespace GastroApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime? PaidDateTime { get; set; }
        public bool IsPaid { get; set; }
        public enum PaidWith { NotPaid, Cash, Card }
        public float TotalPrice { get; set; }
    }
}