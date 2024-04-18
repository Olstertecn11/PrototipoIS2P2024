using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace ModelLayer
{
    public class Sentencias : SentenciasGenerales
    {
        private string tabla = "tbl_inventario";

        public Sentencias()
        {

        }
        public Sentencias(string tabla)
        {
            this.tabla = tabla;
        }
        public void guardar(Dictionary<string, string> parameters)
        {
            string query = this.getQuery(parameters, tabla);
            this.insertarSQL(query);
        }

        public OdbcDataAdapter refrescar()
        {
            return this.refrescarSQL(tabla);
        }
        public void modificar(Dictionary<string, string> parameters, string pk, int id)
        {
            string query = this.getModificarQuery(parameters, tabla, pk, id);
            this.insertarSQL(query);
        }

        //Carol Chuy
        public void eliminar(string campo, int llave)
        {
            string query = this.eliminarQuery(tabla, campo, llave);
            this.insertarSQL(query);
        }
    }
}
