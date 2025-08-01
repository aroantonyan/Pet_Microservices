using Npgsql;

namespace PriceService.Infrastructure.Persistence;

public class StartupDbInitializer
{
    private const string CreatePriceTable = """
                                                CREATE TABLE IF NOT EXISTS Price(
                                                price_id   UUID PRIMARY KEY,
                                                currency   TEXT NOT NULL,
                                                value      NUMERIC(10,2) NOT NULL);
                                            """;

    public static async Task CheckPriceTable(string connectionString, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync(ct);
        await using var cmd = new NpgsqlCommand(CreatePriceTable,conn);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}