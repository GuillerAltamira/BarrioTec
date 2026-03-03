using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TiendaApp.Data;
using TiendaApp.Models;
using TiendaApp.Controllers;

namespace TiendaApp.Forms {
    public class frmNuevaVenta : Form {
        private ComboBox cbClientes;
        private ComboBox cbArticulos;
        private TextBox txtCantidad;
        private Button btnAgregarArticulo;
        private Button btnRegistrarVenta;
        private DataGridView dgvDetalle;
        private Label lblTotal;

        private Venta ventaActual;

        public frmNuevaVenta() {
            InicializarComponentes();
            CargarClientes();
            CargarArticulos();
            ventaActual = new Venta();
        }

        private void InicializarComponentes() {
            this.Text = "Nueva Venta";
            this.Width = 700;
            this.Height = 500;

            Label lblCliente = new Label { Text = "Cliente:", Left = 20, Top = 20 };
            cbClientes = new ComboBox { Left = 100, Top = 20, Width = 200 };

            Label lblArticulo = new Label { Text = "Artículo:", Left = 20, Top = 60 };
            cbArticulos = new ComboBox { Left = 100, Top = 60, Width = 200 };

            Label lblCantidad = new Label { Text = "Cantidad:", Left = 20, Top = 100 };
            txtCantidad = new TextBox { Left = 100, Top = 100, Width = 100 };

            btnAgregarArticulo = new Button { Text = "Agregar Artículo", Left = 220, Top = 100, Width = 150 };
            btnAgregarArticulo.Click += btnAgregarArticulo_Click;

            dgvDetalle = new DataGridView { Left = 20, Top = 150, Width = 640, Height = 220 };
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.MultiSelect = false;

            lblTotal = new Label { Text = "Total: 0", Left = 20, Top = 380, Width = 200 };

            btnRegistrarVenta = new Button { Text = "Registrar Venta", Left = 500, Top = 380, Width = 150 };
            btnRegistrarVenta.Click += btnRegistrarVenta_Click;

            this.Controls.Add(lblCliente);
            this.Controls.Add(cbClientes);
            this.Controls.Add(lblArticulo);
            this.Controls.Add(cbArticulos);
            this.Controls.Add(lblCantidad);
            this.Controls.Add(txtCantidad);
            this.Controls.Add(btnAgregarArticulo);
            this.Controls.Add(dgvDetalle);
            this.Controls.Add(lblTotal);
            this.Controls.Add(btnRegistrarVenta);
        }

        private void CargarClientes() {
            using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Nombre FROM Clientes", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbClientes.DataSource = dt;
                cbClientes.DisplayMember = "Nombre";
                cbClientes.ValueMember = "Id";
            }
        }

        private void CargarArticulos() {
            using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Nombre, Precio FROM Articulos", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbArticulos.DataSource = dt;
                cbArticulos.DisplayMember = "Nombre";
                cbArticulos.ValueMember = "Id";
            }
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e) {
            if (cbArticulos.SelectedValue != null && int.TryParse(txtCantidad.Text, out int cantidad)) {
                DataRowView articulo = (DataRowView)cbArticulos.SelectedItem;
                int articuloId = (int)articulo["Id"];
                string nombre = articulo["Nombre"].ToString();
                decimal precio = (decimal)articulo["Precio"];
                decimal subtotal = precio * cantidad;

                ventaActual.Detalles.Add(new DetalleVenta {
                    ArticuloId = articuloId,
                    Cantidad = cantidad,
                    Subtotal = subtotal
                });

                dgvDetalle.Rows.Add(nombre, cantidad, precio, subtotal);

                ventaActual.Total += subtotal;
                lblTotal.Text = "Total: " + ventaActual.Total.ToString("C");
            }
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e) {
            if (cbClientes.SelectedValue != null && ventaActual.Detalles.Count > 0) {
                ventaActual.ClienteId = (int)cbClientes.SelectedValue;
                ventaActual.Fecha = DateTime.Now;

                VentaController controller = new VentaController();
                controller.RegistrarVenta(ventaActual);

                MessageBox.Show("Venta registrada correctamente.");
                this.Close();
            } else {
                MessageBox.Show("Seleccione un cliente y agregue artículos.");
            }
        }
    }
}
