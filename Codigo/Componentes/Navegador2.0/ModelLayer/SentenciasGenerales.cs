using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Windows.Forms;

namespace ModelLayer
{
    public class SentenciasGenerales
    {
        protected Conexion conn;
        private static string baseDatos = "";

        public SentenciasGenerales()
        {
            this.conn = new Conexion();
            baseDatos = this.conn.connection().Database;
        }

        public string getColumnsQuery(Dictionary<string, string> parameters, List<string> columns)
        {
            string sql = "(";
            foreach (string str in columns)
            {
                if (parameters.ContainsKey(str))
                {
                    if (!str.Equals(columns.Last()))
                    {
                        sql += str + ",";
                    }
                    else
                    {
                        sql += str;
                    }
                }
            }
            sql += ")";
            sql = sql.Replace(",)", ")");
            return sql;
        }

        public string getQuery(Dictionary<string, string> parameters, string tabla)
        {
            //MessageBox.Show("query: " + tabla);
            List<string> columns = this.getColumns(tabla);
            string _columns = this.getColumnsQuery(parameters, columns);
            //Se deberia cambiar la tabla a usuarios para el ingreso de datos y la creacion de roles
            string sql = "INSERT INTO " + tabla + " " + _columns + " VALUES (";
            foreach (string col in columns)
            {
                if (parameters.Keys.Contains(col))
                {
                    string str = parameters[col];
                    sql += "'" + str + "'" + ",";
                }
            }
            sql += ");";
            sql = sql.Replace(",)", ")");
            Console.WriteLine(sql);
            return sql;
        }


        public List<string> getColumns(string tableName)
        {
            //MessageBox.Show("tabla: " + tableName);
            List<string> columns = new List<string>();
            try
            {
                string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + baseDatos + "' AND TABLE_NAME='" + tableName + "';";
                OdbcCommand cmd = new OdbcCommand(query, this.conn.connection());
                OdbcDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string column = reader.GetString(0);
                    Console.WriteLine("c" + column);
                    columns.Add(column);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return columns;
        }

        public void insertarSQL(string query)
        {
            try
            {
                OdbcCommand cmd = new OdbcCommand(query, this.conn.connection());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error al guardar, verifique que todos sus campos esten llenos correctamente");
                Console.WriteLine(e.Message);
            }
        }

        public OdbcDataAdapter refrescarSQL(string tabla)
        {
            try
            {
                string query = "SELECT * FROM " + tabla;
                OdbcDataAdapter dt = new OdbcDataAdapter(query, this.conn.connection());
                return dt;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("EN REFRESCARSQL");
            }
            return null;
        }


        public string getModificarQuery(Dictionary<string, string> parameters, string tabla, string pk, int id)
        {
            List<string> columns = this.getColumns(tabla);
            string sql = "UPDATE " + tabla + " SET ";

            foreach (string col in columns)
            {
                if (parameters.Keys.Contains(col))
                {
                    string str = parameters[col];
                    sql += col + " = '" + str + "', ";
                }
            }

            sql = sql.TrimEnd(',', ' ');
            sql += " WHERE " + pk + " = '" + id + "';";

            Console.WriteLine(sql);
            return sql;
        }

        //Carol Chuy

        public string eliminarQuery(string tabla, string campo, int llave)
        {
            string sql = "DELETE FROM " + tabla + " WHERE " + campo + " = '" + llave + "';";
            Console.WriteLine(sql);
            return sql;
        }
    }
}
