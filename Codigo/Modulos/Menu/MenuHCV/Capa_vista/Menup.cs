using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Seguridad_Controlador;
using Vista_Seguridad;
using Vista_PrototipoMenu;

//Comentado por George Mayén 25/02/2023
namespace MenuHCV
{
    public partial class Menup : Form
    {
        Controlador cn = new Controlador();

        //Método que guarda en un arreglo de tipo botón los botones que se tienen en el formulario. Se les da permiso a los diferentes botones de acuerdo a la función que realice este
        public Menup()
        {
            InitializeComponent();

            Button[] apps = { btnseguridad, btnPrototipo };
            cn.deshabilitarApps(apps);

            cn.getAccesoApp(1000, apps[0]);  //Modulo Seguridad
            cn.getAccesoApp(8000, apps[1]);  //Modulo Compras y Cxp
        }
        //Método que ingresa un valor a la bitacora de acuerdo el módulo
        public void btnlogout_Click(object sender, EventArgs e)
        {
            MenuSeguridad b = new MenuSeguridad();
            b.Show();
            cn.setBtitacora("1000", "Ingreso Seguridad");
            //this.Close();
        }

        public void btnVentas_Click(object sender, EventArgs e)
        {
            //Vista_PrototipoMenu.MenuVentasCxc form = new Vista_PrototipoMenu.MenuVentasCxc();
            cn.setBtitacora("2000", "Ingreso Ventas");
        }
        //Método que ingresa un valor a la bitacora de acuerdo el módulo
        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Login b = new Login();
            cn.setBtitacora("999", "Cerro Sesion");
            b.Show();
            this.Close();
        }
        //Método que ingresa un valor a la bitacora de acuerdo el módulo
        private void btncompras_Click(object sender, EventArgs e)
        {
            cn.setBtitacora("3000", "Ingreso Compras");
        }
        //Método que ingresa un valor a la bitacora de acuerdo el módulo

        private void btnInventario_Click(object sender, EventArgs e)
        {
            cn.setBtitacora("4000", "Ingreso Inventarios");
            /*CapaVistaInventario.menu_inventario b = new CapaVistaInventario.menu_inventario();
            b.Show();
            cn.setBtitacora("4000", "Ingreso Inventario");*/
        }

        private void btnBancos_Click(object sender, EventArgs e)
        {
            cn.setBtitacora("5000", "Ingreso Ventas");
        }

        private void btnNominas_Click_1(object sender, EventArgs e)
        {
            cn.setBtitacora("6000", "Ingreso Ventas");
        }

        private void btnHoteleria_Click(object sender, EventArgs e)
        {
        }

        private void btnCxC_Click(object sender, EventArgs e)
        {

        }

        private void btnCxP_Click(object sender, EventArgs e)
        {

        }

        private void btnPrototipo_Click(object sender, EventArgs e)
        {
            cn.setBtitacora("8000", "Ingreso Prototipo");
            Vista_PrototipoMenu.MenuPrototipo form = new Vista_PrototipoMenu.MenuPrototipo();
            form.Show();
        }

    }
}
