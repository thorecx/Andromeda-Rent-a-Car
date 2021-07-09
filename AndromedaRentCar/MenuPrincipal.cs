using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndromedaRentCar
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FrmReportes reportes = new FrmReportes();
            reportes.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTipoVehiculo_Click(object sender, EventArgs e)
        {
            FrmTipoVehiculo tipoVehiculo = new FrmTipoVehiculo();
            tipoVehiculo.ShowDialog();
        }

        private void btnInspeccion_Click(object sender, EventArgs e)
        {
            FrmInspeccion inspeccion = new FrmInspeccion();
            inspeccion.ShowDialog();
        }

        private void btnMarcas_Click(object sender, EventArgs e)
        {
            FrmMarcas marcas = new FrmMarcas();
            marcas.ShowDialog();
        }

        private void btnTipoCombustible_Click(object sender, EventArgs e)
        {
            FrmTipoCombustible tipoCombustible = new FrmTipoCombustible();
            tipoCombustible.ShowDialog();
        }

        private void btnVehiculos_Click(object sender, EventArgs e)
        {
            FrmVehiculos vehiculos = new FrmVehiculos();
            vehiculos.ShowDialog();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            FrmClientes clientes = new FrmClientes();
            clientes.ShowDialog();
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            FrmEmpleados empleados = new FrmEmpleados();
            empleados.ShowDialog();
        }

        private void btnModelos_Click(object sender, EventArgs e)
        {
            FrmModelos modelos = new FrmModelos();
            modelos.ShowDialog();
        }

        private void btnRentaDevolucion_Click(object sender, EventArgs e)
        {
            FrmRentaDevolucion rentaDevolucion = new FrmRentaDevolucion();
            rentaDevolucion.ShowDialog();
        }
    }
}
