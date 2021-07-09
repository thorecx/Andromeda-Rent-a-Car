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
    public partial class FrmEmpleados : Form
    {
        int? id;
        Empleado empleado = null;
        public FrmEmpleados()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmEmpleados_Load(object sender, EventArgs e)
        {
            Refrescar();

            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");

            cbTanda.Items.Add("Matutina");
            cbTanda.Items.Add("Vespertina");
        }

        private void Refrescar()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Empleados select d;

                DGEmpleados.DataSource = data.ToList();
                DGEmpleados.CurrentCell = null;
            }
        }

        private int? GetId()
        {
            try
            {
                return int.Parse(DGEmpleados.Rows[DGEmpleados.CurrentRow.Index].Cells[0].Value.ToString()); ;
            }
            catch
            {
                return null;
            }
        }

        private void Limpiar()
        {
            txtNombre.Text = "";
            txtCedula.Text = "";
            nComision.Value = 0;
            dtFechaIngreso.Value = DateTime.Now;
        }

        private void Inhabilitar()
        {
            txtNombre.Enabled = false;
            txtCedula.Enabled = false;
            cbTanda.Enabled = false;
            nComision.Enabled = false;
            dtFechaIngreso.Enabled = false;
            cbEstado.Enabled = false;
        }

        private void Habilitar()
        {
            txtNombre.Enabled = true;
            txtCedula.Enabled = true;
            cbTanda.Enabled = true;
            nComision.Enabled = true;
            dtFechaIngreso.Enabled = true;
            cbEstado.Enabled = true;
        }
        public static bool validaCedula(string pCedula)

        {
            int vnTotal = 0;
            string vcCedula = pCedula.Replace("-", "");
            int pLongCed = vcCedula.Trim().Length;
            int[] digitoMult = new int[11] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };

            if (pLongCed < 11 || pLongCed > 11)
                return false;

            for (int vDig = 1; vDig <= pLongCed; vDig++)
            {
                int vCalculo = Int32.Parse(vcCedula.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];
                if (vCalculo < 10)
                    vnTotal += vCalculo;
                else
                    vnTotal += Int32.Parse(vCalculo.ToString().Substring(0, 1)) + Int32.Parse(vCalculo.ToString().Substring(1, 1));
            }

            if (vnTotal % 10 == 0)
                return true;
            else
                return false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                id = GetId();
                if (id == null)
                {
                    empleado = new Empleado();

                }

                empleado.Nombre = txtNombre.Text;
                empleado.Cedula = txtCedula.Text;
                empleado.Tanda = cbTanda.SelectedItem.ToString();
                empleado.PorcientoComision = int.Parse(nComision.Value.ToString());
                empleado.FechaIngreso = dtFechaIngreso.Value;
                if (cbEstado.SelectedItem.ToString() == "Activo")
                {
                    empleado.Estado = true;
                }
                else
                {
                    empleado.Estado = false;
                }

                if (id == null)
                    db.Empleados.Add(empleado);
                else
                {
                    db.Entry(empleado).State = System.Data.Entity.EntityState.Modified;
                }

                if (validaCedula(txtCedula.Text.ToString()))
                {
                db.SaveChanges();
                Refrescar();
                Inhabilitar();
                Limpiar();
                }
                else
                {
                    MessageBox.Show("Ingrese un formato de cedula valida");
                }
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            Habilitar();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {

                    empleado = db.Empleados.Find(id);
                    txtNombre.Text = empleado.Nombre;
                    txtCedula.Text = empleado.Cedula;
                    cbTanda.Text = empleado.Tanda;
                    nComision.Value = Convert.ToDecimal(empleado.PorcientoComision);
                    dtFechaIngreso.Text = empleado.FechaIngreso.ToString();
                    if (empleado.Estado == true)
                    {
                        cbEstado.SelectedIndex = 0;
                    }
                    else
                    {
                        cbEstado.SelectedIndex = 1;
                    }
                }

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DGEmpleados.CurrentCell = null;

            Habilitar();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {
                    Empleado empleado = db.Empleados.Find(id);
                    db.Empleados.Remove(empleado);

                    db.SaveChanges();
                }
            }

            Refrescar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Empleados select d;
                if (!txtBuscar.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.Nombre.Contains(txtBuscar.Text.Trim()));
                }

                DGEmpleados.DataSource = data.ToList();
            }
        }
    }
}
