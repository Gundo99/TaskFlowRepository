using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _dbContext;

        public TaskRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(TaskItem taskItem)
        {
            await _dbContext.Tasks.AddAsync(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetById(Guid id)
        {
            return await _dbContext.Tasks.FindAsync(id);
        }

        public async Task<List<TaskItem>> GetByUserId(Guid userId)
        {
            return await _dbContext.Tasks
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task Update(TaskItem taskItem)
        {
            _dbContext.Tasks.Update(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(TaskItem taskItem)
        {
            _dbContext.Tasks.Remove(taskItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}