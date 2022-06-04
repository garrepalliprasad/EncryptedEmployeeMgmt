using Microsoft.EntityFrameworkCore;

namespace EncryptedEmployeeMgmt.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public  DbSet<Employee> Employees { get; set; }
        public  DbSet<MonthlySalary> Salaries { get; set; }
    }
}
