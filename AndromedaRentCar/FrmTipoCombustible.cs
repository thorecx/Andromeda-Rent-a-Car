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
    public partial class FrmTipoCombustible : Form
    {
        int? id;
        TipoCombustible tipoCombustible = null;
        public FrmTipoCombustible()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmTipoCombustible_Load(object sender, EventArgs e)
        {
            Refrescar();
            DGTipoCombustible.CurrentCell = null;

            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");
        }
        private void Refrescar()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.TipoCombustibles select d;

                DGTipoCombustible.DataSource = data.ToList();
            }
        }

        private int? GetId()
        {
            try
            {
                return int.Parse(DGTipoCombustible.Rows[DGTipoCombustible.CurrentRow.Index].Cells[0].Value.ToString()); ;
            }
            catch
            {
                return null;
            }
        }

        private void btnGuardar_click(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                id = GetId();
                if (id == null)
                {
                    tipoCombustible = new TipoCombustible();

                }

                tipoCombustible.DescTipoCombustible= tcDesc.Text;
                if (cbEstado.SelectedItem.ToString() == "Activo")
                {
                    tipoCombustible.Estado = true;
                }
                else
                {
                    tipoCombustible.Estado = false;
                }

                if (id == null)
                    db.TipoCombustibles.Add(tipoCombustible);
                else
                {
                    db.Entry(tipoCombustible).State = System.Data.Entity.EntityState.Modified;
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

                    tipoCombustible = db.TipoCombustibles.Find(id);
                    tcDesc.Text = tipoCombustible.DescTipoCombustible;
                    if (tipoCombustible.Estado == true)
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {
                    TipoCombustible tipoCombustible = db.TipoCombustibles.Find(id);
                    db.TipoCombustibles.Remove(tipoCombustible);

                    db.SaveChanges();
                }
            }

            Refrescar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DGTipoCombustible.CurrentCell = null;
            tcDesc.Text = "";
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.TipoCombustibles select d;
                if (!txtBuscar.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.DescTipoCombustible.Contains(txtBuscar.Text.Trim()));
                }

                DGTipoCombustible.DataSource = data.ToList();
            }
        }
    }
}
