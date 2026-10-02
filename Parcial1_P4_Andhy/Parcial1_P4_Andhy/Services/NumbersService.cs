using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using Parcial1_P4_Andhy.Models;


namespace Parcial1_P4_Andhy.Services
{
    public class NumbersService
    {

        private readonly IConfiguration _configuration;

        public NumbersService(IConfiguration configuracion)
        {
            _configuration = configuracion;
        }

        // Crear la tabla.
        public async Task InitializeAsync()
        {
            string? connectionString =
                _configuration.GetConnectionString("DefaultConnection");

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

        // Guardar un cálculo.
        public async Task SaveAsync(NumberRecord record)
        {
            string? connectionString =
                _configuration.GetConnectionString("DefaultConnection");

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
        }

        // Actualizar un cálculo.
        public async Task UpdateAsync(NumberRecord record)
        {
            string? connectionString =
                _configuration.GetConnectionString("DefaultConnection");

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

        // Buscar un registro por Id.
        public async Task<NumberRecord?> GetByIdAsync(int id)
        {
            string? connectionString =
                _configuration.GetConnectionString("DefaultConnection");

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

        // Recuperar todo el historial.
        public async Task<IEnumerable<NumberRecord>> GetListAsync()
        {
            string? connectionString =
                _configuration.GetConnectionString("DefaultConnection");

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
