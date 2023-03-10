using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using MySql.Data.MySqlClient;

namespace Employment_Administration
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //These are used to set the connection to MySql database
        private const string ConnectionString = @"server=localhost;user id=root;database=project; password=ciku123";
        MySqlConnection con = new MySqlConnection(ConnectionString);


        //Log in button is used to handle all log in actions
        private void button1_Click(object sender, EventArgs e)
        {
            if (textEmail.Text == "" || textPass.Text == "" || comboRole.SelectedIndex <= -1) MessageBox.Show("Missing information.");

            else if (comboRole.SelectedItem.ToString() == "ADMINISTRATOR")
            {
                try
                {
                    con.Open();

                    String query = "select * from administrator";
                    MySqlDataAdapter mda = new MySqlDataAdapter(query, con);
                    DataTable set = new DataTable();
                    mda.Fill(set);

                    if (set.Rows[0][0].ToString() == textEmail.Text && set.Rows[0][1].ToString() == textPass.Text)
                    {

                        this.Hide();
                        Administrator ad = new Administrator();
                        ad.Show();
                    }

                    else {
                        MessageBox.Show("Your email or password is not correct. Check again.");
                    }

                    con.Close();
                }

                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                    con.Close();
                }

            }

            else {
                try
                {
                    con.Open();
                    String query = "select count(*) , Id from employee where Email='" + textEmail.Text+ "'and Password='" +textPass.Text+"'";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                    DataTable set = new DataTable();
                    da.Fill(set);
                    if (set.Rows[0][0].ToString() == "1")
                    {
                        this.Hide();
                        Employee emp = new Employee(set.Rows[0][1].ToString());
                        emp.Show();
                    }
                    else {
                        MessageBox.Show("Your email or password is not correct. Check again.");
                    }

                    con.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    con.Close();
                }
            }
        }

      

      

     


        //action performd if administrator option is selected in combo box
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          

            if (comboRole.SelectedItem.ToString()== "ADMINISTRATOR" ) {

                if (textEmail.Text == "") {
                    comboRole.Text = "SELECT ROLE";
                    MessageBox.Show("Email box is empty. Please enter email."); }
                else
                {
                    try
                    {
                        con.Open();
                        String query = "select count(*) from administrator where Email='" + textEmail.Text + "'";
                        MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                        DataTable set = new DataTable();
                        da.Fill(set);
                        if (set.Rows[0][0].ToString() == "1")
                        {
                            DialogResult dialogResult = MessageBox.Show("Do you want to recieve authentication password?", " Authentication", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                Authentication auth = new Authentication(this);
                                auth.Show();
                                this.Enabled = false;
                            }
                            textEmail.Text = "";
                            textPass.Text = "";

                        }
                        else
                        {
                            MessageBox.Show("Your email is not correct. Check again.");
                        }

                        con.Close();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        con.Close();
                    }

                }
            }
           

        }

        //Turn the login form to initial state
        private void label5_Click(object sender, EventArgs e)
        {
            textEmail.Text = "";
            textPass.Text = "";
            comboRole.SelectedItem=null;
            comboRole.Text = "SELECT ROLE";
        }

        private void showCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (showCheck.Checked)
            {
                textPass.UseSystemPasswordChar=false;

            }

            else {
                textPass.UseSystemPasswordChar = true;
            }
        }
    }
}
