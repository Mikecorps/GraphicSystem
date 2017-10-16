using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace GraficSystem.Controllers
{
    public class OracleHelper
    {
        public List<object> json = new List<object>();
        string ConnectionString;

        public OracleHelper()
        {
        }
        public OracleHelper(string host, string port, string ssid, string user, string pass)
        {
             ConnectionString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL =TCP)(HOST ="
                + host + ")(PORT = " + port + "))(CONNECT_DATA = (SERVER = DEDICATE)(SERVICE_NAME="
                + ssid + "))); Password =" + pass + "; User ID = " + user;
            fetch(ConnectionString);
        }
        public void fetch( string str)
        {
            OracleConnection conection = new OracleConnection(str);
            foreach (var table in getTables(conection))
                json.Add(new {key = table , values = getColumns(table,conection)});
        }
        public List<string> getTables(OracleConnection conection)
        {
            List<string> data = new List<string>();
            
            try
            {
                conection.Open();
                OracleCommand cmd = new OracleCommand("SELECT TABLE_NAME FROM TABS", conection);
                cmd.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    data.Add(reader.GetString(0));
                conection.Close();
                return data;
                
            }
            catch (Exception e) {
                data.Add(e.ToString());
                return data;
            }
        }

        public List<String> getColumns(string table, OracleConnection conection)
        {
            List<string> data = new List<string>();
            string query = "SELECT COLUMN_NAME FROM USER_TAB_COLUMNS WHERE TABLE_NAME ='" + table+"'";
            try
            {
                conection.Open();
                OracleCommand cmd = new OracleCommand(query, conection);
                cmd.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    data.Add(reader.GetString(0));
                conection.Close();
                return data;
            }
            catch (Exception e)
            {
                data.Add(e.ToString());
                return data;
            }
           
        }
        public List<object> getData(string table)
        {
            OracleConnection conection = new OracleConnection(ConnectionString);
            List<object> data = new List<object>();
            string query = "SELECT * FROM " + table;
            try
            {
                conection.Open();
                OracleCommand cmd = new OracleCommand(query, conection);
                cmd.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = cmd.ExecuteReader();
                int count = reader.FieldCount;
                while (reader.Read())
                {
                    List<object> temp = new List<object>();
                    for (int i = 0; i < count; i++)
                        temp.Add(reader[i]);
                    data.Add(new { value = temp });
                }
                conection.Close();
                return data;
            }
            catch (Exception e)
            {
                data.Add(new { error = e.ToString() });
                return data;
            }
        }
      
    }
}