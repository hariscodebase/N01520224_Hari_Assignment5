using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01520224_Hari_Assignment5.Models
{
    public class SchoolDbContext
    {
        //declare readonly properties essential for db connectivity
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        //form connection string
        protected static string ConnectionString
        {
            get { return $"server = {Server}; user = {User}; database = {Database}; port = {Port}; password = {Password}; convert zero datetime = True"; }
        }

        //method to connect to db using connection-string
        public MySqlConnection AccessDatabase()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}