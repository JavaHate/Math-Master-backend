using System.Text.Json;
using JavaHateBE.Data.JsonTransformers;
using JavaHateBE.Model;
using Microsoft.EntityFrameworkCore;

namespace JavaHateBE.Data
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(DbContext context)
        {
            // Clear existing data
            context.Set<User>().RemoveRange(context.Set<User>());
            context.Set<Question>().RemoveRange(context.Set<Question>());
            context.Set<Game>().RemoveRange(context.Set<Game>());
            context.SaveChanges();

            // Seed new data
            SeedUsers(context);
            SeedQuestions(context);
            context.SaveChanges();
        }

        private static void SeedUsers(DbContext context)
        {
            var users = new List<User>();
            var userDataList = ReadJsonFile<List<UserData>>("Data/SeedData/users.json");
            foreach (var userData in userDataList)
            {
                users.Add(new User(userData.Name, userData.Password, userData.Email));
            }
            context.Set<User>().AddRange(users);
        }

        private static void SeedQuestions(DbContext context)
        {
            var questions = new List<Question>();
            var questionDataList = ReadJsonFile<List<QuestionData>>("Data/SeedData/questions.json");
            foreach (var questionData in questionDataList)
            {
                questions.Add(new Question(questionData.Text, questionData.Answer, questionData.Difficulty));
            }
            context.Set<Question>().AddRange(questions);
        }

        private static T ReadJsonFile<T>(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var result = JsonSerializer.Deserialize<T>(jsonString);
            if (result == null)
            {
                throw new InvalidOperationException($"Failed to deserialize JSON from file: {filePath}");
            }
            return result;
        }
    }
}