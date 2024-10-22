using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OAuth2WebClient.Data;

public class ApplicationDbContext2 : DbContext
{
    public ApplicationDbContext2(DbContextOptions<ApplicationDbContext2> options)
        : base(options)
    {
    }

    public DbSet<Test> Tests { get; set; }
}

public class Test
{
    public int Id { get; set; }
}