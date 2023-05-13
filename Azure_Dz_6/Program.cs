// See https://aka.ms/new-console-template for more information
using Azure_Dz_6;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using System.ComponentModel;
using System.Text.Json;
using Container = Microsoft.Azure.Cosmos.Container;

string endpointURL = "YourUrl";
string accountKey = "YourKey";
string databaseId = "blogDatabase";
string containerId = "blogContainer";
Blog blog = createBlog();

try
{
    Database database = await CreateAndGetDatabaseAsync(endpointURL, accountKey, databaseId);
    Container container = await CreateAndGetContainerAsync(database, containerId);

    //Запись

    //await WriteDataAsync(blog, container);

    //Чтение
    await QueryItemAsync(container);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}


async Task<Database> CreateAndGetDatabaseAsync(string endpoint,string accountKey,string databaseId)
{
    CosmosClient cosmosClient = new CosmosClient(endpoint, accountKey);
    Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
    return database;
}

async Task<Container> CreateAndGetContainerAsync(Database database, string containerId)
{
    Container container = await database.CreateContainerIfNotExistsAsync(containerId, partitionKeyPath: "/Name");
    return container;
}

async Task WriteDataAsync(Blog blog, Container container)
{
    try
    {
        ItemResponse<Blog> response = await container.CreateItemAsync(blog, partitionKey: new Microsoft.Azure.Cosmos.PartitionKey(blog.Name));
        Console.WriteLine($"Item {response.Resource.Id} was added");
    }
    catch(CosmosException cex)
    when (cex.StatusCode == System.Net.HttpStatusCode.Conflict)
    {
        Console.WriteLine("This el is in data base");
    }
}
Blog createBlog()
{
    Blog blog = new Blog
    {
        Id = Guid.NewGuid().ToString(),
        Name = "Blog about Cloud",
        BestComment = new BestComment()
        {
            Text = "I love Clouds"
        },
    };
    return blog;
}
async Task QueryItemAsync(Container container)
{
    string query = "SELECT * FROM c";
    QueryDefinition queryDefinition = new QueryDefinition(query);
    FeedIterator<Blog> feedIterator = container.GetItemQueryIterator<Blog>(queryDefinition);
    List<Blog> blogs = new List<Blog>();
    while (feedIterator.HasMoreResults)
    {
        FeedResponse<Blog> feedResponse = await feedIterator.ReadNextAsync();
        foreach (Blog blog in feedResponse)
        {
            blogs.Add(blog);
            Console.WriteLine($"\t{blog}");
        }
    }
}
