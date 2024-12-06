using System.Net.Http;
using System.Threading.Tasks;
using JavaHateBE.Data;
using JavaHateBE.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace JavaHateBE.Test.Integration
{
    public class QuestionIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public QuestionIntegrationTest(WebApplicationFactory<Program> factory)
        {
            
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.UseContentRoot("../../../");
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<MathMasterDBContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabaseQuestion");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<MathMasterDBContext>();

                        db.Database.EnsureCreated();

                        try
                        {
                            DatabaseSeeder.SeedDatabase(db);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }
                    }
                });
            });
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAllQuestions_ReturnsQuestions()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var questions = await response.Content.ReadAsAsync<List<Question>>();
            Assert.NotEmpty(questions);
        }

        [Fact]
        public async Task GetQuestionById_ReturnsQuestion()
        {
            // Arrange
            var getAllRequest = new HttpRequestMessage(HttpMethod.Get, "/question");
            var getAllResponse = await _client.SendAsync(getAllRequest);
            getAllResponse.EnsureSuccessStatusCode();

            var questions = await getAllResponse.Content.ReadAsAsync<List<Question>>();
            var firstQuestionId = questions.First().Id;

            var request = new HttpRequestMessage(HttpMethod.Get, $"/question/id/{firstQuestionId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var question = await response.Content.ReadAsAsync<Question>();
            Assert.Equal(firstQuestionId, question.Id);
        }

        [Fact]
        public async Task GetQuestionById_WithInvalidId_Returns404NotFound()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question/id/00000000-0000-0000-0000-000000000000");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetRandomQuestion_ReturnsQuestion()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question/random");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var questions = await response.Content.ReadAsAsync<List<Question>>();
            Assert.Single(questions);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(1)]
        [InlineData(15)]
        [InlineData(500)]
        public async Task GetRandomQuestion_WithAmount_ReturnsQuestions(int amount)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, $"/question/random?amount={amount}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var questions = await response.Content.ReadAsAsync<List<Question>>();
            Assert.Equal(amount, questions.Count);
        }

        [Fact]
        public async Task GetRandomQuestion_WithInvalidAmount_ReturnsBadRequest()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question/random?amount=invalid");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetRandomQuestion_WithNegativeAmount_ReturnsBadRequest()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question/random?amount=-1");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetRandomQuestion_WithZeroAmount_ReturnsBadRequest()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question/random?amount=0");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("What is 1 + 1?", 2, 1)]
        [InlineData("What is 2 + 2?", 4, 2)]
        [InlineData("What is 3 + 3?", 6, 3)]
        public async Task AddQuestion_ReturnsQuestion(string questionText, double answer, int difficulty)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/question")
            {
                Content = new StringContent("{\"text\":\"" + questionText + "\",\"answer\":" + answer + ",\"difficulty\":" + difficulty + "}")
            };
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var question = await response.Content.ReadAsAsync<Question>();
            Assert.Equal(questionText, question.Text);
            Assert.Equal(answer, question.Answer);
            Assert.Equal(difficulty, question.Difficulty);

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MathMasterDBContext>();

                var savedQuestion = await db.Questions.FindAsync(question.Id);
                Assert.NotNull(savedQuestion);
                Assert.Equal(questionText, savedQuestion.Text);
                Assert.Equal(answer, savedQuestion.Answer);
                Assert.Equal(difficulty, savedQuestion.Difficulty);
            }
        }

        [Fact]
        public async Task AddQuestion_WithDuplicatedText_ReturnsBadRequest() {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/question")
            {
                Content = new StringContent("{\"text\":\"What is 7 + 7?\",\"answer\":2,\"difficulty\":1}")
            };
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var request2 = new HttpRequestMessage(HttpMethod.Post, "/question")
            {
                Content = new StringContent("{\"text\":\"What is 7 + 7?\",\"answer\":2,\"difficulty\":1}")
            };
            request2.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.SendAsync(request);
            var response2 = await _client.SendAsync(request2);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response2.StatusCode);
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsQuestion()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var questions = await response.Content.ReadAsAsync<List<Question>>();
            var firstQuestion = questions.First();

            var updateRequest = new HttpRequestMessage(HttpMethod.Put, "/question")
            {
                Content = new StringContent("{\"id\":\"" + firstQuestion.Id + "\",\"text\":\"What is 8 + 1?\",\"answer\":9,\"difficulty\":1}")
            };
            updateRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Act
            var updateResponse = await _client.SendAsync(updateRequest);

            // Assert
            updateResponse.EnsureSuccessStatusCode();
            var updatedQuestion = await updateResponse.Content.ReadAsAsync<Question>();
            Assert.Equal("What is 8 + 1?", updatedQuestion.Text);
            Assert.Equal(9, updatedQuestion.Answer);
            Assert.Equal(1, updatedQuestion.Difficulty);

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MathMasterDBContext>();

                var savedQuestion = await db.Questions.FindAsync(firstQuestion.Id);
                Assert.NotNull(savedQuestion);
                Assert.Equal("What is 8 + 1?", savedQuestion.Text);
                Assert.Equal(9, savedQuestion.Answer);
                Assert.Equal(1, savedQuestion.Difficulty);
            }
        }

        [Fact]
        public async Task UpdateQuestion_WithDuplicatedText_ReturnsBadRequest()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var questions = await response.Content.ReadAsAsync<List<Question>>();
            var firstQuestion = questions.First();
            var secondQuestion = questions.Skip(1).First();

            var updateRequest = new HttpRequestMessage(HttpMethod.Put, "/question")
            {
                Content = new StringContent("{\"id\":\"" + secondQuestion.Id + "\",\"text\":\"" + firstQuestion.Text + "\",\"answer\":9,\"difficulty\":1}")
            };
            updateRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Act
            var updateResponse = await _client.SendAsync(updateRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, updateResponse.StatusCode);
        }

        [Fact]
        public async Task UpdateQuestion_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateRequest = new HttpRequestMessage(HttpMethod.Put, "/question")
            {
                Content = new StringContent("{\"id\":\"00000000-0000-0000-0000-000000000000\",\"text\":\"What is 8 + 1?\",\"answer\":9,\"difficulty\":1}")
            };
            updateRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Act
            var updateResponse = await _client.SendAsync(updateRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, updateResponse.StatusCode);
        }

        [Fact]
        public async Task RemoveQuestion_ReturnsQuestion()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/question");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var questions = await response.Content.ReadAsAsync<List<Question>>();
            var firstQuestion = questions.Skip(17).First();

            var removeRequest = new HttpRequestMessage(HttpMethod.Delete, $"/question/id/{firstQuestion.Id}");

            // Act
            var removeResponse = await _client.SendAsync(removeRequest);

            // Assert
            removeResponse.EnsureSuccessStatusCode();
            var removedQuestion = await removeResponse.Content.ReadAsAsync<Question>();
            Assert.Equal(firstQuestion.Id, removedQuestion.Id);

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MathMasterDBContext>();

                var savedQuestion = await db.Questions.FindAsync(firstQuestion.Id);
                Assert.Null(savedQuestion);
            }
        }

        [Fact]
        public async Task RemoveQuestion_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var removeRequest = new HttpRequestMessage(HttpMethod.Delete, "/question/id/00000000-0000-0000-0000-000000000000");

            // Act
            var removeResponse = await _client.SendAsync(removeRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, removeResponse.StatusCode);
        }
    }
}