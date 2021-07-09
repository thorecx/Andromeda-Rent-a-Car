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
    public partial class FrmModelos : Form
    {
        int? id;
        Modelo modelo = null;
        public FrmModelos()
        {
            InitializeComponent();
            LlenarMarcas();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmModelos_Load(object sender, EventArgs e)
        {
            Refrescar();
            DGModelos.CurrentCell = null;

            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");

        }

        private void Refrescar()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Modelos select d;

                DGModelos.DataSource = data.ToList();
            }
        }

        private void LlenarMarcas()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {

                var data = db.Marcas.Where(x => x.Estado == true).Select(x => new { x.IdMarca, descMarca = x.DescMarca }).ToList();
                cbMarca.DataSource = data;
                cbMarca.DisplayMember = "descMarca";
                cbMarca.ValueMember = "IdMarca";
                if (cbMarca.Items.Count > 1)
                    cbMarca.SelectedIndex = -1;
            }
        }

        private int? GetId()
        {
            try
            {
                return int.Parse(DGModelos.Rows[DGModelos.CurrentRow.Index].Cells[0].Value.ToString()); ;
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
                    modelo = new Modelo();

                }

                modelo.IdMarca = int.Parse(cbMarca.SelectedValue.ToString());
                modelo.DescModelos = mDesc.Text;
                if (cbEstado.SelectedItem.ToString() == "Activo")
                {
                    modelo.Estado = true;
                }
                else
                {
                    modelo.Estado = false;
                }

                if (id == null)
                    db.Modelos.Add(modelo);
                else
                {
                    db.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
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

                    modelo = db.Modelos.Find(id);
                    mDesc.Text = modelo.DescModelos;
                    cbMarca.SelectedIndex = modelo.IdMarca - 1;
                    if (modelo.Estado == true)
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
            DGModelos.CurrentCell = null;
            mDesc.Text = "";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {
                    Modelo modelo = db.Modelos.Find(id);
                    db.Modelos.Remove(modelo);

                    db.SaveChanges();
                }
            }

            Refrescar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Modelos select d;
                if (!txtBuscar.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.DescModelos.Contains(txtBuscar.Text.Trim()));
                }

                DGModelos.DataSource = data.ToList();
            }
        }
    }
}
