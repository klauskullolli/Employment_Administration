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
    public partial class Employee : Form
    {
        //used to keep user information after loging in 
        private string id;

        public Employee(string id)
        {
            this.id = id;
            InitializeComponent();

        }

        //These are used to set the connection to MySql database
        private const string ConnectionString = @"server=localhost;user id=root;database=project; password=ciku123";
        MySqlConnection con = new MySqlConnection(ConnectionString);
        

        //this is X label that terminate the program

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //Fill your task combo box according to data of database for a particular employee
        private void combo_fill()
        {
            comboTasks.Items.Clear();
            comboTasks.Text = "";
            try
            {
                con.Open();
                String query = "select * from task where Id in ( select TaId from employee_task where EmpId =" + id + ")";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String Pname = reader.GetString("Tname");
                    comboTasks.Items.Add(Pname);

                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //funciton that fill the project text box with project name 
        private void project_name()
        {
            try
            {
                con.Open();
                String query = "select Pname from projects where Id in (select ProId from employee where Id=" + id + ")";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                textProject.Text = reader.GetString("Pname");

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }


        //Action performed when window is loaded
        private void Employee_Load(object sender, EventArgs e)
        {


             try
            {
                con.Open();
                String query = "select * from employee where Id=" + id;
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                textName.Text = reader.GetString("Name");
                textSurname.Text= reader.GetString("Surname");
                textEmail.Text = reader.GetString("Email");
                PhotoBox.Image = new Bitmap(reader.GetString("Photo"));
                con.Close();

                project_name();
                combo_fill();


            }

            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }


        // Action handled when a value is selected in task combo box 
        private void comboTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                String query = "select * from employee_task where EmpId=" + id + " and TaId=(select Id from task where Tname='" +comboTasks.SelectedItem.ToString()+"')";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                if (reader.GetString("Completed") == "0")
                    textComp.Text = "False";
                if (reader.GetString("Completed") == "1")
                    textComp.Text = "True";
                con.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }


        //this is button that send employee update form where employee data can be moified 
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            EmployeeUpdate upd = new EmployeeUpdate(id);
            upd.Show();
        }
    }
}
