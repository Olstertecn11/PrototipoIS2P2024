using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer;

namespace ControllerLayer
{
    public class Controlador
    {

        public Sentencias sn;
        public string tabla = "tbl_inventario";


        public Controlador()
        {
            this.sn = new Sentencias();
        }
        public void setTabla(string tabla)
        {
            this.tabla = tabla;
            this.sn = new Sentencias(tabla);
        }
        public void guardar(Dictionary<string, string> parameters)
        {
            this.sn.guardar(parameters);
        }


        public DataTable refrescar()
        {
            OdbcDataAdapter dt = sn.refrescar();
            DataTable table = new DataTable();
            dt.Fill(table);
            return table;
        }

        public void modificar(Dictionary<string, string> parameters, string pk, int id)
        {
            this.sn.modificar(parameters, pk, id);
        }
        public List<string> getColumns(string tabla)
        {
            return this.sn.getColumns(tabla);
        }
        //Carol Chuy
        public void eliminar(string campo, int llave)
        {
            this.sn.eliminar(campo, llave);
        }
    }
}
