using Microsoft.EntityFrameworkCore;

namespace ConsultingApp.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<ProjectService> ProjectServices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-A928918\SQLEXPRESS;Database=Ppraktika;Trusted_Connection=True;Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Invoice>().ToTable("Invoices");
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<ProjectEmployee>().ToTable("ProjectEmployees");
            modelBuilder.Entity<ProjectService>().ToTable("ProjectServices");
            modelBuilder.Entity<Service>().ToTable("Services");
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.Client_ID);

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.Project_ID);

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.Employee_ID);

            modelBuilder.Entity<ProjectService>()
                .HasOne(ps => ps.Project)
                .WithMany(p => p.ProjectServices)
                .HasForeignKey(ps => ps.Project_ID);

            modelBuilder.Entity<ProjectService>()
                .HasOne(ps => ps.Service)
                .WithMany(s => s.ProjectServices)
                .HasForeignKey(ps => ps.Service_ID);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.Project_ID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
