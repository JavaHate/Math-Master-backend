using JavaHateBE.model;
using Microsoft.EntityFrameworkCore;

namespace JavaHateBE.Data
{
    public partial class MathMasterDBContext : DbContext
    {
        public MathMasterDBContext(DbContextOptions<MathMasterDBContext> options) : base(options) { }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Score).IsRequired();
                entity.Property(g => g.StartTime).IsRequired();
                entity.Property(g => g.EndTime).IsRequired();
                entity.Property(g => g.UserId).IsRequired();
                entity.HasMany(g => g.Questions);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.LastLogin).IsRequired();
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Text).IsRequired();
                entity.Property(q => q.Answer).IsRequired();
                entity.Property(q => q.Difficulty).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}