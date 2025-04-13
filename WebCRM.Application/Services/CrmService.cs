using Microsoft.EntityFrameworkCore;
using WebCRM.Application.Interfaces;
using WebCRM.Domain;
using WebCRM.Domain.Entities;
using TaskStatus = WebCRM.Domain.Entities.TaskStatus;

namespace WebCRM.Application.Services
{
    public class CrmService : ICrmService
    {
        private readonly OrdersDbContext _context;

        public CrmService(OrdersDbContext context)
        {
            _context = context;
        }

        #region Lead Management
        public async Task<LeadEntity> CreateLeadAsync(LeadEntity lead)
        {
            lead.CreatedDate = DateTime.UtcNow;
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<LeadEntity> UpdateLeadAsync(LeadEntity lead)
        {
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<LeadEntity> GetLeadByIdAsync(long id)
        {
            return await _context.Leads
                .Include(l => l.AssignedToUser)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<LeadEntity>> GetAllLeadsAsync()
        {
            return await _context.Leads
                .Include(l => l.AssignedToUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeadEntity>> GetLeadsByStatusAsync(LeadStatus status)
        {
            return await _context.Leads
                .Include(l => l.AssignedToUser)
                .Where(l => l.Status == status)
                .ToListAsync();
        }

        public async Task<bool> DeleteLeadAsync(long id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return false;

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Deal Management
        public async Task<DealEntity> CreateDealAsync(DealEntity deal)
        {
            deal.CreatedDate = DateTime.UtcNow;
            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();
            return deal;
        }

        public async Task<DealEntity> UpdateDealAsync(DealEntity deal)
        {
            _context.Deals.Update(deal);
            await _context.SaveChangesAsync();
            return deal;
        }

        public async Task<DealEntity> GetDealByIdAsync(long id)
        {
            return await _context.Deals
                .Include(d => d.Customer)
                .Include(d => d.AssignedToUser)
                .Include(d => d.Lead)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DealEntity>> GetAllDealsAsync()
        {
            return await _context.Deals
                .Include(d => d.Customer)
                .Include(d => d.AssignedToUser)
                .Include(d => d.Lead)
                .ToListAsync();
        }

        public async Task<IEnumerable<DealEntity>> GetDealsByStatusAsync(DealStatus status)
        {
            return await _context.Deals
                .Include(d => d.Customer)
                .Include(d => d.AssignedToUser)
                .Include(d => d.Lead)
                .Where(d => d.Status == status)
                .ToListAsync();
        }

        public async Task<bool> DeleteDealAsync(long id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal == null) return false;

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Task Management
        public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
        {
            task.CreatedDate = DateTime.UtcNow;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity> UpdateTaskAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity> GetTaskByIdAsync(long id)
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(long userId)
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Where(t => t.AssignedToUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByStatusAsync(TaskStatus status)
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task<bool> DeleteTaskAsync(long id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Activity Management
        public async Task<ActivityEntity> CreateActivityAsync(ActivityEntity activity)
        {
            activity.CreatedDate = DateTime.UtcNow;
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<ActivityEntity> UpdateActivityAsync(ActivityEntity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<ActivityEntity> GetActivityByIdAsync(long id)
        {
            return await _context.Activities
                .Include(a => a.CreatedByUser)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<ActivityEntity>> GetAllActivitiesAsync()
        {
            return await _context.Activities
                .Include(a => a.CreatedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivityEntity>> GetActivitiesByEntityIdAsync(long entityId, string entityType)
        {
            return await _context.Activities
                .Include(a => a.CreatedByUser)
                .Where(a => a.RelatedEntityId == entityId && a.RelatedEntityType == entityType)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivityEntity>> GetActivitiesByUserIdAsync(long userId)
        {
            return await _context.Activities
                .Include(a => a.CreatedByUser)
                .Where(a => a.CreatedByUserId == userId)
                .ToListAsync();
        }

        public async Task<bool> DeleteActivityAsync(long id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Analytics
        public async Task<CrmAnalytics> GetAnalyticsAsync(DateTime startDate, DateTime endDate)
        {
            var analytics = new CrmAnalytics();

            // Lead Analytics
            var leads = await _context.Leads
                .Where(l => l.CreatedDate >= startDate && l.CreatedDate <= endDate)
                .ToListAsync();
            
            analytics.TotalLeads = leads.Count;
            analytics.ConvertedLeads = leads.Count(l => l.Status == LeadStatus.Won);
            analytics.ConversionRate = analytics.TotalLeads > 0 
                ? (decimal)analytics.ConvertedLeads / analytics.TotalLeads * 100 
                : 0;

            // Deal Analytics
            var deals = await _context.Deals
                .Where(d => d.CreatedDate >= startDate && d.CreatedDate <= endDate)
                .ToListAsync();
            
            analytics.TotalDeals = deals.Count;
            analytics.WonDeals = deals.Count(d => d.Status == DealStatus.Won);
            analytics.WinRate = analytics.TotalDeals > 0 
                ? (decimal)analytics.WonDeals / analytics.TotalDeals * 100 
                : 0;
            
            analytics.TotalRevenue = deals.Where(d => d.Status == DealStatus.Won).Sum(d => d.Amount);
            analytics.AverageDealSize = analytics.WonDeals > 0 
                ? analytics.TotalRevenue / analytics.WonDeals 
                : 0;

            // Task Analytics
            var tasks = await _context.Tasks
                .Where(t => t.CreatedDate >= startDate && t.CreatedDate <= endDate)
                .ToListAsync();
            
            analytics.ActiveTasks = tasks.Count(t => t.Status == TaskStatus.InProgress);
            analytics.CompletedTasks = tasks.Count(t => t.Status == TaskStatus.Completed);
            analytics.TaskCompletionRate = tasks.Count > 0 
                ? (decimal)analytics.CompletedTasks / tasks.Count * 100 
                : 0;

            return analytics;
        }
        #endregion
    }
} 