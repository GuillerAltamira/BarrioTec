using System;
using System.Windows.Forms;
using TiendaApp.Forms;

namespace TiendaApp {
    public class frmMenuPrincipal : Form {
        private Button btnClientes;
        private Button btnArticulos;
        private Button btnNuevaVenta;
        private Button btnHistorial;

        public frmMenuPrincipal() {
            InicializarComponentes();
        }

        private void InicializarComponentes() {
            this.Text = "Menú Principal - TiendaApp";
            this.Width = 400;
            this.Height = 300;

            btnClientes = new Button { Text = "Clientes", Left = 50, Top = 50, Width = 120 };
            btnClientes.Click += (s, e) => { new frmClientes().ShowDialog(); };

            btnArticulos = new Button { Text = "Artículos", Left = 200, Top = 50, Width = 120 };
            btnArticulos.Click += (s, e) => { new frmArticulos().ShowDialog(); };

            btnNuevaVenta = new Button { Text = "Nueva Venta", Left = 50, Top = 120, Width = 120 };
            btnNuevaVenta.Click += (s, e) => { new frmVenta().ShowDialog(); };

            btnHistorial = new Button { Text = "Historial Ventas", Left = 200, Top = 120, Width = 120 };
            btnHistorial.Click += (s, e) => { new frmVentas().ShowDialog(); };

            this.Controls.Add(btnClientes);
            this.Controls.Add(btnArticulos);
            this.Controls.Add(btnNuevaVenta);
            this.Controls.Add(btnHistorial);
        }
    }
}

