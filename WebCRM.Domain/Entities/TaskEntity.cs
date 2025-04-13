using System;
using System.ComponentModel.DataAnnotations;

namespace WebCRM.Domain.Entities
{
    public class TaskEntity : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public TaskPriority Priority { get; set; }

        public TaskStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public long AssignedToUserId { get; set; }
        public UserEntity AssignedToUser { get; set; }

        public long? RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; } // Lead, Deal, Customer, etc.
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Completed,
        Cancelled
    }
} 