using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TiendaApp.Data;

namespace TiendaApp.Forms {
    public class frmClientes : Form {
        private TextBox txtNombre;
        private TextBox txtNIT;
        private Button btnAgregar;
        private Button btnEliminar;
        private DataGridView dgvClientes;

        public frmClientes() {
            InicializarComponentes();
            CargarClientes();
        }

        private void InicializarComponentes() {
            this.Text = "Gestión de Clientes";
            this.Width = 600;
            this.Height = 400;

            Label lblNombre = new Label { Text = "Nombre:", Left = 20, Top = 20 };
            txtNombre = new TextBox { Left = 100, Top = 20, Width = 200 };

            Label lblNIT = new Label { Text = "NIT:", Left = 20, Top = 60 };
            txtNIT = new TextBox { Left = 100, Top = 60, Width = 200 };

            btnAgregar = new Button { Text = "Agregar Cliente", Left = 320, Top = 20, Width = 150 };
            btnAgregar.Click += btnAgregar_Click;

            btnEliminar = new Button { Text = "Eliminar Seleccionado", Left = 320, Top = 60, Width = 150 };
            btnEliminar.Click += btnEliminar_Click;

            dgvClientes = new DataGridView { Left = 20, Top = 100, Width = 540, Height = 220 };
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;

            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblNIT);
            this.Controls.Add(txtNIT);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(btnEliminar);
            this.Controls.Add(dgvClientes);
        }

        private void CargarClientes() {
            using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Clientes", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvClientes.DataSource = dt;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e) {
            using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Clientes (Nombre, NIT) VALUES (@n, @nit)", conn);
                cmd.Parameters.AddWithValue("@n", txtNombre.Text);
                cmd.Parameters.AddWithValue("@nit", txtNIT.Text);
                cmd.ExecuteNonQuery();
            }
            CargarClientes();
            txtNombre.Clear();
            txtNIT.Clear();
        }

        private void btnEliminar_Click(object sender, EventArgs e) {
            if (dgvClientes.SelectedRows.Count > 0) {
                int id = Convert.ToInt32(dgvClientes.SelectedRows[0].Cells["Id"].Value);
                using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Clientes WHERE Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                CargarClientes();
            }
        }
    }
}
