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
    public partial class FrmTipoVehiculo : Form
    {
        int? id;
        TipoVehiculo tipoVehiculo = null;
        public FrmTipoVehiculo()
        {
            InitializeComponent();

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmTipoVehiculo_Load(object sender, EventArgs e)
        {
            Refrescar();
            DGTipoVehiculo.SelectedRows[0].Selected = false;
            DGTipoVehiculo.CurrentCell = null;

            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");

        }

        private void Refrescar()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.TipoVehiculos select d;

                DGTipoVehiculo.DataSource = data.ToList();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                id = GetId();
                if(id == null)
                {
                tipoVehiculo = new TipoVehiculo();

                }

                tipoVehiculo.DescTipoVehiculo = tvDesc.Text;
                if(cbEstado.SelectedItem.ToString() == "Activo")
                {
                    tipoVehiculo.Estado = true;
                }
                else
                {
                    tipoVehiculo.Estado = false;
                }

                if(id == null)
                db.TipoVehiculos.Add(tipoVehiculo);
                else
                {
                    db.Entry(tipoVehiculo).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }

            Refrescar();
        }

        private int? GetId()
        {
            try
            {
                return int.Parse(DGTipoVehiculo.Rows[DGTipoVehiculo.CurrentRow.Index].Cells[0].Value.ToString()); ;
            }
            catch
            {
                return null;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetId();

            if(id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {
                    
                    tipoVehiculo = db.TipoVehiculos.Find(id);
                    tvDesc.Text = tipoVehiculo.DescTipoVehiculo;
                    if(tipoVehiculo.Estado == true)
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
            DGTipoVehiculo.SelectedRows[0].Selected = false;
            DGTipoVehiculo.CurrentCell = null;
            tvDesc.Text = "";
        }

        private void DGTipoVehiculo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if(id != null)
            {
                using(AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {
                    TipoVehiculo tipoVehiculo = db.TipoVehiculos.Find(id);
                    db.TipoVehiculos.Remove(tipoVehiculo);

                    db.SaveChanges();
                }
            }

            Refrescar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            using(AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.TipoVehiculos select d;
                if (!txtBuscar.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.DescTipoVehiculo.Contains(txtBuscar.Text.Trim()));
                }

                DGTipoVehiculo.DataSource = data.ToList();
            }
        }
    }
}
