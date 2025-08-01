using Grpc.Core;
using Npgsql;
using PriceContracts;

namespace PriceService.API.Services.gRPC;

public class PriceGrpcService(IConfiguration cfg) : PriceContracts.PriceService.PriceServiceBase
{
    private string ConnString => cfg.GetConnectionString("PriceDb")!;

    public override async Task<CreatePriceReply> CreatePrice(
        CreatePriceRequest r, ServerCallContext ctx)
    {
        const string sql =
            "INSERT INTO price (price_id, currency, value) VALUES ($1,$2,$3) " +
            "ON CONFLICT (price_id) DO NOTHING;";

        await using var c = new NpgsqlConnection(ConnString);
        await c.OpenAsync(ctx.CancellationToken);
        await using var cmd = new NpgsqlCommand(sql, c);
        cmd.Parameters.AddWithValue(Guid.Parse(r.PriceId));
        cmd.Parameters.AddWithValue(r.Currency);
        cmd.Parameters.AddWithValue(r.Value);

        var rows = await cmd.ExecuteNonQueryAsync(ctx.CancellationToken);
        return new CreatePriceReply { Success = rows > 0 };
    }
    public override async Task<GetPriceByIdReply> GetPriceById(
        GetPriceByIdRequest r, ServerCallContext ctx)
    {
        const string sql = "SELECT currency, value FROM price WHERE price_id = $1;";

        await using var c = new NpgsqlConnection(ConnString);
        await c.OpenAsync(ctx.CancellationToken);
        await using var cmd = new NpgsqlCommand(sql, c);
        cmd.Parameters.AddWithValue(Guid.Parse(r.PriceId));

        await using var reader = await cmd.ExecuteReaderAsync(ctx.CancellationToken);

        if (!await reader.ReadAsync(ctx.CancellationToken))
            return new GetPriceByIdReply { Found = false };

        return new GetPriceByIdReply {
            Currency = reader.GetString(0),
            Value    = (double)reader.GetDecimal(1),
            Found    = true
        };
    }

    public  async Task<DeletePriceReply> DeletePrice(DeletePriceRequest r, ServerCallContext ctx)
    {
        const string sql = "DELETE FROM price WHERE price_id = $1;";
        await using var c = new NpgsqlConnection(ConnString);
        await c.OpenAsync(ctx.CancellationToken);
        await using var cmd = new NpgsqlCommand(sql, c);
        cmd.Parameters.AddWithValue(Guid.Parse(r.PriceId));
        var rows =await cmd.ExecuteNonQueryAsync(ctx.CancellationToken);
        return new DeletePriceReply { Success = rows > 0 };
    }
}