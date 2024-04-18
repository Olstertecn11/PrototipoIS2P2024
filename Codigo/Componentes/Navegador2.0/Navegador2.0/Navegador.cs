using Navegador2._0.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Navegador2._0
{
    public partial class Navegador : UserControl
    {
        private utilidadesConsultasI utilConsultasI;
        public string operacion = "";
        public string tabla = "";
        public int filaActual = 0;
        public bool gridExiste = false;
        string tabla_cmb = "";
        DataGridView data_invisible = new DataGridView();
        public DataTable mydata = new DataTable();
        public Seguridad_Controlador.Controlador ctrl_seguridad = new Seguridad_Controlador.Controlador();
        public Form parent;
        public string idApp;
        public Navegador()
        {
            InitializeComponent();
            this.parent = new Form();
            this.utilConsultasI = new utilidadesConsultasI();
            this.cambiarEstado(false);
            string encriptado = this.SetHash("12345");
            bool login = ctrl_seguridad.validarLogin("admin", encriptado);
        }

        public string SetHash(string inputString)
        {
            string hash = "x2";
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(inputString);
            MD5 mD5 = MD5.Create();
            TripleDES tripleDES = TripleDES.Create();
            tripleDES.Key = mD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripleDES.Mode = CipherMode.ECB;
            ICryptoTransform transform = tripleDES.CreateEncryptor();
            byte[] output = transform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(output);
        }

        public void fillCombo()
        {
            if (this.tabla_cmb == "") return;
            ComboBox cmb = null;
            foreach (Control c in this.parent.Controls)
            {
                if (c is ComboBox)
                {
                    cmb = (ComboBox)c;
                }
            }
            if (cmb == null) return;
            DataTable auxDt = this.utilConsultasI.getCMBData(tabla_cmb);

            string tagName = cmb.Tag.ToString();
            tagName = tagName.Replace("fk", "nbr");
            if (auxDt.Columns.Contains(tagName))
            {
                string[] valoresColumna = new string[auxDt.Rows.Count];

                for (int i = 0; i < auxDt.Rows.Count; i++)
                {
                    string valor = auxDt.Rows[i][tagName].ToString();
                    cmb.Items.Add(valor);
                }
            }
        }

        public void loadButtons()
        {
            int[] arr = this.ctrl_seguridad.getPermisosAplicaion(idApp);
            btn_agregar.Enabled = Convert.ToBoolean(arr[0]);
            btn_modificar.Enabled = Convert.ToBoolean(arr[1]);
            btn_eliminar.Enabled = Convert.ToBoolean(arr[2]);
            btn_consultar.Enabled = Convert.ToBoolean(arr[3]);
            btn_imprimir.Enabled = Convert.ToBoolean(arr[4]);
        }



        public void config(string tabla, Form parent, string _id = "1000")
        {
            this.idApp = _id;
            this.tabla = tabla;
            this.tabla_cmb = "";
            this.parent = parent;
            this.utilConsultasI.setTabla(this.tabla);
            DataGridView gd = GetDGV(this.parent);
            this.mydata = this.utilConsultasI.getArrData();
            //this.fillCombo();
            if (gd == null)
            {
                focusData(this.mydata);
                return;
            }
            _config_grid(gd);
            gridExiste = true;
            gd.CellClick += this.data_Click;
            this.utilConsultasI.refrescar(this.parent);
            this.cambiarEstado(false);
            gd.AllowUserToAddRows = false;
            this.loadButtons();
        }


        void _config_grid(DataGridView gd)
        {
            gd.ReadOnly = true;
            gd.RowHeadersVisible = false;
            gd.RowHeadersWidth = 51;
            gd.RowTemplate.Height = 24;
            gd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        }
        bool verificarDG()
        {
            return gridExiste;
        }
        private void data_Click(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dt = sender as DataGridView;
            if (dt.SelectedRows.Count > 0)
            {
                filaActual = dt.SelectedRows[0].Index;
                focusData((DataTable)dt.DataSource);
            }
        }
        public void identificarFormulario(Form child, string operacion)
        {
            DataGridView dgvname = GetDGV(child);

            if (operacion.Equals("g"))
            {
                this.ctrl_seguridad.setBtitacora(idApp, "Se ha guardado un registro");
                this.utilConsultasI.guardar(child);
            }
            if (operacion.Equals("m"))
            {
                this.ctrl_seguridad.setBtitacora(idApp, "Se ha modificado un registro");
                this.utilConsultasI.modificar(child);
            }
            if (operacion.Equals("r"))
            {
                this.utilConsultasI.refrescar(child);
            }
            if (operacion.Equals("e"))
            {
                this.utilConsultasI.eliminar(child, dgvname);
                this.ctrl_seguridad.setBtitacora(idApp, "Se ha eliminado un registro");
            }
        }
        public DataGridView GetDGV(Form child)
        {
            foreach (Control c in child.Controls)
            {
                if (c is DataGridView dgv)
                {
                    return dgv;
                }
            }
            return null;
            throw new Exception("No se encontró un DataGridView en el formulario.");
        }
        private void btn_guardar_Click(object sender, EventArgs e)
        {

            this.identificarFormulario(this.parent, this.operacion);
            this.cambiarEstado(false);
        }

        public void cambiarEstado(bool estado)
        {
            foreach (Control control in this.panel.Controls)
            {
                if (control is Button)
                {
                    Button btn = (Button)control;
                    if (btn.Name.Equals("btn_guardar") || btn.Name.Equals("btn_cancelar"))
                    {
                        btn.Enabled = estado;
                    }
                    else
                    {
                        btn.Enabled = !estado;
                    }
                }
            }

            foreach (Control c in this.parent.Controls)
            {
                if (c is TextBox)
                {
                    TextBox txt = (TextBox)c;
                    txt.ReadOnly = !estado;
                }
                else if (c is DateTimePicker)
                {
                    DateTimePicker dtp = (DateTimePicker)c;
                    dtp.Enabled = estado;
                }
            }

        }
        private void btn_agregar_Click(object sender, EventArgs e)
        {
            this.limpiarControles();
            this.cambiarEstado(true);
            this.operacion = "g";
        }
        public void limpiarControles()
        {
            foreach (Control control in this.parent.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
                else if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).Value = DateTime.Now;
                }
                else if (control is ComboBox)
                {
                    ComboBox cmb = ((ComboBox)control);
                    if (cmb.Items.Count > 0)
                    {
                        cmb.SelectedIndex = 0;
                    }
                }
            }
        }
        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.limpiarControles();
            this.cambiarEstado(false);
        }
        private void btn_ayuda_Click_1(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Ayudas/AyudaSO2.chm", "NavAyuda.html");
        }
        private void btn_refrescar_Click(object sender, EventArgs e)
        {
            this.identificarFormulario(this.parent, "r");
        }
        private void btn_modificar_Click(object sender, EventArgs e)
        {

            try
            {
                MessageBox.Show(" Modificar");
                this.utilConsultasI.cargarModificar(this.parent, GetDGV(this.parent));
                this.operacion = "m";
                this.cambiarEstado(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Error" + ex);
            }



        }
        public void focusData(DataTable dataTable)
        {
            Dictionary<string, string> rowData = new Dictionary<string, string>();

            if (dataTable.Rows.Count > 0)
            {
                DataRow selectedRow = dataTable.Rows[filaActual]; // En este ejemplo, seleccionamos la primera fila

                foreach (DataColumn column in dataTable.Columns)
                {
                    string columnName = column.ColumnName;
                    object cellValue = selectedRow[columnName];
                    rowData[columnName.ToString()] = cellValue.ToString();
                }

                foreach (Control c in this.parent.Controls)
                {
                    if (c is TextBox)
                    {
                        TextBox txt = (TextBox)c;
                        if (rowData.ContainsKey(txt.Tag.ToString()))
                        {
                            txt.Text = rowData[txt.Tag.ToString()];
                        }
                    }
                    else if (c is DateTimePicker)
                    {
                        DateTimePicker dt = (DateTimePicker)c;
                        if (rowData.ContainsKey(dt.Tag.ToString()))
                        {
                            DateTime date;
                            string _date_str = rowData[dt.Tag.ToString()];
                            if (DateTime.TryParse(_date_str, out date))
                            {
                                dt.Value = date;
                            }
                        }
                    }
                }
            }
        }
        private void btn_anterior_Click(object sender, EventArgs e)
        {
            bool call_data = !verificarDG();
            if (call_data)
            {
                if (filaActual > 0)
                {
                    filaActual--;
                    focusData(this.mydata);
                    return;
                }
                MessageBox.Show("No hay filas anteriores para seleccionar la anterior.");
                return;
            }
            DataGridView gd = GetDGV(this.parent);
            gd.ClearSelection();
            if (filaActual > 0)
            {
                filaActual--;
                gd.Rows[filaActual].Selected = true;
                focusData((DataTable)gd.DataSource);
            }
            else if (filaActual <= 0)
            {
                MessageBox.Show("No hay filas anteriores para seleccionar la anterior.");

            }

        }
        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            bool call_data = !verificarDG();
            if (call_data)
            {

                if (filaActual < mydata.Rows.Count - 1)
                {

                    filaActual++;
                    focusData(this.mydata);
                    return;
                }
                MessageBox.Show("No hay filas posteriores para seleccionar la siguiente.");
                return;
            }
            DataGridView gd = GetDGV(this.parent);
            gd.ClearSelection();
            if (filaActual < gd.Rows.Count - 1)
            {
                filaActual++;
                gd.Rows[filaActual].Selected = true;
                focusData((DataTable)gd.DataSource);
            }
            else
            {
                MessageBox.Show("No hay filas posteriores para seleccionar la siguiente.");
            }
        }
        private void btn_inicio_Click(object sender, EventArgs e)
        {
            bool call_data = !verificarDG();
            if (call_data)
            {
                filaActual = 0;
                focusData(this.mydata);
                return;
            }
            filaActual = 0;
            DataGridView gd = (verificarDG() ? GetDGV(this.parent) : this.data_invisible);
            gd.ClearSelection();
            gd.Rows[0].Selected = true;
            gd.FirstDisplayedScrollingRowIndex = 0;
            focusData((DataTable)gd.DataSource);
        }
        private void btn_fin_Click(object sender, EventArgs e)
        {
            bool call_data = !verificarDG();
            if (call_data)
            {
                filaActual = this.mydata.Rows.Count - 1;
                focusData(this.mydata);
                return;
            }
            DataGridView gd = (verificarDG() ? GetDGV(this.parent) : this.data_invisible);
            gd.ClearSelection();
            gd.Rows[gd.Rows.Count - 1].Selected = true;
            gd.FirstDisplayedScrollingRowIndex = gd.Rows.Count - 1;
            filaActual = gd.Rows.Count - 1;
            focusData((DataTable)gd.DataSource);
        }
        //Carol Chuy
        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            this.identificarFormulario(this.parent, "e");
        }
    }
}
