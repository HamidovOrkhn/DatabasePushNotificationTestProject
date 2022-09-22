using AspNetCoreWebSocketDatabasePush.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreWebSocketDatabasePush.Repository
{
    public class Notify : INotify
    {
        public readonly string connectionString;z
     


        public Notify(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("SqlConnect");
        }
        public List<Notification> GetAll()
        {
            var employees = new List<Notification>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDependency.Start(connectionString);

                string commandText = "select Id,Message from dbo.NOTIFICATION_TABLE";

                SqlCommand cmd = new SqlCommand(commandText, conn);

                SqlDependency dependency = new SqlDependency(cmd);

                dependency.OnChange += new OnChangeEventHandler(dbChangeNotification);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var employee = new Notification
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Message = reader["Message"].ToString(),

                    };

                    employees.Add(employee);
                }
            }

            return employees;
        }
        private async void dbChangeNotification(object sender, SqlNotificationEventArgs e)
        {
            Console.WriteLine("data");
        }
    }
}
