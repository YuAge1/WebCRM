using System;
using System.ComponentModel.DataAnnotations;

namespace WebCRM.Domain.Entities
{
    public class ActivityEntity : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public ActivityType Type { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ActivityDate { get; set; }

        public long CreatedByUserId { get; set; }
        public UserEntity CreatedByUser { get; set; }

        public long? RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; } // Lead, Deal, Customer, etc.

        [StringLength(500)]
        public string Notes { get; set; }
    }

    public enum ActivityType
    {
        Call,
        Meeting,
        Email,
        Note,
        Task,
        Other
    }
} 