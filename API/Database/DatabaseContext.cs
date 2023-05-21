using Microsoft.EntityFrameworkCore;

namespace ChrisUsher.MoveMate.API.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
}