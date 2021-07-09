using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndromedaRentCar
{
    public partial class FrmReportes : Form
    {
        public FrmReportes()
        {
            InitializeComponent();
            LlenarClientes();
            LlenarEmpleados();
            LlenarVehiculos();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void LlenarVehiculos()
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {

                var data = db.Vehiculos.Select(x => new { x.IdVehiculo, descVehiculo = x.DescVehiculo }).ToList();
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

                var data = db.Empleados.Select(x => new { x.IdEmpleado, empleado = x.Nombre }).ToList();
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

                var data = db.Clientes.Select(x => new { x.IdClientes, cliente = x.Nombre }).ToList();
                cbCliente.DataSource = data;
                cbCliente.DisplayMember = "cliente";
                cbCliente.ValueMember = "IdClientes";
                if (cbCliente.Items.Count > 1)
                    cbCliente.SelectedIndex = -1;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (AndromedaRentCarEntities db = new AndromedaRentCarEntities())
            {
                //int? idVehiculo = int.Parse(cbVehiculo.SelectedValue.ToString());
                //int? idEmpleado = int.Parse(cbEmpleado.SelectedValue.ToString());
                //int? idCliente = int.Parse(cbCliente.SelectedValue.ToString());
                DateTime desde = dtDesde.Value;
                DateTime hasta = dtHasta.Value;
                //bool estado;

                //var data = db.RentaDevolucions.Where(x => (x.FechaRenta >= desde && x.FechaRenta <= hasta)).ToList();
                var data = (from RentaDevolucion in db.RentaDevolucions
                            join Empleado in db.Empleados
                            on RentaDevolucion.IdEmpleado equals Empleado.IdEmpleado
                            join Vehiculo in db.Vehiculos
                            on RentaDevolucion.IdVehiculo equals Vehiculo.IdVehiculo
                            join Cliente in db.Clientes
                            on RentaDevolucion.IdCliente equals Cliente.IdClientes
                            where DbFunctions.TruncateTime(RentaDevolucion.FechaRenta) >= DbFunctions.TruncateTime(desde) &&
                                  DbFunctions.TruncateTime(RentaDevolucion.FechaDevolucion) <= DbFunctions.TruncateTime(hasta)
                            select new
                            {
                                IdRenta = RentaDevolucion.IdRenta,
                                FechaRenta = RentaDevolucion.FechaRenta,
                                FechaDevolucion = RentaDevolucion.FechaDevolucion,
                                MontoDias = RentaDevolucion.MontoDia,
                                CantidadDias = RentaDevolucion.CantidadDias,
                                NombreCliente = Cliente.Nombre,
                                NombreEmpleado = Empleado.Nombre,
                                DescVehiculo = Vehiculo.DescVehiculo,
                                Estado = RentaDevolucion.Estado,
                                Comentario = RentaDevolucion.Comentario
                            }).AsQueryable();

                if (!cbVehiculo.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.DescVehiculo.Contains(cbVehiculo.Text.Trim()));
                }
                if (!cbCliente.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.NombreCliente.Contains(cbCliente.Text.Trim()));
                }
                if (!cbEmpleado.Text.Trim().Equals(""))
                {
                    data = data.Where(d => d.NombreEmpleado.Contains(cbEmpleado.Text.Trim()));
                }
                if (cbEstado.Text.Trim().Equals("Activo"))
                {
                    data = data.Where(d => d.Estado == true);
                }
                else if(cbEstado.Text.Trim().Equals("Inactivo"))
                {
                    data = data.Where(d => d.Estado == false);
                }
                DGRentaDevolucion.DataSource = data.ToList();
            }
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            Refrescar();
            cbEstado.Items.Add("Activo");
            cbEstado.Items.Add("Inactivo");

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

            int i = 1;
        private void btnExportar_Click(object sender, EventArgs e)
        {
            SLDocument sl = new SLDocument();
            int ic = 1;
            foreach(DataGridViewColumn column in DGRentaDevolucion.Columns)
            {
                sl.SetCellValue(1, ic, column.HeaderText.ToString());
                ic++;
            }

            int ir = 2;
            foreach(DataGridViewRow row in DGRentaDevolucion.Rows)
            {
                sl.SetCellValue(ir, 1, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 2, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[3].Value.ToString());
                sl.SetCellValue(ir, 5, row.Cells[4].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[5].Value.ToString());
                sl.SetCellValue(ir, 7, row.Cells[6].Value.ToString());
                sl.SetCellValue(ir, 8, row.Cells[7].Value.ToString());
                sl.SetCellValue(ir, 9, row.Cells[8].Value.ToString());
                sl.SetCellValue(ir, 10, row.Cells[9].Value.ToString());
                ir++;
            }
            sl.SaveAs(@"C:\Users\hecto\OneDrive\Documentos\UNAPEC\Cuarto cuatrimestre\Desarrollo de Software con Tecnologia Propietaria\archivos\reportes"+ i +".xlsx");
            i++;
            MessageBox.Show("Datos Exportados Correctamente!!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refrescar();
        }
    }
}
