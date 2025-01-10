using Elastic.Clients.Elasticsearch;

namespace Budget.Helpers;

public class SearchService: ISearchService
{
    private readonly ElasticsearchClient _client;

    public SearchService(ElasticsearchClient client)
    {
        _client = client;
    }
    
    public async Task IndexAsync<T>(T entity, string indexName)
    {
        await _client.IndexAsync(entity, idx=>idx.Index(indexName));
    }

    public async Task<List<T>> SearchAsync<T>(string query, string indexName, int page, int pageSize)
    {
        var response = await _client.SearchAsync<T>(s => s
            .Index(indexName)
            .Query(q=> q 
                .Match(m => m 
                    .Field("name")
                    .Query(query)
                )
            )
            .From((page-1)*pageSize)
            .Size(pageSize)
        );
        if (response == null)
        {
            Console.WriteLine("Response is null.");
        }

        if (!response.IsValidResponse)
        {
            Console.WriteLine($"Search failed: {response.DebugInformation}");
        }
        
        if (response.HitsMetadata == null || response.Documents == null)
        {
            Console.WriteLine("No documents found.");
        }
        return response.Documents.ToList();
    }

    public async Task<List<dynamic>> SortAsync(IEnumerable<dynamic> data, string indexName)
    {
        throw new NotImplementedException();
    }
}