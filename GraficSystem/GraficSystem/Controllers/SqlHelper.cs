using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace GraficSystem.Controllers
{
    
    public class SqlHelper
    {
        public Dictionary<string,List<object>> datos = new Dictionary<string,List< object>>();
        public Dictionary<string, Dictionary<string, string>> rows = new Dictionary<string, Dictionary<string, string>>();
        string strconection;
        public SqlHelper()
        {

        }
        public SqlHelper(string server, string user, string pass)
        {
            strconection = "data source=" + server + ";user id=" + user + ";password=" + pass + ";";
            fetch();
           
        }
        private void fetch()
        {
            foreach (var DB in getDataBases())
            {
                List<object> data = new List<object>();
                List<object> rows = new List<object>();
                foreach (var table in getTables(DB))
                {
                    data.Add(new { Key= table,Value=getTablekeys(table,DB)});
                }
                datos.Add(DB, data);
            }
        }


        private List<string> getDataBases()
        {
            List<string> data = new List<string>();
            string sql = "select name from sys.databases";
            using (SqlConnection cn = new SqlConnection(strconection))
            {
                cn.Open();
                SqlCommand comand = new SqlCommand(sql, cn);
                SqlDataReader reader = comand.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(reader[0].ToString());
                }
                cn.Close();
            }
            return data;
        }
        private List<string> getTables(string catalog)
        {
            List<string> data = new List<string>();
            string sql = "SELECT SCHEMA_NAME(schema_id)+'.'+name AS SchemaTable FROM sys.tables";
            using (SqlConnection cn = new SqlConnection(strconection+ "initial catalog="+catalog+";"))
            {
                cn.Open();
                SqlCommand comand = new SqlCommand(sql, cn);
                SqlDataReader reader = comand.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(reader[0].ToString());
                }
                cn.Close();
            }
            return data;
        }
        
        private List<string> getTablekeys(string tableName,string catalog)
        {
            List<string> data = new List<string>();
            string sql = "SELECT Name FROM sys.columns WHERE object_id = object_id('"+tableName+"')";
            using (SqlConnection cn = new SqlConnection(strconection+ "initial catalog=" + catalog + ";"))
            {
                cn.Open();
                SqlCommand comand = new SqlCommand(sql, cn);
                SqlDataReader reader = comand.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(reader[0].ToString());
                }
                cn.Close();
            }
            return data;
        }
        public List<object> getDataFromTables(string table,string catalog)
        {
           
            List<object> data = new List<object>();
            string sql = "SELECT TOP (100) * FROM " + table;
            using (SqlConnection cn = new SqlConnection(strconection+ "initial catalog=" + catalog + ";"))
            {
               
                cn.Open();
                SqlCommand comand = new SqlCommand(sql, cn);
                SqlDataReader reader = comand.ExecuteReader();
                int count = reader.FieldCount;
                while (reader.Read())
                {
                    List<object> temp = new List<object>();
                    for (int i = 0; i < count; i++)
                    {
                        if(!reader.IsDBNull(i))
                            temp.Add(reader[i]);
                        
                    }
                    data.Add(new { Value = temp});
                }
                cn.Close();
            }
            return data;
        }

       

    }
}

