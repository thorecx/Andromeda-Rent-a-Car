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
    public partial class FrmInspeccion : Form
    {
        int? id;
        Inspeccion inspeccion = null;
        public FrmInspeccion()
        {
            InitializeComponent();
            LlenarVehiculos();
            LlenarEmpleados();
            LlenarClientes();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void FrmInspeccion_Load(object sender, EventArgs e)
        {

            Refrescar();

            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");

            cbCantidadCombustible.Items.Add("1/4");
            cbCantidadCombustible.Items.Add("1/2");
            cbCantidadCombustible.Items.Add("3/4");
            cbCantidadCombustible.Items.Add("Lleno");


        }

        private void LlenarVehiculos()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                
                var data = db.Vehiculos.Where(x => x.Estado == true).Select(x => new { x.IdVehiculo, descVehiculo = x.DescVehiculo}).ToList();
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
                var data = from d in db.Inspeccions select d;

                DGInspeccion.DataSource = data.ToList();
                DGInspeccion.CurrentCell = null;
            }
        }

        private int? GetId()
        {
            try
            {
                return int.Parse(DGInspeccion.Rows[DGInspeccion.CurrentRow.Index].Cells[0].Value.ToString()); ;
            }
            catch
            {
                return null;
            }
        }

        private void EditarVehiculo(int? id, AndromedaRentCarEntities db)
        {

            inspeccion = db.Inspeccions.Find(id);

            var vehiculo = db.Vehiculos.Where(x => x.Estado == true).Select(x => new { x.IdVehiculo, x.DescVehiculo }).ToList();
            var vehiculoSelected = db.Vehiculos.Where(w => w.IdVehiculo == inspeccion.IdVehiculo).Select(x => new { x.IdVehiculo, x.DescVehiculo }).FirstOrDefault();

            vehiculo.Insert(0, vehiculoSelected);
            vehiculo = vehiculo.Distinct().ToList();

            cbVehiculo.DataSource = vehiculo;
            cbVehiculo.DisplayMember = "DescVehiculo";
            cbVehiculo.ValueMember = "IdVehiculo";
            cbVehiculo.SelectedItem = vehiculoSelected;

        }

        private void EditarEmpleado(int? id, AndromedaRentCarEntities db)
        {
            inspeccion = db.Inspeccions.Find(id);

            var empleado = db.Empleados.Where(x => x.Estado == true).Select(x => new { x.IdEmpleado, x.Nombre }).ToList();
            var empleadoSelected = db.Empleados.Where(w => w.IdEmpleado == inspeccion.IdEmpleado).Select(x => new { x.IdEmpleado, x.Nombre }).FirstOrDefault();

            empleado.Insert(0, empleadoSelected);
            empleado = empleado.Distinct().ToList();

            cbEmpleado.DataSource = empleado;
            cbEmpleado.DisplayMember = "Nombre";
            cbEmpleado.ValueMember = "IdEmpleado";
            cbEmpleado.SelectedItem = empleadoSelected;
        }

        private void EditarCliente(int? id, AndromedaRentCarEntities db)
        {
            inspeccion = db.Inspeccions.Find(id);

            var cliente = db.Clientes.Where(x => x.Estado == true).Select(x => new { x.IdClientes, x.Nombre }).ToList();
            var clienteSelected = db.Clientes.Where(w => w.IdClientes == inspeccion.IdCliente).Select(x => new { x.IdClientes, x.Nombre }).FirstOrDefault();

            cliente.Insert(0, clienteSelected);
            cliente = cliente.Distinct().ToList();

            cbCliente.DataSource = cliente;
            cbCliente.DisplayMember = "Nombre";
            cbCliente.ValueMember = "IdClientes";
            cbCliente.SelectedItem = clienteSelected;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                id = GetId();
                if (id == null)
                {
                    inspeccion = new Inspeccion();

                }

                inspeccion.IdVehiculo = Convert.ToInt32(cbVehiculo.SelectedValue.ToString());
                inspeccion.IdEmpleado = int.Parse(cbEmpleado.SelectedValue.ToString());
                inspeccion.IdCliente = int.Parse(cbCliente.SelectedValue.ToString());
                inspeccion.CantCombustible = cbCantidadCombustible.Text;
                inspeccion.Fecha = dtFecha.Value;
                if (cbEstado.SelectedItem.ToString() == "Activo")
                {
                    inspeccion.Estado = true;
                }
                else
                {
                    inspeccion.Estado = false;
                }
                if (chRalladuras.Checked)
                {
                    inspeccion.Ralladuras = true;
                }
                else
                {
                    inspeccion.Ralladuras = false;
                }
                if (chGato.Checked)
                {
                    inspeccion.Gato = true;
                }
                else
                {
                    inspeccion.Gato = false;
                }
                if (chGomaRepuesto.Checked)
                {
                    inspeccion.GomaRepuesto = true;
                }
                else
                {
                    inspeccion.GomaRepuesto = false;
                }
                if (chRoturaCristal.Checked)
                {
                    inspeccion.RoturaCristal = true;
                }
                else
                {
                    inspeccion.RoturaCristal = false;
                }
                if (chGomasDelanteras.Checked)
                {
                    inspeccion.GomasDelanteras = true;
                }
                else
                {
                    inspeccion.GomasDelanteras = false;
                }
                if (chGomasTraseras.Checked)
                {
                    inspeccion.GomasTraseras = true;
                }
                else
                {
                    inspeccion.GomasTraseras = false;
                }

                if (id == null)
                    db.Inspeccions.Add(inspeccion);
                else
                {
                    db.Entry(inspeccion).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }

            Refrescar();
            }
            catch(NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetId();

            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {

                    inspeccion = db.Inspeccions.Find(id);
                    cbCantidadCombustible.Text = inspeccion.CantCombustible;
                    dtFecha.Text = inspeccion.Fecha.ToString();
                    if (inspeccion.Estado == true)
                    {
                        cbEstado.SelectedIndex = 0;
                    }
                    else
                    {
                        cbEstado.SelectedIndex = 1;
                    }
                    if (inspeccion.Ralladuras == true)
                    {
                        chRalladuras.Checked = true;
                    }
                    else
                    {
                        chRalladuras.Checked = false;
                    }
                    if (inspeccion.Gato == true)
                    {
                        chGato.Checked = true;
                    }
                    else
                    {
                        chGato.Checked = false;
                    }
                    if (inspeccion.GomaRepuesto == true)
                    {
                        chGomaRepuesto.Checked = true;
                    }
                    else
                    {
                        chGomaRepuesto.Checked = false;
                    }
                    if (inspeccion.RoturaCristal == true)
                    {
                        chRoturaCristal.Checked = true;
                    }
                    else
                    {
                        chRoturaCristal.Checked = false;
                    }
                    if (inspeccion.GomasDelanteras == true)
                    {
                        chGomasDelanteras.Checked = true;
                    }
                    else
                    {
                        chGomasDelanteras.Checked = false;
                    }
                    if (inspeccion.GomasTraseras == true)
                    {
                        chGomasTraseras.Checked = true;
                    }
                    else
                    {
                        chGomasTraseras.Checked = false;
                    }

                    EditarCliente(id, db);
                    EditarEmpleado(id, db);
                    EditarVehiculo(id, db);
                }

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DGInspeccion.CurrentCell = null;

            dtFecha.Value = DateTime.Now;
            chRalladuras.Checked = false;
            chGato.Checked = false;
            chGomaRepuesto.Checked = false;
            chRoturaCristal.Checked = false;
            chGomasDelanteras.Checked = false;
            chGomasTraseras.Checked = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
             int? id = GetId();
             if (id != null)
             {
                 using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                 {
                     Inspeccion inspeccion = db.Inspeccions.Find(id);
                     db.Inspeccions.Remove(inspeccion);

                     db.SaveChanges();
                 }
             }

             Refrescar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Inspeccions select d;

                if (!txtBuscar.Text.Trim().Equals(""))
                {
                    int idIns = int.Parse(txtBuscar.Text.ToString());
                    data = data.Where(d => d.IdInspeccion == idIns);
                }

                DGInspeccion.DataSource = data.ToList();
            }
        }
    }
}
