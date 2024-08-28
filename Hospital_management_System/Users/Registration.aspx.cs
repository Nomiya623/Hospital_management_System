using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Hospital_management_System.Users
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        // Connection string is read from the Web.config file
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd;
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateNewUserId();
            }
        }

        private void GenerateNewUserId()
        {
            int id1 = 0;

            string str1 = "SELECT MAX(Id) AS Id FROM dbo.UserTab"; // Ensure the table is qualified with the schema
            da = new SqlDataAdapter(str1, con);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Id"] != DBNull.Value)
            {
                id1 = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                id1++;
            }
            else
            {
                id1 = 1;
            }

            lbl_rid.Text = id1.ToString(); // Assign the generated ID to the label
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Forgotpass.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx");
        }

        protected void btn_login_b_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                con.Open();
                try
                {
                    // Adjust the SQL to match your table columns
                    string str = "INSERT INTO dbo.UserTab (Id, Uname, Email, Mobile, Pass) VALUES (@Id, @Uname, @Email, @Mobile, @Pass)";
                    using (SqlCommand cmd = new SqlCommand(str, con))
                    {
                        // Add parameters to the command to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Id", lbl_rid.Text);
                        cmd.Parameters.AddWithValue("@Uname", txt_signup_username.Text);
                        cmd.Parameters.AddWithValue("@Email", txt_signup_email.Text);
                        cmd.Parameters.AddWithValue("@Mobile", txt_signup_mobile.Text);
                        cmd.Parameters.AddWithValue("@Pass", txt_signup_pass.Text);

                        cmd.ExecuteNonQuery();

                        // Show success message
                        Response.Write("<script>alert('Registration Successful');</script>");
                    }
                }
                catch (Exception exx)
                {
                    // Handle exceptions with a more user-friendly message
                    Response.Write("<script>alert('An error occurred during registration. Please try again.');</script>");
                    // Optionally log the exception message for further debugging
                    // System.Diagnostics.Debug.WriteLine(exx.ToString());
                }
            }
        }
    }
}
