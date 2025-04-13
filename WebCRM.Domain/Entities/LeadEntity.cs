using System;
using System.ComponentModel.DataAnnotations;

namespace WebCRM.Domain.Entities
{
    public class LeadEntity : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Company { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public LeadStatus Status { get; set; }

        public decimal EstimatedValue { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastContactDate { get; set; }

        public long? AssignedToUserId { get; set; }
        public UserEntity AssignedToUser { get; set; }
    }

    public enum LeadStatus
    {
        New,
        Contacted,
        Qualified,
        Proposal,
        Negotiation,
        Won,
        Lost
    }
} 