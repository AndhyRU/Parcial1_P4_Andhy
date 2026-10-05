using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using Parcial1_P4_Andhy.Models;


namespace Parcial1_P4_Andhy.Services
{
    public class NumbersService(IConfiguration configuration)
    {
        
        public async Task InitializeAsync()
        {
            string? connectionString =
                configuration.GetConnectionString("DefaultConnection");

            using var connection =
                new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string sql = """
                CREATE TABLE IF NOT EXISTS NumberRecords (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Fecha TEXT DEFAULT CURRENT_TIMESTAMP NOT NULL,
                    Numero INTEGER NOT NULL,
                    Resultado INTEGER NOT NULL
                );
                """;

            await connection.ExecuteAsync(sql);
        }

        
        public async Task SaveAsync(NumberRecord record)
        {
            string? connectionString =
                configuration.GetConnectionString("DefaultConnection");

            using var connection =
                new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string sql = """
                INSERT INTO NumberRecords
                    (Fecha, Numero, Resultado)
                VALUES
                    (@Fecha, @Numero, @Resultado);
                """;

            await connection.ExecuteAsync(sql, new
            {
                Fecha = record.Fecha.ToString("O"),
                record.Numero,
                record.Resultado
            });

            record.Id = await connection.ExecuteScalarAsync<int>(
             "SELECT last_insert_rowid();");
        }

        
        public async Task UpdateAsync(NumberRecord record)
        {
            string? connectionString =
                configuration.GetConnectionString("DefaultConnection");

            using var connection =
                new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string sql = """
                UPDATE NumberRecords
                SET Fecha = @Fecha,
                    Numero = @Numero,
                    Resultado = @Resultado
                WHERE Id = @Id;
                """;

            await connection.ExecuteAsync(sql, new
            {
                record.Id,
                Fecha = record.Fecha.ToString("O"),
                record.Numero,
                record.Resultado
            });
        }

        
        public async Task<NumberRecord?> GetByIdAsync(int id)
        {
            string? connectionString =
                configuration.GetConnectionString("DefaultConnection");

            using var connection =
                new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string sql = """
                SELECT *
                FROM NumberRecords
                WHERE Id = @Id;
                """;

            return await connection
                .QueryFirstOrDefaultAsync<NumberRecord>(
                    sql, new { Id = id });
        }

        
        public async Task<IEnumerable<NumberRecord>> GetListAsync()
        {
            string? connectionString =
                configuration.GetConnectionString("DefaultConnection");

            using var connection =
                new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string sql = """
                SELECT *
                FROM NumberRecords
                ORDER BY Id DESC;
                """;

            return await connection.QueryAsync<NumberRecord>(sql);
        }
    }


}
