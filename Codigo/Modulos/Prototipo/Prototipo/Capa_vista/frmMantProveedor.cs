using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista_PrototipoMenu
{
    public partial class frmMantProveedor : Form
    {
        public frmMantProveedor()
        {
            InitializeComponent();
            this.navegador1.config("proveedores", this, "8000");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.txt_status.Text = this.checkBox1.Checked ? "1" : "0";
        }

        private void txt_status_TextChanged(object sender, EventArgs e)
        {
            this.txt_status.Text = this.checkBox1.Checked ? "1" : "0";
        }
    }
}
