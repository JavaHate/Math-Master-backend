using Microsoft.EntityFrameworkCore;

namespace JavaHateBE.model
{
     public partial class SampleDBContext : DbContext
     {
       public SampleDBContext(DbContextOptions <SampleDBContext> options) : base(options) {}
       public virtual DbSet<Game> Games { get; set; }
       public virtual DbSet<User> Users { get; set; }
       public virtual DbSet<Question> Questions { get; set; }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.Entity<Game>(entity =>
           {
               entity.HasKey(g => g.Id);
               entity.Property(g => g.Score).IsRequired();
               entity.Property(g => g.startTime).IsRequired();
               entity.Property(g => g.endTime).IsRequired();
               entity.Property(g => g.endTime).IsRequired();
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
           SeedUsers(modelBuilder);
           SeedQuestions(modelBuilder);
       }
       partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

       private void SeedUsers(ModelBuilder modelBuilder)
       {
           var users = new List<User>
           {
               new User("David", "Password1", "david@example.com"),
               new User("Robbe", "Password2", "robbe@example.com"),
               new User("Valdemar", "Password3", "valdemar@example.com"),
               new User("Matas", "Password4", "matas@example.com")
           };
           modelBuilder.Entity<User>().HasData(users);
       }

       private void SeedQuestions(ModelBuilder modelBuilder)
       {
           var Questions = new List<Question>();
           for(int i = 1; i <= 15; i++)
           {
               for(int j = 0; j < 15; j++)
               {
                   Questions.Add(new Question($"{i} + {j}", i + j, 1));
               }
           }
           modelBuilder.Entity<Question>().HasData(Questions);
       }
     }
}