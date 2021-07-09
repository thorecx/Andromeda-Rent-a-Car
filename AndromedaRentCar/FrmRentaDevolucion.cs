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
    public partial class FrmRentaDevolucion : Form
    {
        int? id;
        RentaDevolucion rentaDevolucion = null;
        public FrmRentaDevolucion()
        {
            InitializeComponent();
            LlenarVehiculos();
            LlenarEmpleados();
            LlenarClientes();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmRentaDevolucion_Load(object sender, EventArgs e)
        {
            Refrescar();

            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");

        }
        private void LlenarVehiculos()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {

                var data = db.Vehiculos.Where(x => x.Estado == true).Select(x => new { x.IdVehiculo, descVehiculo = x.DescVehiculo }).ToList();
                cbVehiculo.DataSource = data;
                cbVehiculo.DisplayMember = "descVehiculo";
                cbVehiculo.ValueMember = "IdVehiculo";
                if (cbVehiculo.Items.Count > 1)
                    cbVehiculo.SelectedIndex = -1;
            }
        }
        private void LlenarEmpleados()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {

                var data = db.Empleados.Where(x => x.Estado == true).Select(x => new { x.IdEmpleado, empleado = x.Nombre }).ToList();
                cbEmpleado.DataSource = data;
                cbEmpleado.DisplayMember = "empleado";
                cbEmpleado.ValueMember = "IdEmpleado";
                if (cbEmpleado.Items.Count > 1)
                    cbEmpleado.SelectedIndex = -1;
            }
        }
        private void LlenarClientes()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {

                var data = db.Clientes.Where(x => x.Estado == true).Select(x => new { x.IdClientes, cliente = x.Nombre }).ToList();
                cbCliente.DataSource = data;
                cbCliente.DisplayMember = "cliente";
                cbCliente.ValueMember = "IdClientes";
                if (cbCliente.Items.Count > 1)
                    cbCliente.SelectedIndex = -1;
            }
        }

        private void Refrescar()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.RentaDevolucions select d;

                DGRentaDevolucion.DataSource = data.ToList();
                DGRentaDevolucion.CurrentCell = null;
            }
        }
        private int? GetId()
        {
            try
            {
                return int.Parse(DGRentaDevolucion.Rows[DGRentaDevolucion.CurrentRow.Index].Cells[0].Value.ToString()); ;
            }
            catch
            {
                return null;
            }
        }
        private void EditarVehiculo(int? id, AndromedaRentCarEntities db)
        {

            rentaDevolucion = db.RentaDevolucions.Find(id);

            var vehiculo = db.Vehiculos.Where(x => x.Estado == true).Select(x => new { x.IdVehiculo, x.DescVehiculo }).ToList();
            var vehiculoSelected = db.Vehiculos.Where(w => w.IdVehiculo == rentaDevolucion.IdVehiculo).Select(x => new { x.IdVehiculo, x.DescVehiculo }).FirstOrDefault();

            vehiculo.Insert(0, vehiculoSelected);
            vehiculo = vehiculo.Distinct().ToList();

            cbVehiculo.DataSource = vehiculo;
            cbVehiculo.DisplayMember = "DescVehiculo";
            cbVehiculo.ValueMember = "IdVehiculo";
            cbVehiculo.SelectedItem = vehiculoSelected;

        }

        private void EditarEmpleado(int? id, AndromedaRentCarEntities db)
        {
            rentaDevolucion = db.RentaDevolucions.Find(id);

            var empleado = db.Empleados.Where(x => x.Estado == true).Select(x => new { x.IdEmpleado, x.Nombre }).ToList();
            var empleadoSelected = db.Empleados.Where(w => w.IdEmpleado == rentaDevolucion.IdEmpleado).Select(x => new { x.IdEmpleado, x.Nombre }).FirstOrDefault();

            empleado.Insert(0, empleadoSelected);
            empleado = empleado.Distinct().ToList();

            cbEmpleado.DataSource = empleado;
            cbEmpleado.DisplayMember = "Nombre";
            cbEmpleado.ValueMember = "IdEmpleado";
            cbEmpleado.SelectedItem = empleadoSelected;
        }

        private void Limpiar()
        {
            dtRenta.Value = DateTime.Now;
            dtDevolucion.Value = DateTime.Now;
            nMonto.Value = 0;
            nCantidad.Value = 0;
            txtComentario.Text = "";
        }

        private void Inhabilitar()
        {
            cbCliente.Enabled = false;
            cbEmpleado.Enabled = false;
            cbVehiculo.Enabled = false;
            cbEstado.Enabled = false;
            dtRenta.Enabled = false;
            dtDevolucion.Enabled = false;
            nMonto.Enabled = false;
            nCantidad.Enabled = false;
            txtComentario.Enabled = false;
        }

        private void Habilitar()
        {
            cbCliente.Enabled = true;
            cbEmpleado.Enabled = true;
            cbVehiculo.Enabled = true;
            cbEstado.Enabled = true;
            dtRenta.Enabled = true;
            dtDevolucion.Enabled = true;
            nMonto.Enabled = true;
            nCantidad.Enabled = true;
            txtComentario.Enabled = true;
        }

        private void EditarCliente(int? id, AndromedaRentCarEntities db)
        {
            rentaDevolucion = db.RentaDevolucions.Find(id);

            var cliente = db.Clientes.Where(x => x.Estado == true).Select(x => new { x.IdClientes, x.Nombre }).ToList();
            var clienteSelected = db.Clientes.Where(w => w.IdClientes == rentaDevolucion.IdCliente).Select(x => new { x.IdClientes, x.Nombre }).FirstOrDefault();

            cliente.Insert(0, clienteSelected);
            cliente = cliente.Distinct().ToList();

            cbCliente.DataSource = cliente;
            cbCliente.DisplayMember = "Nombre";
            cbCliente.ValueMember = "IdClientes";
            cbCliente.SelectedItem = clienteSelected;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                id = GetId();
                if (id == null)
                {
                    rentaDevolucion = new RentaDevolucion();

                }

                rentaDevolucion.IdEmpleado = int.Parse(cbEmpleado.SelectedValue.ToString());
                rentaDevolucion.IdCliente = int.Parse(cbCliente.SelectedValue.ToString());
                rentaDevolucion.IdVehiculo = int.Parse(cbVehiculo.SelectedValue.ToString());
                rentaDevolucion.FechaRenta = dtRenta.Value;
                rentaDevolucion.FechaDevolucion = dtDevolucion.Value;
                rentaDevolucion.MontoDia = int.Parse(nMonto.Value.ToString());
                rentaDevolucion.CantidadDias = int.Parse(nCantidad.Value.ToString());
                rentaDevolucion.Comentario = txtComentario.Text;
                if (cbEstado.SelectedItem.ToString() == "Activo")
                {
                    rentaDevolucion.Estado = true;
                }
                else
                {
                    rentaDevolucion.Estado = false;
                }

                if (id == null)
                    db.RentaDevolucions.Add(rentaDevolucion);
                else
                {
                    db.Entry(rentaDevolucion).State = System.Data.Entity.EntityState.Modified;
                }

                 db.SaveChanges();
            }

            Refrescar();
            Limpiar();
            Inhabilitar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            Habilitar();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {

                    rentaDevolucion = db.RentaDevolucions.Find(id);
                    dtRenta.Text = rentaDevolucion.FechaRenta.ToString();
                    dtDevolucion.Text = rentaDevolucion.FechaDevolucion.ToString();
                    nMonto.Value = Convert.ToDecimal(rentaDevolucion.MontoDia);
                    nCantidad.Value = Convert.ToDecimal(rentaDevolucion.CantidadDias);
                    txtComentario.Text = rentaDevolucion.Comentario;
                    if (rentaDevolucion.Estado == true)
                    {
                        cbEstado.SelectedIndex = 0;
                    }
                    else
                    {
                        cbEstado.SelectedIndex = 1;
                    }

                    EditarVehiculo(id, db);
                    EditarCliente(id, db);
                    EditarEmpleado(id, db);
                }

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DGRentaDevolucion.CurrentCell = null;
            Habilitar();
            Limpiar();
            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {
                    RentaDevolucion rentaDevolucion = db.RentaDevolucions.Find(id);
                    db.RentaDevolucions.Remove(rentaDevolucion);

                    db.SaveChanges();
                }
            }

            Refrescar();
        }
    }
}
