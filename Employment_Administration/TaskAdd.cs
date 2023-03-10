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
    public partial class TaskAdd : Form
    {

        //this are needed to keep track of employee and his/her project 
        string emId, proId; 
        public TaskAdd(string emId, string proId  )
        {
            this.emId = emId;
            this.proId = proId;
            InitializeComponent();
        }


        //These are used to set the connection to MySql database
        private const string ConnectionString = @"server=localhost;user id=root;database=project; password=ciku123";
        MySqlConnection con = new MySqlConnection(ConnectionString);


        //action handled when add button is pressed 
        //assign new task to the employee and create new task related to project assigned to the employee
        private void button2_Click(object sender, EventArgs e)
        {
            if (textTask.Text == "") MessageBox.Show("Missing information.");


            else
            {
                try
                {
                    con.Open();
                    String query = "insert into task(Tname , ProId) values ('" + textTask.Text + "'," + proId + ")";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    con.Open();
                    query = "select * from task where Tname='" + textTask.Text + "' and ProId = " + proId;
                    cmd = new MySqlCommand(query, con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    string tid = reader.GetString("Id");
                    con.Close();

                    con.Open();
                    query = "insert employee_task(EmpId , TaId) values (" +emId  + "," + tid + ")";
                    cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    con.Close();

                    this.Hide();
                    EmployeeUpdate upd = new EmployeeUpdate(emId);
                    upd.Show();


                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    con.Close();
                    this.Hide();
                    EmployeeUpdate upd = new EmployeeUpdate(emId);
                    upd.Show();
                }
            }
        }
    }
}
