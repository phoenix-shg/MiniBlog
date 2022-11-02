using Microsoft.AspNetCore.Mvc.Testing;
using MiniBlog.Model;
using MiniBlog.Stores;
using Moq;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Net;
using System.Text;
using Xunit;

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
public class ArticleControllerTest
{
    private IArticleStore _articleStore = new ArticleStoreContext();
    public ArticleControllerTest()
    {
        _articleStore.Save(new Article(null, "Happy new year", "Happy 2021 new year"));
        _articleStore.Save(new Article(null, "Happy Halloween", "Halloween is coming"));
    }

    [Fact]
    public async void Should_get_all_Article()
    {
        var client = GetClient();
        var response = await client.GetAsync("/article");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<Article>>(body);
        Assert.Equal(2, users.Count);
    }

    [Fact]
    public async void Should_create_article_fail_when_ArticleStore_unavailable()
    {
        var articleSoreMocker = new Mock<IArticleStore>();
        articleSoreMocker.Setup(store => store.Save(It.IsAny<Article>())).Throws<Exception>();
        var factory = new WebApplicationFactory<Program>();
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            services.AddSingleton(ServiceProvider => articleSoreMocker.Object));
        }).CreateClient();
        
        string userNameWhoWillAdd = "Tom";
        string articleContent = "What a good day today!";
        string articleTitle = "Good day";
        Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);

        var httpContent = JsonConvert.SerializeObject(article);
        StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/article", content);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async void Should_create_article_and_register_user_correct()
    {
        var client = GetClient();
        string userNameWhoWillAdd = "Tom";
        string articleContent = "What a good day today!";
        string articleTitle = "Good day";
        Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);

        var httpContent = JsonConvert.SerializeObject(article);
        StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
        var createArticleResponse = await client.PostAsync("/article", content);

        // It fail, please help
        Assert.Equal(HttpStatusCode.Created, createArticleResponse.StatusCode);

        var articleResponse = await client.GetAsync("/article");
        var body = await articleResponse.Content.ReadAsStringAsync();
        var articles = JsonConvert.DeserializeObject<List<Article>>(body);
        Assert.Equal(3, articles.Count);
        Assert.Equal(articleTitle, articles[2].Title);
        Assert.Equal(articleContent, articles[2].Content);
        Assert.Equal(userNameWhoWillAdd, articles[2].UserName);

        var userResponse = await client.GetAsync("/user");
        var usersJson = await userResponse.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<User>>(usersJson);

        Assert.Equal(1, users.Count);
        Assert.Equal(userNameWhoWillAdd, users[0].Name);
        Assert.Equal("anonymous@unknow.com", users[0].Email);
    }

    private HttpClient GetClient()
    {
        var factory = new WebApplicationFactory<Program>();
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            services.AddSingleton(ServiceProvider => _articleStore));
        }).CreateClient();
    }
}
}