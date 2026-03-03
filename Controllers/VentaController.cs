using System;
using System.Data.SqlClient;
using TiendaApp.Data;
using TiendaApp.Models;

namespace TiendaApp.Controllers {
    public class VentaController {
        public void RegistrarVenta(Venta venta) {
            using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try {
                    // Insertar Cabecera
                    string sqlVenta = "INSERT INTO Ventas (ClienteId, Total) OUTPUT INSERTED.Id VALUES (@c, @t)";
                    SqlCommand cmdV = new SqlCommand(sqlVenta, conn, tran);
                    cmdV.Parameters.AddWithValue("@c", venta.ClienteId);
                    cmdV.Parameters.AddWithValue("@t", venta.Total);
                    int ventaId = (int)cmdV.ExecuteScalar();

                    // Insertar Detalles
                    foreach (var d in venta.Detalles) {
                        string sqlDetalle = "INSERT INTO DetalleVentas (VentaId, ArticuloId, Cantidad, Subtotal) VALUES (@v, @a, @can, @s)";
                        SqlCommand cmdD = new SqlCommand(sqlDetalle, conn, tran);
                        cmdD.Parameters.AddWithValue("@v", ventaId);
                        cmdD.Parameters.AddWithValue("@a", d.ArticuloId);
                        cmdD.Parameters.AddWithValue("@can", d.Cantidad);
                        cmdD.Parameters.AddWithValue("@s", d.Subtotal);
                        cmdD.ExecuteNonQuery();

                        // Actualizar Stock
                        string sqlStock = "UPDATE Articulos SET Stock = Stock - @can WHERE Id = @a";
                        SqlCommand cmdS = new SqlCommand(sqlStock, conn, tran);
                        cmdS.Parameters.AddWithValue("@can", d.Cantidad);
                        cmdS.Parameters.AddWithValue("@a", d.ArticuloId);
                        cmdS.ExecuteNonQuery();
                    }

                    tran.Commit();
                } catch (Exception) {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}
