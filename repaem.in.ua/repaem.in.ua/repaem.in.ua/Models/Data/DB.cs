using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using aspdev.repaem.Models.Data.RepaemDataSetTableAdapters;
using System.Data.SqlClient;

namespace aspdev.repaem.Models.Data
{
    /// <summary>
    /// клас який відповідає за роботу з БД.
    /// </summary>
    public class DB
    {
        static string GetConnectionString()
        {
            return @"Data source=localhost;Initial Catalog=TestDB;user=ben;password=password;";
        }

        /// <summary>
        /// додаємо в базу дані про нову РепБазу
        /// </summary>
        /// <param name="name">ім"я бази</param>
        /// <param name="cityId">Id міста зі списку</param>
        /// <param name="address">адреса бази</param>
        /// <param name="coordinates">координати місцезнаходження бази</param>
        /// <param name="rating">рейтинг бази</param>
        /// <param name="comments">коментарі до бази(поки не реалізовано в базі)</param>
        /// <param name="additionalInformation">додаткова інформація про базу</param>
        public static void AddRepBase(string name, int cityId, string address, string coordinates,
                                      int rating, string comments, string additionalInformation)
        {
            try
            {
                string connectionString = GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (
                        SqlCommand cmd =
                            new SqlCommand("INSERT INTO RepBase(Name, City, Address, Coordinates, Rating, " +
                                           "Comments, AdditionalInformation) VALUES (@Name, @City, @Address, " +
                                           "@Coordinates, @Rating, @Comments, @AdditionalInformation)", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Name", name));
                        cmd.Parameters.Add(new SqlParameter("@City", cityId));
                        cmd.Parameters.Add(new SqlParameter("@Address", address));
                        cmd.Parameters.Add(new SqlParameter("@Coordinates", coordinates));
                        cmd.Parameters.Add(new SqlParameter("@Rating", rating));
                        cmd.Parameters.Add(new SqlParameter("@Comments", comments));
                        cmd.Parameters.Add(new SqlParameter("@AdditionalInformation", additionalInformation));

                        //передаємо в змінну rowsAffected кількість зроблених записів.
                        // типа для тесту, що дані були збережені.
                        // можна так і не робити.
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {

            }
        }
    }
}