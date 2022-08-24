using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            //request data from form
            clientInfo.Name = Request.Form["Name"];
            clientInfo.Email = Request.Form["Email"];
            clientInfo.Phone = Request.Form["Phone"];
            clientInfo.Address = Request.Form["Address"];


            if (clientInfo.Name.Length == 0 || clientInfo.Email.Length == 0 ||
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
                   
                    string sql = "INSERT INTO clients" + "(Name, Email, Phone, Address) VALUES " +
                        "(@Name, @Email, @Phone, @Address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        //add values to clientinfo model
                        command.Parameters.AddWithValue("@Name", clientInfo.Name);
                        command.Parameters.AddWithValue("@Email", clientInfo.Email);
                        command.Parameters.AddWithValue("@Phone", clientInfo.Phone);
                        command.Parameters.AddWithValue("@Address", clientInfo.Address);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage=ex.Message;
                return;
            }
            clientInfo.Name = ""; clientInfo.Email = ""; clientInfo.Phone = ""; clientInfo.Address = ""; //deletes input text from user
            successMessage = "New client added correctly";

            Response.Redirect("/Clients/Index"); //redirect user to client

        }
    }
}
