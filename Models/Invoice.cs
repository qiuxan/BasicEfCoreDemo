using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BasicEfCoreDemo.Models
{
    public class Invoice
    {
        [Column("Id")]
        [Key]
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
    }
}

public enum InvoiceStatus
{
    Draft,
    Sent,
    Paid,
    Overdue,
    AwaitPayment
}
