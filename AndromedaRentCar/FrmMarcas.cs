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
    public partial class FrmMarcas : Form
    {
        int? id;
        Marca marcas = null;
        public FrmMarcas()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmMarcas_Load(object sender, EventArgs e)
        {
            Refrescar();
            DGMarcas.CurrentCell = null;

            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");
        }

        private void Refrescar()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Marcas select d;

                DGMarcas.DataSource = data.ToList();
            }
        }

        private int? GetId()
        {
            try
            {
                return int.Parse(DGMarcas.Rows[DGMarcas.CurrentRow.Index].Cells[0].Value.ToString()); ;
            }
            catch
            {
                return null;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                id = GetId();
                if (id == null)
                {
                    marcas = new Marca();

                }

                marcas.DescMarca = mDesc.Text;
                if (cbEstado.SelectedItem.ToString() == "Activo")
                {
                    marcas.Estado = true;
                }
                else
                {
                    marcas.Estado = false;
                }

                if (id == null)
                    db.Marcas.Add(marcas);
                else
                {
                    db.Entry(marcas).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }

            Refrescar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetId();

            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {

                    marcas = db.Marcas.Find(id);
                    mDesc.Text = marcas.DescMarca;
                    if (marcas.Estado == true)
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
            DGMarcas.CurrentCell = null;
            mDesc.Text = "";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
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
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Marcas select d;
                if (!txtBuscar.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.DescMarca.Contains(txtBuscar.Text.Trim()));
                }

                DGMarcas.DataSource = data.ToList();
            }
        }
    }
}
