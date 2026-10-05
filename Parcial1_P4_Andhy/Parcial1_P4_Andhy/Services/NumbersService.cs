using Dapper;
using Microsoft.Data.Sqlite;
using Parcial1_P4_Andhy.Models;

namespace Parcial1_P4_Andhy.Services;

public class NumbersService(IConfiguration config)
{
    private readonly string _connectionString =
        config.GetConnectionString("DefaultConnection")!;

    private SqliteConnection CreateConnection =>
        new SqliteConnection(_connectionString);


    public async Task InitializeAsync()
    {
        const string query = @"CREATE TABLE IF NOT EXISTS Numeros (" +
            " Id INTEGER PRIMARY KEY AUTOINCREMENT," +
            " Fecha TEXT NOT NULL," +
            " Numero INTEGER NOT NULL," +
            " Resultado INTEGER NOT NULL);";

        using var conexion = CreateConnection;

        await conexion.ExecuteAsync(query);
    }


    public async Task<bool> SaveAsync(NumberRecordSet number)
    {
        const string query =
            @"INSERT INTO Numeros (Fecha, Numero, Resultado)" +
            " VALUES (DATETIME('now'), @Numero, @Resultado)";

        using var conexion = CreateConnection;

        int filasAfectadas =
            await conexion.ExecuteAsync(query, number);

        return filasAfectadas > 0;
    }


    public async Task<bool> UpdateAsync(int Id, NumberRecordSet number)
    {
        const string query =
            @"UPDATE Numeros SET" +
            " Fecha = DATETIME('now')," +
            " Numero = @Numero," +
            " Resultado = @Resultado" +
            " WHERE Id = @Id";

        using var conexion = CreateConnection;

        int filasAfectadas =
            await conexion.ExecuteAsync(query, new
            {
                Id,
                number.Numero,
                number.Resultado
            });

        return filasAfectadas > 0;
    }


    public async Task<NumberRecordGet?> GetByIdAsync(int Id)
    {
        const string query =
            "SELECT Id, Fecha, Numero, Resultado" +
            " FROM Numeros WHERE Id = @Id";

        using var conexion = CreateConnection;

        var registro = await conexion
            .QueryFirstOrDefaultAsync(query, new { Id });

        if (registro == null)
        {
            return null;
        }

        return new NumberRecordGet(
            Convert.ToInt32(registro.Id),
            DateTime.Parse(registro.Fecha.ToString()),
            Convert.ToInt32(registro.Numero),
            Convert.ToInt32(registro.Resultado)
        );
    }


    public async Task<IEnumerable<NumberRecordGet>> GetListAsync()
    {
        const string query =
            "SELECT Id, Fecha, Numero, Resultado" +
            " FROM Numeros";

        using var conexion = CreateConnection;

        var registros = await conexion.QueryAsync(query);

        return registros.Select(registro =>
            new NumberRecordGet(
                Convert.ToInt32(registro.Id),
                DateTime.Parse(registro.Fecha.ToString()),
                Convert.ToInt32(registro.Numero),
                Convert.ToInt32(registro.Resultado)
            )
        );
    }
}