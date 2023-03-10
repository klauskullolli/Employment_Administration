using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employment_Administration
{
    public partial class ChangePass : Form
    {

        String id; 

        public ChangePass(string id)
        {
            this.id = id;
            InitializeComponent();
        }



        //These are used to set the connection to MySql database
        private const string ConnectionString = @"server=localhost;user id=root;database=project; password=ciku123";
        MySqlConnection con = new MySqlConnection(ConnectionString);
       
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (textCon.Text == "" || textPass.Text == "") MessageBox.Show("Missing information.");

            else {

                if (textCon.Text == textPass.Text)
                {
                    try
                    {
                        con.Open();
                        String query = "update employee set Password='" + textPass.Text + "' where Id=" +id ;
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                        this.Hide();
                        EmployeeUpdate upd = new EmployeeUpdate(id);
                        upd.Show();


                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        con.Close();
                    }
                }

                else {

                    MessageBox.Show("Your password and confirmation password do not match. Write again correctly");
                }


            }
        }
    }
}
