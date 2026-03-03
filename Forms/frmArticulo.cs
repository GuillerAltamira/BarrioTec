using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TiendaApp.Data;

namespace TiendaApp.Forms {
    public class frmArticulos : Form {
        private TextBox txtNombre;
        private TextBox txtPrecio;
        private TextBox txtStock;
        private Button btnAgregar;
        private Button btnEliminar;
        private DataGridView dgvArticulos;

        public frmArticulos() {
            InicializarComponentes();
            CargarArticulos();
        }

        private void InicializarComponentes() {
            this.Text = "Gestión de Artículos.";
            this.Width = 600;
            this.Height = 400;

            Label lblNombre = new Label { Text = "Nombre:", Left = 20, Top = 20 };
            txtNombre = new TextBox { Left = 100, Top = 20, Width = 200 };

            Label lblPrecio = new Label { Text = "Precio:", Left = 20, Top = 60 };
            txtPrecio = new TextBox { Left = 100, Top = 60, Width = 200 };

            Label lblStock = new Label { Text = "Stock:", Left = 20, Top = 100 };
            txtStock = new TextBox { Left = 100, Top = 100, Width = 200 };

            btnAgregar = new Button { Text = "Agregar Artículo", Left = 320, Top = 20, Width = 150 };
            btnAgregar.Click += btnAgregar_Click;

            btnEliminar = new Button { Text = "Eliminar Seleccionado", Left = 320, Top = 60, Width = 150 };
            btnEliminar.Click += btnEliminar_Click;

            dgvArticulos = new DataGridView { Left = 20, Top = 140, Width = 540, Height = 180 };
            dgvArticulos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArticulos.MultiSelect = false;

            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblPrecio);
            this.Controls.Add(txtPrecio);
            this.Controls.Add(lblStock);
            this.Controls.Add(txtStock);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(btnEliminar);
            this.Controls.Add(dgvArticulos);
        }

        private void CargarArticulos() {
            using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Articulos", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvArticulos.DataSource = dt;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e) {
            using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Articulos (Nombre, Precio, Stock) VALUES (@n, @p, @s)", conn);
                cmd.Parameters.AddWithValue("@n", txtNombre.Text);
                cmd.Parameters.AddWithValue("@p", Convert.ToDecimal(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@s", Convert.ToInt32(txtStock.Text));
                cmd.ExecuteNonQuery();
            }
            CargarArticulos();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
        }

        private void btnEliminar_Click(object sender, EventArgs e) {
            if (dgvArticulos.SelectedRows.Count > 0) {
                int id = Convert.ToInt32(dgvArticulos.SelectedRows[0].Cells["Id"].Value);
                using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Articulos WHERE Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                CargarArticulos();
            }
        }
    }
}