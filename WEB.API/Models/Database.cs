using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBProducts.WEB.API.Models
{
    public class Database
    {
        // Connection string 
        static readonly string connstr = String.Format("Server={0};Port={1};" +
           "User Id={2};Password={3};Database={4};Ssl Mode={5};",
           "hbp1234.postgres.database.azure.com", "5432", "hbp123@hbp1234",
           "123456789@Hb", "HBP", "Require");


        private readonly NpgsqlConnection conn = new NpgsqlConnection(connstr);
        private NpgsqlCommand command;
        private NpgsqlDataReader dataReader;
        private readonly NpgsqlConnection conn2 = new NpgsqlConnection(connstr);
        private NpgsqlCommand command2;
        private NpgsqlDataReader dataReader2;

        // This method is used for getting
        // a list of all Product
        public ProductList GetAll()
        {
            var list = new ProductList();
            Product pTemp = null;
            conn.Open();
            try
            {
                command = new NpgsqlCommand("Select p.id, m.model, t.type " +
                                                " from product p " +
                                                " inner join model m " +
                                                " on m.id = p.model_id " +
                                                " inner join type t " +
                                                " on t.id = p.type_id ", conn);

                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    pTemp = new Product((String)dataReader[1], (String)dataReader[2], "", 
                                         GetProductDataForList((int)dataReader[0]), (int)dataReader[0]);
                    list.AddProduct(pTemp);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            conn.Close();
            return list;
        }

        // This method is used for getting
        // a specific Product given the 
        // Product ID
        public Product GetProduct(int id)
        {
            Product pTemp = null;
            conn.Open();
            try
            {
                command = new NpgsqlCommand("Select p.id, m.model, t.type, p.three_d_model " +
                                                " from product p " +
                                                " inner join model m " +
                                                " on m.id = p.model_id " +
                                                " inner join type t " +
                                                " on t.id = p.type_id " +
                                                " where p.id = :ID", conn);
                command.Parameters.Add(new NpgsqlParameter("ID", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Prepare();
                command.Parameters[0].Value = id;
                dataReader = command.ExecuteReader();
                dataReader.Read();
                pTemp = new Product((String)dataReader[1], (String)dataReader[2], (String)dataReader[3], 
                                    GetProductData((int)dataReader[0]), (int)dataReader[0]);
            }
            catch(Exception e)
            {
                Console.Write(e);
            }
            conn.Close();
            return pTemp;
        }

        // This method is used for getting
        // the product data for a specific 
        // product from the database.
        private List<ProductData> GetProductData(int id)
        {

            var pdList = new List<ProductData>();
            command2 = new NpgsqlCommand("Select  pi.product_id, dt.datatype, pi.datavalue, pi.isurl " +
                                            " from product_info pi " +
                                            " inner join datatype dt " +
                                            " on dt.id = pi.datatype_id " +
                                            " where pi.product_id = :Id", conn2);
            conn2.Open();
            try
            {
                command2.Parameters.Add(new NpgsqlParameter("Id", NpgsqlTypes.NpgsqlDbType.Integer));
                command2.Prepare();
                command2.Parameters[0].Value = id;
                dataReader2 = command2.ExecuteReader();
                ProductData prData;
                while (dataReader2.Read())
                {
                    prData = new ProductData((String)dataReader2[1], (String)dataReader2[2], (Boolean)dataReader2[3]);
                    pdList.Add(prData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            conn2.Close();
            return pdList;
        }

        // This method is used for getting
        // the Thimbnail information for a 
        // specific product from the database.
        private List<ProductData> GetProductDataForList(int id)
        {
            var pdList = new List<ProductData>();
            command2 = new NpgsqlCommand("Select  pi.product_id, dt.datatype, pi.datavalue " +
                                            " from product_info pi " +
                                            " inner join datatype dt " +
                                            " on dt.id = pi.datatype_id " +
                                            " where pi.product_id = :id and dt.datatype = 'Thumbnail'", conn2);
            conn2.Open();
            try
            {
                command2.Parameters.Add(new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer));
                command2.Prepare();
                command2    .Parameters[0].Value = id;
                dataReader2 = command2.ExecuteReader();
                ProductData prData;
                dataReader2.Read();
                prData = new ProductData((String)dataReader2[1], (String)dataReader2[2], true);
                pdList.Add(prData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            conn2.Close();
            return pdList;
        }
    }
}