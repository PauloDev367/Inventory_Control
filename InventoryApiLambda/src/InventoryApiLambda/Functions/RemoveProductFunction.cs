namespace InventoryApiLambda.Functions;

public class RemoveProductFunction
{
    public async Task<string> FunctionHandler(int productId, ILambdaContext context)
    {
        var db = new DatabaseHelper();

        using var conn = db.GetConnection();
        await conn.OpenAsync();

        var query = "DELETE FROM Products WHERE Id = @Id";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Id", productId);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();

        return rowsAffected > 0 ? "Produto removido com sucesso!" : "Produto não encontrado.";
    }
}
