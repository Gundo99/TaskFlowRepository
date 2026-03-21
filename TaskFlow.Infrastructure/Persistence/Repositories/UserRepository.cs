using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Users;

namespace TaskFlow.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetById(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email.Value == email);
        }
        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<List<User>> GetPaged(int pageNumber, int pageSize, string? search, string? sortBy, string? sortDirection)
        {
            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim().ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(searchTerm) ||
                    x.Email.Value.ToLower().Contains(searchTerm));
            }

            var sortField = sortBy?.Trim().ToLower();
            var direction = sortDirection?.Trim().ToLower();

            query = (sortField, direction) switch
            {
                ("email", "desc") => query.OrderByDescending(x => x.Email.Value),
                ("email",_) => query.OrderBy(x => x.Email.Value),
                ("name", "desc") => query.OrderByDescending(x => x.Name),
                ("name", _) => query.OrderBy(x => x.Name),
                _ => query.OrderBy(x => x.Name)
            };

            return await query
                .Skip((pageNumber -1)* pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> Count(string? search)
        {
            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim().ToLower();

                query = query.Where(x =>
                    x.Name.ToLower().Contains(searchTerm) ||
                    x.Email.Value.ToLower().Contains(searchTerm));
            }
            return await query.CountAsync();
        }

        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}