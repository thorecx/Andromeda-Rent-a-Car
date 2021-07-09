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
    public partial class FrmClientes : Form
    {
        int? id;
        Cliente cliente = null;
        public FrmClientes()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            Refrescar();
            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");

            cbTipoPersona.Items.Add("Fisica");
            cbTipoPersona.Items.Add("Juridica");
        }
        private void Refrescar()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Clientes select d;

                DGClientes.DataSource = data.ToList();
                DGClientes.CurrentCell = null;
            }
        }
        private int? GetId()
        {
            try
            {
                return int.Parse(DGClientes.Rows[DGClientes.CurrentRow.Index].Cells[0].Value.ToString()); ;
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
            txtTarjeta.Text = "";
            txtLC.Value = 0;
        }

        private void Inhabilitar()
        {
            txtNombre.Enabled = false;
            txtCedula.Enabled = false;
            txtTarjeta.Enabled = false;
            txtLC.Enabled = false;
            cbTipoPersona.Enabled = false;
            cbEstado.Enabled = false;
        }

        private void Habilitar()
        {
            txtNombre.Enabled = true;
            txtCedula.Enabled = true;
            txtTarjeta.Enabled = true;
            txtLC.Enabled = true;
            cbTipoPersona.Enabled = true;
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
                    cliente = new Cliente();

                }

                cliente.Nombre = txtNombre.Text;
                cliente.Cedula = txtCedula.Text;
                cliente.NoTarjetaCredito = txtTarjeta.Text;
                cliente.LimiteCredito = int.Parse(txtLC.Value.ToString());
                cliente.TipoPersona = cbTipoPersona.SelectedItem.ToString();
                if (cbEstado.SelectedItem.ToString() == "Activo")
                {
                    cliente.Estado = true;
                }
                else
                {
                    cliente.Estado = false;
                }

                if (id == null)
                    db.Clientes.Add(cliente);
                else
                {
                    db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
                }

                if (validaCedula(txtCedula.Text.ToString()))
                {
                db.SaveChanges();
                Refrescar();
                Limpiar();
                Inhabilitar();
                }
                else
                {
                    MessageBox.Show("Ingrese un formato de cedula valido");
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

                    cliente = db.Clientes.Find(id);
                    txtNombre.Text = cliente.Nombre;
                    txtCedula.Text = cliente.Cedula;
                    txtTarjeta.Text = cliente.NoTarjetaCredito;
                    txtLC.Value = Convert.ToDecimal(cliente.LimiteCredito);
                    cbTipoPersona.Text = cliente.TipoPersona;
                    if (cliente.Estado == true)
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
            DGClientes.CurrentCell = null;
            Limpiar();
            Habilitar();
            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {
                    Cliente cliente = db.Clientes.Find(id);
                    db.Clientes.Remove(cliente);

                    db.SaveChanges();
                }
            }

            Refrescar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Clientes select d;
                if (!txtBuscar.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.Nombre.Contains(txtBuscar.Text.Trim()));
                }

                DGClientes.DataSource = data.ToList();
            }
        }
    }
}
