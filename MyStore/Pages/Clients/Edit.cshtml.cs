using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
       public ClientInfo clientInfo = new ClientInfo();
       public string successMessage = "";
       public string errorMessage = "";
        public void OnGet()
        {
            string id = Request.Query["Id"];//request id from db

            try
            {
                string connectionString = "Data Source=DESKTOP-FTDN9V8\\XERXEZ;Initial Catalog=CSharpStore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients WHERE Id=@Id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@id", id); //adds requested id to id
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //client info mit db fuellen
                                clientInfo.Id = "" + reader.GetInt32(0); // string damit converted wird
                                clientInfo.Name = reader.GetString(1);
                                clientInfo.Email = reader.GetString(2);
                                clientInfo.Phone = reader.GetString(3);
                                clientInfo.Address = reader.GetString(4);

                            }
                        }

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
        
        public void OnPost()
        {
            clientInfo.Id = Request.Form["id"]; // string damit converted wird
            clientInfo.Name = Request.Form["name"];
            clientInfo.Email = Request.Form["email"];
            clientInfo.Phone = Request.Form["phone"];
            clientInfo.Address = Request.Form["address"];


            if (clientInfo.Id.Length == 0 || clientInfo.Name.Length == 0 || clientInfo.Email.Length == 0 ||
                clientInfo.Phone.Length == 0 || clientInfo.Address.Length == 0)
            {
                errorMessage = "All fields required!";
                return;
            }
            try
            {
                string connectionString = "Data Source=DESKTOP-FTDN9V8\\XERXEZ;Initial Catalog=CSharpStore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients" + " SET name=@name, email=@email, phone=@phone, address=@address"
                        + " WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", clientInfo.Name);
                        command.Parameters.AddWithValue("@Email", clientInfo.Email);
                        command.Parameters.AddWithValue("@Phone", clientInfo.Phone);
                        command.Parameters.AddWithValue("@Address", clientInfo.Address);
                        command.Parameters.AddWithValue("@Id", clientInfo.Id);

                        command.ExecuteNonQuery();

                    }
                }
                }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return;
            }
            Response.Redirect("/Clients/Index");

        }
    }
}
