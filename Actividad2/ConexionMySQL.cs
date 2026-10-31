using System.Data;
using MySql.Data.MySqlClient;
namespace SistemaInventario;

public class ConexionMySQL
{
    // CAMBIA Pwd por tu contraseña de MySQL
    private readonly string cadenaConexion =
    "Server=ciberinfraestructura.cn2imkmi0b6p.us-east-2.rds.amazonaws.com;Port=3306;Database=SistemaInventario;Uid=admin;Pwd=ubjFQXxc0020;";

    public MySqlConnection ObtenerConexion() => new MySqlConnection(cadenaConexion);

    public bool ProbarConexion()
    {
        try { using var c = ObtenerConexion(); c.Open(); return true; }
        catch (MySqlException) { return false; }
    }

    public DataTable Consultar(string sql)
    {
        using var con = ObtenerConexion();
        con.Open();
        using var ad = new MySqlDataAdapter(sql, con);
        var tabla = new DataTable();
        ad.Fill(tabla);
        return tabla;
    }

    public int Ejecutar(string sql, params (string, object)[] parametros)
    {
        using var con = ObtenerConexion();
        con.Open();
        using var cmd = new MySqlCommand(sql, con);
        foreach (var (n, v) in parametros) cmd.Parameters.AddWithValue(n, v);
        return cmd.ExecuteNonQuery();
    }
}
