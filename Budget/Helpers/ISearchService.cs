namespace Budget.Helpers;

public interface ISearchService
{
    Task IndexAsync<T>(T entity, string indexName);
    Task<List<T>> SearchAsync<T>(string query, string indexName, int page, int pageSize);
    Task<List<dynamic>> SortAsync(IEnumerable<dynamic> data, string indexName);
}