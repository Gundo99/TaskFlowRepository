namespace TaskFlow.Domain.Users
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User?> GetById(Guid id);
        Task<User?> GetByEmail(string email);
        Task Update(User user);
        Task Delete(User user);
        Task<List<User>> GetAll();

        Task<List<User>> GetPaged(int pageNumber, int pageSize, string? search, string? sortBy, string? sortDirection);
        Task<int> Count(string? search);
    }
}