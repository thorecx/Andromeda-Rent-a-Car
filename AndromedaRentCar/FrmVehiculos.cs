using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndromedaRentCar
{
    public partial class FrmVehiculos : Form
    {
        int? id;
        Vehiculo vehiculo = null;
        public FrmVehiculos()
        {
            InitializeComponent();
            LlenarMarcas();
            LlenarVehiculos();
            LlenarModelos();
            LlenarTipoCombustible();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmVehiculos_Load(object sender, EventArgs e)
        {
            Refrescar();
            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");

        }
        private void Refrescar()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Vehiculos select d;

                DGVehiculos.DataSource = data.ToList();
                DGVehiculos.CurrentCell = null;

            }
        }
        private int? GetId()
        {
            try
            {
                return int.Parse(DGVehiculos.Rows[DGVehiculos.CurrentRow.Index].Cells[0].Value.ToString()); ;
            }
            catch
            {
                return null;
            }
        }

        private void Limpiar()
        {
            vDesc.Text = "";
            txtChasis.Text = "";
            txtMotor.Text = "";
            txtPlaca.Text = "";
        }

        private void Inhabilitar()
        {
            vDesc.Enabled = false;
            txtChasis.Enabled = false;
            txtMotor.Enabled = false;
            txtPlaca.Enabled = false;
            cbTipoCombustible.Enabled = false;
            cbMarca.Enabled = false;
            cbModelo.Enabled = false;
            cbTipoVehiculo.Enabled = false;
            cbEstado.Enabled = false;
        }

        private void Habilitar()
        {
            vDesc.Enabled = true;
            txtChasis.Enabled = true;
            txtMotor.Enabled = true;
            txtPlaca.Enabled = true;
            cbTipoCombustible.Enabled = true;
            cbMarca.Enabled = true;
            cbModelo.Enabled = true;
            cbTipoVehiculo.Enabled = true;
            cbEstado.Enabled = true;
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

        private void LlenarVehiculos()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = db.TipoVehiculos.Where(x => x.Estado == true).Select(x => new { x.IdTipoVehiculo, descTipoVehiculo = x.DescTipoVehiculo }).ToList();
                cbTipoVehiculo.DataSource = data;
                cbTipoVehiculo.DisplayMember = "descTipoVehiculo";
                cbTipoVehiculo.ValueMember = "IdTipoVehiculo";
                if (cbTipoVehiculo.Items.Count > 1)
                    cbTipoVehiculo.SelectedIndex = -1;
            }
        }
        private void LlenarModelos()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = db.Modelos.Where(x => x.Estado == true).Select(x => new { x.IdModelos, descModelos = x.DescModelos }).ToList();
                cbModelo.DataSource = data;
                cbModelo.DisplayMember = "descModelos";
                cbModelo.ValueMember = "IdModelos";
                if (cbModelo.Items.Count > 1)
                    cbModelo.SelectedIndex = -1;
            }
        }
        private void LlenarTipoCombustible()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = db.TipoCombustibles.Where(x => x.Estado == true).Select(x => new { x.IdTipoCombustible, descTipoCombustible = x.DescTipoCombustible }).ToList();
                cbTipoCombustible.DataSource = data;
                cbTipoCombustible.DisplayMember = "descTipoCombustible";
                cbTipoCombustible.ValueMember = "IdTipoCombustible";
                if (cbTipoCombustible.Items.Count > 1)
                    cbTipoCombustible.SelectedIndex = -1;
            }
        }

        private void EditarMarca(int? id, AndromedaRentCarEntities db)
        {
            
                vehiculo = db.Vehiculos.Find(id);
            
             var marcas = db.Marcas.Where(x => x.Estado == true).Select(x => new { x.IdMarca, x.DescMarca }).ToList();
             var marcaSelected = db.Marcas.Where(w => w.IdMarca == vehiculo.IdMarca).Select(x => new { x.IdMarca, x.DescMarca }).FirstOrDefault();

             marcas.Insert(0, marcaSelected);
             marcas = marcas.Distinct().ToList();

             cbMarca.DataSource = marcas;
             cbMarca.DisplayMember = "DescMarca";
             cbMarca.ValueMember = "IdMarca";
             cbMarca.SelectedItem = marcaSelected;
            
        }

        private void EditarTipoVehiculo(int? id, AndromedaRentCarEntities db)
        {
            vehiculo = db.Vehiculos.Find(id);

            var tipoVehiculo = db.TipoVehiculos.Where(x => x.Estado == true).Select(x => new { x.IdTipoVehiculo, x.DescTipoVehiculo }).ToList();
            var tipoVehiculoSelected = db.TipoVehiculos.Where(w => w.IdTipoVehiculo == vehiculo.IdTipoVehiculo).Select(x => new { x.IdTipoVehiculo, x.DescTipoVehiculo }).FirstOrDefault();

            tipoVehiculo.Insert(0, tipoVehiculoSelected);
            tipoVehiculo = tipoVehiculo.Distinct().ToList();

            cbTipoVehiculo.DataSource = tipoVehiculo;
            cbTipoVehiculo.DisplayMember = "DescTipoVehiculo";
            cbTipoVehiculo.ValueMember = "IdTipoVehiculo";
            cbTipoVehiculo.SelectedItem = tipoVehiculoSelected;
        }

        private void EditarModelo(int? id, AndromedaRentCarEntities db)
        {
            vehiculo = db.Vehiculos.Find(id);

            var modelo = db.Modelos.Where(x => x.Estado == true).Select(x => new { x.IdModelos, x.DescModelos }).ToList();
            var modeloSelected = db.Modelos.Where(w => w.IdModelos == vehiculo.IdModelo).Select(x => new { x.IdModelos, x.DescModelos }).FirstOrDefault();

            modelo.Insert(0, modeloSelected);
            modelo = modelo.Distinct().ToList();

            cbModelo.DataSource = modelo;
            cbModelo.DisplayMember = "DescModelos";
            cbModelo.ValueMember = "IdModelos";
            cbModelo.SelectedItem = modeloSelected;
        }

        private void EditarTipoCombustible(int? id, AndromedaRentCarEntities db)
        {
            vehiculo = db.Vehiculos.Find(id);

            var tipoCombustible = db.TipoCombustibles.Where(x => x.Estado == true).Select(x => new { x.IdTipoCombustible, x.DescTipoCombustible }).ToList();
            var tipoCombustibleSelected = db.TipoCombustibles.Where(w => w.IdTipoCombustible == vehiculo.IdTipoCombustible).Select(x => new { x.IdTipoCombustible, x.DescTipoCombustible }).FirstOrDefault();

            tipoCombustible.Insert(0, tipoCombustibleSelected);
            tipoCombustible = tipoCombustible.Distinct().ToList();

            cbTipoCombustible.DataSource = tipoCombustible;
            cbTipoCombustible.DisplayMember = "DescTipoCombustible";
            cbTipoCombustible.ValueMember = "IdTipoCombustible";
            cbTipoCombustible.SelectedItem = tipoCombustibleSelected;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                id = GetId();
                if (id == null)
                {
                    vehiculo = new Vehiculo();

                }

                vehiculo.DescVehiculo = vDesc.Text;
                vehiculo.NoChasis = txtChasis.Text;
                vehiculo.NoMotor = txtMotor.Text;
                vehiculo.NoPlaca = txtPlaca.Text;
                vehiculo.IdTipoVehiculo = int.Parse(cbTipoVehiculo.SelectedValue.ToString());
                vehiculo.IdTipoCombustible = int.Parse(cbTipoCombustible.SelectedValue.ToString());
                vehiculo.IdMarca = int.Parse(cbMarca.SelectedValue.ToString());
                vehiculo.IdModelo = int.Parse(cbModelo.SelectedValue.ToString());
                if (cbEstado.SelectedItem.ToString() == "Activo")
                {
                    vehiculo.Estado = true;
                }
                else
                {
                    vehiculo.Estado = false;
                }

                if (id == null)
                    db.Vehiculos.Add(vehiculo);
                else
                {
                    db.Entry(vehiculo).State = System.Data.Entity.EntityState.Modified;
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
                    vehiculo = db.Vehiculos.Find(id);

                    vDesc.Text = vehiculo.DescVehiculo.ToString();
                    txtMotor.Text = vehiculo.NoMotor.ToString();
                    txtChasis.Text = vehiculo.NoChasis.ToString();
                    txtPlaca.Text = vehiculo.NoPlaca.ToString();
                    if (vehiculo.Estado == true)
                    {
                        cbEstado.SelectedIndex = 0;
                    }
                    else
                    {
                        cbEstado.SelectedIndex = 1;
                    }

                    EditarTipoVehiculo(id, db);
                    EditarMarca(id, db);
                    EditarModelo(id, db);
                    EditarTipoCombustible(id, db);

                }

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DGVehiculos.CurrentCell = null;
            Habilitar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
                {
                    Vehiculo vehiculo = db.Vehiculos.Find(id);
                    db.Vehiculos.Remove(vehiculo);

                    db.SaveChanges();
                }
            }

            Refrescar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                var data = from d in db.Vehiculos select d;
                if (!txtBuscar.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.DescVehiculo.Contains(txtBuscar.Text.Trim()));
                }

                DGVehiculos.DataSource = data.ToList();
            }
        }
    }
}
