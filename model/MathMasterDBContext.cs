using System.Text.Json;
using JavaHateBE.Data;
using Microsoft.EntityFrameworkCore;

namespace JavaHateBE.model
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
            var users = new List<User>();
            var userDataList = ReadJsonFile<List<UserData>>("Data/SeedData/users.json");
            foreach (var userData in userDataList)
            {
                users.Add(new User(userData.Name, userData.Password, userData.Email));
            }
            modelBuilder.Entity<User>().HasData(users);
        }

        private void SeedQuestions(ModelBuilder modelBuilder)
        {
            var Questions = new List<Question>();
            var questionDataList = ReadJsonFile<List<QuestionData>>("Data/SeedData/questions.json");
            foreach (var questionData in questionDataList)
            {
                Questions.Add(new Question(questionData.Text, questionData.Answer, questionData.Difficulty));
            }
            modelBuilder.Entity<Question>().HasData(Questions);
        }

        private T ReadJsonFile<T>(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var result = JsonSerializer.Deserialize<T>(jsonString) ?? throw new InvalidOperationException($"Failed to deserialize JSON from file: {filePath}");
            return result;
        }
    }
}