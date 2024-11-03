using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

//Classe Post
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}

public class ApiService
{
    private static readonly HttpClient httpClient = new HttpClient();

    // Método GET
    public async Task<List<Post>> GetPosts()
    {
        var url = "https://jsonplaceholder.typicode.com/posts";

        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<List<Post>>(json);
            return posts;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Erro ao acessar a API: {e.Message}");
            return new List<Post>();
        }
    }

    // Método POST
    public async Task<Post> CreatePost(Post newPost)
    {
        var url = "https://jsonplaceholder.typicode.com/posts";

        try
        {
            var json = JsonConvert.SerializeObject(newPost);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var createdPost = JsonConvert.DeserializeObject<Post>(responseData);
            return createdPost;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Erro ao criar post: {e.Message}");
            return null;
        }
    }

    // Método DELETE
    public async Task<bool> DeletePost(int postId)
    {
        var url = $"https://jsonplaceholder.typicode.com/posts/1";

        try
        {
            var response = await httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Erro ao remover post: {e.Message}");
            return false;
        }
    }
}

public class FileService
{
    // Método salvar em JSON
    public void SavePosts(string filePath, List<Post> posts)
    {
        var json = JsonConvert.SerializeObject(posts, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    // Método ler de JSON
    public List<Post> LoadPosts(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Arquivo não encontrado.");
            return new List<Post>();
        }

        var json = File.ReadAllText(filePath);
        var posts = JsonConvert.DeserializeObject<List<Post>>(json);
        return posts;
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var apiService = new ApiService();
        var fileService = new FileService();
        string filePath = "posts.json";

        // Obtendo posts
        List<Post> posts = await apiService.GetPosts();
        foreach (var post in posts) Console.WriteLine($"ID: {post.Id}, Título: {post.Title}");

        // Salvando posts em um arquivo JSON
        fileService.SavePosts(filePath, posts);
        Console.WriteLine($"Posts salvos em {filePath}");

        // Criando um novo post
        var newPost = new Post { Title = "Novo Post", Body = "Este é o conteúdo do novo post." };
        Post createdPost = await apiService.CreatePost(newPost);
        if (createdPost != null) Console.WriteLine($"Post criado com ID: {createdPost.Id}");

        // Removendo um post
        bool deleteSuccess = await apiService.DeletePost(createdPost.Id);
        Console.WriteLine(deleteSuccess ? "Post removido com sucesso." : "Falha ao remover post.");

        // Lendo posts do arquivo JSON
        var loadedPosts = fileService.LoadPosts(filePath);
        foreach (var post in loadedPosts) Console.WriteLine($"ID: {post.Id}, Título: {post.Title}");  
    }
}
