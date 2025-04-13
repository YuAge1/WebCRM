using WebCRM.Domain.Entities;
using TaskStatus = WebCRM.Domain.Entities.TaskStatus;

namespace WebCRM.Application.Interfaces
{
    public interface ICrmService
    {
        // Lead Management
        Task<LeadEntity> CreateLeadAsync(LeadEntity lead);
        Task<LeadEntity> UpdateLeadAsync(LeadEntity lead);
        Task<LeadEntity> GetLeadByIdAsync(long id);
        Task<IEnumerable<LeadEntity>> GetAllLeadsAsync();
        Task<IEnumerable<LeadEntity>> GetLeadsByStatusAsync(LeadStatus status);
        Task<bool> DeleteLeadAsync(long id);

        // Deal Management
        Task<DealEntity> CreateDealAsync(DealEntity deal);
        Task<DealEntity> UpdateDealAsync(DealEntity deal);
        Task<DealEntity> GetDealByIdAsync(long id);
        Task<IEnumerable<DealEntity>> GetAllDealsAsync();
        Task<IEnumerable<DealEntity>> GetDealsByStatusAsync(DealStatus status);
        Task<bool> DeleteDealAsync(long id);

        // Task Management
        Task<TaskEntity> CreateTaskAsync(TaskEntity task);
        Task<TaskEntity> UpdateTaskAsync(TaskEntity task);
        Task<TaskEntity> GetTaskByIdAsync(long id);
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(long userId);
        Task<IEnumerable<TaskEntity>> GetTasksByStatusAsync(TaskStatus status);
        Task<bool> DeleteTaskAsync(long id);

        // Activity Management
        Task<ActivityEntity> CreateActivityAsync(ActivityEntity activity);
        Task<ActivityEntity> UpdateActivityAsync(ActivityEntity activity);
        Task<ActivityEntity> GetActivityByIdAsync(long id);
        Task<IEnumerable<ActivityEntity>> GetAllActivitiesAsync();
        Task<IEnumerable<ActivityEntity>> GetActivitiesByEntityIdAsync(long entityId, string entityType);
        Task<IEnumerable<ActivityEntity>> GetActivitiesByUserIdAsync(long userId);
        Task<bool> DeleteActivityAsync(long id);

        // Analytics
        Task<CrmAnalytics> GetAnalyticsAsync(DateTime startDate, DateTime endDate);
    }

    public class CrmAnalytics
    {
        public int TotalLeads { get; set; }
        public int ConvertedLeads { get; set; }
        public decimal ConversionRate { get; set; }
        public int TotalDeals { get; set; }
        public int WonDeals { get; set; }
        public decimal WinRate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageDealSize { get; set; }
        public int ActiveTasks { get; set; }
        public int CompletedTasks { get; set; }
        public decimal TaskCompletionRate { get; set; }
    }
} 