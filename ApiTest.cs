using System.Net;
using Refit;
using System.Text.Json;


namespace AutomatonV25;

public class ApiTest
{
    private readonly IClientApi _clientApi;

    public ApiTest()
    {
        _clientApi = RestService.For<IClientApi>("https://jsonplaceholder.typicode.com");;
    }
    
    [Test]
    public async Task TestGetPosts()
    {
        // Arrange
        
        // Act
        var response = _clientApi.GetPosts();
        
        // Assert
        await Assert.That(response.Result.StatusCode).IsEqualTo(HttpStatusCode.OK);
        
        var jsonString = response.Result.Content.ReadAsStringAsync().Result;
        await Assert.That(jsonString).IsNotEmpty();
        
        var posts = JsonSerializer.Deserialize<List<PostResponse>>(jsonString);
        await Assert.That(posts).IsNotNull();
        await Assert.That(posts.Count).IsGreaterThan(0);
        await Assert.That(posts.Count).IsEqualTo(100);
    }
    
    [Test]
    [Arguments(1)]
    [Arguments(100)]
    [Arguments(50)]
    public async Task TestGetPost(int postId)
    {
        // Arrange
        
        // Act
        var response = _clientApi.GetPost(postId);
        
        // Assert
        await Assert.That(response.Result.StatusCode).IsEqualTo(HttpStatusCode.OK);
        
        var jsonString = response.Result.Content.ReadAsStringAsync().Result;
        await Assert.That(jsonString).IsNotEmpty();
        
        var post = JsonSerializer.Deserialize<PostResponse>(jsonString);
        await Assert.That(post).IsNotNull();
        await Assert.That(post.id).IsEqualTo(postId);
    }
}