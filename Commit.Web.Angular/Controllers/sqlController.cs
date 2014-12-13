using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace Commit.Web.Angular.Controllers
{
    public class sqlController : ApiController
    {
        public const string connectionString = "Server=tcp:uld67t7jyl.database.windows.net,1433;Database=singleton;User ID=robert@uld67t7jyl;Password=Commit!1;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public List<Dictionary<string, string>> Get()
        {
            List<Dictionary<string, string>> all_models = new List<Dictionary<string, string>>();

            List<string> fields = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {
                    return null;
                }

                SqlCommand command = new SqlCommand("SELECT * FROM dbo.singleton WHERE [CAMPUS] LIKE '''057%' Order By [CAMPNAME]", connection);

                var reader = command.ExecuteReader();

                for (var f = 0; f < reader.FieldCount; f++)
                {
                    fields.Add(reader.GetName(f));
                }

                if (reader.HasRows)
                    while (reader.Read())
                    {
                        try
                        {
                            Dictionary<string, string> models = new Dictionary<string, string>();

                            for (int f = 0; f < fields.Count; f++)
                            {
                                models.Add(fields[f], reader[fields[f]].ToString());
                            }

                            all_models.Add(models);

                        }
                        catch { }
                    }

                connection.Close();
            }
            return all_models;



        }


    }
    }

