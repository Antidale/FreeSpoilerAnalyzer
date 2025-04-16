using System.Reflection;
using SQLite;

namespace FreeSpoilerAnalyzer;

public class DatabaseHelper
{
    private readonly string _dbPath = Path.Combine("/Users/antidale/Desktop", "spoiler-data.db");
    private readonly SQLiteAsyncConnection _dbConnection;

    public SQLiteAsyncConnection DbConnection => _dbConnection;

    public DatabaseHelper()
    {
        _dbConnection = new SQLiteAsyncConnection(_dbPath);
    }

    public async Task InitDatabaseAsync()
    {
        var entities = Assembly.GetExecutingAssembly().GetTypes().Where(t => t is { IsClass: true, Namespace: "FreeSpoilerAnalyzer.Models" }).ToArray();
        await _dbConnection.CreateTablesAsync(CreateFlags.None, entities);
    }
}