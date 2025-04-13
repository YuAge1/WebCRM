using System;
using System.ComponentModel.DataAnnotations;

namespace WebCRM.Domain.Entities
{
    public class DealEntity : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DealStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public DateTime? ActualCloseDate { get; set; }

        public long CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }

        public long? AssignedToUserId { get; set; }
        public UserEntity AssignedToUser { get; set; }

        public long? LeadId { get; set; }
        public LeadEntity Lead { get; set; }
    }

    public enum DealStatus
    {
        New,
        InProgress,
        Negotiation,
        Won,
        Lost
    }
} 