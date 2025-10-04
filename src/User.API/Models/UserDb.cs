using Microsoft.EntityFrameworkCore;

namespace BackendAPI.User.API.Models;

class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options)
        : base(options) { }

    public DbSet<UserItem> Users => Set<UserItem>();
}
