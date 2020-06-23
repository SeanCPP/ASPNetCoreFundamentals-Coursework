using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;

namespace Data.SqlServer
{
    public class SqlServerRestaurantData : IRestaurantData
    {
        private readonly DbSettings config;

        private const string TableName = "Restaurants";
        public SqlServerRestaurantData(IOptions<DbSettings> config)
        {
            this.config = config.Value;
        }
        public int Commit()
        {
            return 1;
        }

        public Restaurant Create(Restaurant newItem)
        {
            string sql = $"INSERT INTO {TableName}(Name, Location, Cuisine)";
            sql += " OUTPUT Inserted.Id";
            sql += " VALUES(@Name, @Location, @Cuisine);";
            SqlDo(sql, (cnn, cmd) => 
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", newItem.Name);
                cmd.Parameters.AddWithValue("@Location", newItem.Location);
                cmd.Parameters.AddWithValue("@Cuisine", (int)newItem.Cuisine);

                // get ID back from query
                int id = (int)cmd.ExecuteScalar();
                newItem.Id = id;
            });
            return newItem;
        }

        public Restaurant Delete(int id)
        {
            var restaurant = GetById(id);
            if(restaurant != null)
            {
                string sql = $"DELETE FROM {TableName} WHERE Id = @Id";
                SqlDo(sql, (cnn, cmd) =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                });
            }
            return restaurant;
        }

        public Restaurant GetById(int id)
        {
            // HACK
            var result = null as Restaurant;
            result = GetByName().Where(r => r.Id == id).FirstOrDefault();
            return result;
        }

        public IEnumerable<Restaurant> GetByName(string name = null)
        {
            var restaurants = new List<Restaurant>();
            DataTable dt = null;
            string sql = "";
            if (name is null)
            {
                sql = $"SELECT * FROM {TableName};";
            }
            else
            {
                // search partial match
                sql = $"SELECT * FROM {TableName} WHERE CHARINDEX(@Name, Name) > 0;";
            }
            SqlDo(sql, (cnn, cmd) =>
            {
                if (name != null)
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                }
                using var adapter = new SqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    restaurants = (from row in dt.AsEnumerable()
                                   select new Restaurant
                                   {
                                       Id = row.Field<int>("Id"),
                                       Name = row.Field<string>("Name"),
                                       Location = row.Field<string>("Location"),
                                       Cuisine = (CuisineType)row.Field<int>("Cuisine")
                                   }).ToList();
                }
            });
            return restaurants;
        }

        public Restaurant Update(Restaurant itemToUpdate)
        {
            string sql = $"UPDATE {TableName}";
            sql += $" SET Name = @Name,";
            sql += $" Location = @Location,";
            sql += $" Cuisine = @Cuisine";
            sql += $" WHERE Id = {itemToUpdate.Id}";
            SqlDo(sql, (cnn, cmd) =>
            {
                cmd.Parameters.AddWithValue("@Name", itemToUpdate.Name);
                cmd.Parameters.AddWithValue("@Location", itemToUpdate.Location);
                cmd.Parameters.AddWithValue("@Cuisine", itemToUpdate.Cuisine);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            });
            return GetById(itemToUpdate.Id);
        }

        private void SqlDo(string sql, Action<SqlConnection, SqlCommand> action)
        {
            using var cnn = new SqlConnection(config.ConnectionString);
            using var cmd = new SqlCommand(sql, cnn);
            cnn.Open();
            action(cnn, cmd);
        }
    }
}
