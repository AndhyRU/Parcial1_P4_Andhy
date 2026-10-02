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

        public async Task InitializeAsync()
        {
            string? ConectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqliteConnection(ConectionString);

            await connection.OpenAsync();

            string sql = "CREATE TABLE IF NOT EXISTS NumberRecords(\r\n\r\n          " +
                " Id INT PRIMARY KEY AUTOINCREMENT NOT NULL,\r\n\r\n          " +
                " Fecha DATETIME dateCurrentime NOT NULL,\r\n\r\n           " +
                "Numero INT NOT NULL,\r\n           \r\n           " +
                "Resultado INT NOT NULL,\r\n\r\n         );";

            await connection.ExecuteAsync(sql);

        }

        public async Task SaveAsync(NumberRecord record)
        {
            string? ConectionString = _configuration.GetConnectionString("DefaultConection");

            using var connection = new SqliteConnection(ConectionString);

            await connection.OpenAsync();

            string sql = "INSERT INTO NumberRecords (Numero, Resultado)\r\n" +
                "VALUES (@Numero, @Resultado);";

            await connection.ExecuteAsync(sql, record);
        }


        public async Task UpdateAsync(NumberRecord record)
        {
            string? ConectionString = _configuration.GetConnectionString("DefaultConection");

            using var connection = new SqliteConnection(ConectionString);

            await connection.OpenAsync();

            string sql = "UPDATE NumberRecords\r\n" +
                "SET Numero = @Numero,\r\n    " +
                "Resultado = @Resultado\r\n" +
                "WHERE Id = @Id;";

            await connection.ExecuteAsync(sql, record);
        }


        public async Task<NumberRecord?> GetByIdAsync(int id)
        {
            string? ConectionString = _configuration.GetConnectionString("DefaultConection");

            using var connection = new SqliteConnection(ConectionString);

            await connection.OpenAsync();

            string sql = 
                "SELECT Numero,\r\n" +
                "Resultado,\r\n" +
                "FROM NumberRecords\r\n;" +
                "WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<NumberRecord>(sql, new { Id = id }); ;
        }

        public async Task<IEnumerable<NumberRecord>> GetListAsync()
        {
            string? connectionString =
                _configuration.GetConnectionString("DefaultConection");

            using var connection =
                new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string sql = "SELECT * FROM NumberRecords;";

            return await connection.QueryAsync<NumberRecord>(sql);
        }
    }


}
