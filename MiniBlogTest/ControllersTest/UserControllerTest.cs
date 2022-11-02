namespace MiniBlogTest.ControllerTest
{
    using System.Net;
    using System.Net.Mime;
    using System.Text;
    using Microsoft.AspNetCore.Mvc.Testing;
    using MiniBlog.Model;
    using MiniBlog.Stores;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    [Collection("IntegrationTest")]
    public class UserControllerTest
    {
        private IArticleStore _articleStore = new ArticleStoreContext();
        private IUserStore _userStore = new UserStoreContext();
        public UserControllerTest()
            : base()

        {
            UserStoreWillReplaceInFuture.Instance.Init();
            ArticleStoreWillReplaceInFuture.Instance.Init();
        }

        [Fact]
        public async Task Should_get_all_users()
        {
            var client = GetClient();
            var response = await client.GetAsync("/user");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(body);
            Assert.Equal(0, users.Count);
        }

        [Fact]
        public async Task Should_register_user_success()
        {
            var client = GetClient();

            var userName = "Tom";
            var email = "a@b.com";
            var user = new User(userName, email);
            var userJson = JsonConvert.SerializeObject(user);

            StringContent content = new StringContent(userJson, Encoding.UTF8, MediaTypeNames.Application.Json);
            var registerResponse = await client.PostAsync("/user", content);

            // It fail, please help
            Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);

            var users = await GetUsers(client);
            Assert.Single(users);
            Assert.Equal(email, users[0].Email);
            Assert.Equal(userName, users[0].Name);
        }

        [Fact]
        public async Task Should_register_user_fail_when_UserStore_unavailable()
        {
            var userSoreMocker = new Mock<IUserStore>();
            userSoreMocker.Setup(store => store.Save(It.IsAny<User>())).Throws<Exception>();
            var factory = new WebApplicationFactory<Program>();
            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                services.AddSingleton(ServiceProvider => userSoreMocker.Object));
            }).CreateClient();
            var userName = "Tom";
            var email = "a@b.com";
            var user = new User(userName, email);
            var userJson = JsonConvert.SerializeObject(user);

            StringContent content = new StringContent(userJson, Encoding.UTF8, MediaTypeNames.Application.Json);
            var registerResponse = await client.PostAsync("/user", content);
            Assert.Equal(HttpStatusCode.InternalServerError, registerResponse.StatusCode);
        }

        [Fact]
        public async Task Should_update_user_email_success_()
        {
            var client = GetClient();

            var userName = "Tom";
            var originalEmail = "a@b.com";
            var updatedEmail = "tom@b.com";
            var originalUser = new User(userName, originalEmail);

            var newUser = new User(userName, updatedEmail);
            StringContent registerUserContent = new StringContent(JsonConvert.SerializeObject(originalUser),
                Encoding.UTF8, MediaTypeNames.Application.Json);
            var registerResponse = await client.PostAsync("/user", registerUserContent);

            StringContent updateUserContent = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8,
                MediaTypeNames.Application.Json);
            await client.PutAsync("/user", updateUserContent);

            var users = await GetUsers(client);
            Assert.Equal(1, users.Count);
            Assert.Equal(updatedEmail, users[0].Email);
            Assert.Equal(userName, users[0].Name);
        }

        [Fact]
        public async Task Should_delete_user_and_related_article_success()
        {
            var client = GetClient();

            var userName = "Tom";

            await PrepareArticle(new Article(userName, string.Empty, string.Empty), client);
            await PrepareArticle(new Article(userName, string.Empty, string.Empty), client);

            var articles = await GetArticles(client);
            Assert.Equal(2, articles.Count);

            var users = await GetUsers(client);
            Assert.Equal(1, users.Count);

            await client.DeleteAsync($"/user?name={userName}");

            var articlesAfterDeleteUser = await GetArticles(client);
            Assert.Equal(0, articlesAfterDeleteUser.Count);

            var usersAfterDeleteUser = await GetUsers(client);
            Assert.Equal(0, usersAfterDeleteUser.Count);
        }

        private static async Task<List<User>> GetUsers(HttpClient client)
        {
            var response = await client.GetAsync("/user");
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(body);
            return users;
        }

        private static async Task<List<Article>> GetArticles(HttpClient client)
        {
            var articleResponse = await client.GetAsync("/article");
            var articlesJson = await articleResponse.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<List<Article>>(articlesJson);
            return articles;
        }

        private static async Task PrepareArticle(Article article1, HttpClient client)
        {
            StringContent registerUserContent = new StringContent(JsonConvert.SerializeObject(article1), Encoding.UTF8,
                MediaTypeNames.Application.Json);
            await client.PostAsync("/article", registerUserContent);
        }

        private HttpClient GetClient()
        {
            var factory = new WebApplicationFactory<Program>();
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(ServiceProvider => _articleStore);
                    services.AddSingleton(ServiceProvider => _userStore);
                });
            }).CreateClient();
        }
    }
}