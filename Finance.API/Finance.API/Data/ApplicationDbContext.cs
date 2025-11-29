using Finance.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finance.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =======================
        // Таблицы Базы Данных
        // =======================
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<FinancialRule> FinancialRules { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // =======================
        // Конфигурация моделей
        // =======================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- ApplicationUser ---
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(x => x.FullName)
                      .HasMaxLength(100);
            });

            // --- Transaction ---
            builder.Entity<Transaction>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Amount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(x => x.Description)
                      .HasMaxLength(500);

                entity.HasOne(x => x.User)
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- FinancialRule ---
            builder.Entity<FinancialRule>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ThresholdAmount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(x => x.RuleName)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.HasOne(x => x.User)
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Notification ---
            builder.Entity<Notification>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Message)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(x => x.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(x => x.User)
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}