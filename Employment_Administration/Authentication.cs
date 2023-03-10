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
    public partial class Authentication : Form
    {   


        private Login log ; 
        public Authentication(Login log)
        {
            this.log = log;
            InitializeComponent();

        }

        
       private  string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //These are used to set the connection to MySql database
        private const string ConnectionString = @"server=localhost;user id=root;database=project; password=ciku123";
        MySqlConnection con = new MySqlConnection(ConnectionString);



        private void Authentication_Load(object sender, EventArgs e)
        {
         textpass.Text = RandomString(8);
            try
            {
                con.Open();

                String query = "update administrator set Password='" +textpass.Text + "'" ;
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message);
                con.Close();
            }
          

    }

        private void copyButt_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textpass.Text);
            this.Hide();
            log.Enabled = true;
        }
    }
}
