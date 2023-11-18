using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using RealEstateApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RealEstateApi.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=RealEstateDb;");
            optionsBuilder.UseSqlServer(@"Server = tcp:realestatedbserver1.database.windows.net,1433; Initial Catalog = RealEstateDb; Persist Security Info = False; User ID = efernandes; Password =Ef13101988; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
            
        }
    }
}
