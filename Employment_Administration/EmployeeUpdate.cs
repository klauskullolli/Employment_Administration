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
    public partial class EmployeeUpdate : Form
    {

        private string id;

        public EmployeeUpdate(string id)
        {
            this.id = id;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        //These are used to set the connection to MySql database
        private const string ConnectionString = @"server=localhost;user id=root;database=project; password=ciku123";
        MySqlConnection con = new MySqlConnection(ConnectionString);



        //use to convert the path of a photo to double slashed path because is lost the slash 
        //where is single when is inserted into table
        private string double_slash(String str)
        {
            String[] arry = str.Split('\\');
            return string.Join("\\\\", arry);

        }


        //This function is used to write project name into tex project for this employee

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

        //Fill your task combo box according to data of database for a particular project
        private void combo_fill()
        {
            comboTasks.Items.Clear();
            comboTasks.Text = "";
            try
            {
                con.Open();
                String query = "select * from task where ProId= (select id from projects where Pname='" + textProject.Text + "')";
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

        // used to save path of uploaded photo
        private string path;

        //This the button that handle the acction of uploading photo
        private void UploadButt_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                path=double_slash(open.FileName);
                PhotoBox.Image = new Bitmap(open.FileName);
            }
        }



        //this is te function that fill the my tasks data grid view with data from task table for a specific employee
        private void display()
        {

            try
            {
                con.Open();
                String query = "select Id , Tname  from task where Id in (select TaId from employee_task where EmpId=  " + id + ")";
                MySqlDataAdapter mda = new MySqlDataAdapter(query, con);
                DataSet set = new DataSet();
                mda.Fill(set);
                myTasksDGV.DataSource = set.Tables[0];
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }




        //After the form is loaded 
        // These action are handle 
        //filling of attributes of employee with particular id 
        private void EmployeeUpdate_Load(object sender, EventArgs e)
        {

            try
            {
                con.Open();
                String query = "select * from employee where Id=" + id;
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                textName.Text = reader.GetString("Name");
                textSurname.Text = reader.GetString("Surname");
                textEmail.Text = reader.GetString("Email");

                path = reader.GetString("Photo");
                PhotoBox.Image = new Bitmap(path);
                
                con.Close();

                project_name();
                combo_fill();
                display();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }



        //Action handled when a row in task DGV is selected
        //this show if task is completed or no
        private void myTasksDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           String tId = myTasksDGV.SelectedRows[0].Cells[0].Value.ToString();
            delButt.Enabled = true;
            yesCheck.Checked = false;
            noCheck.Checked = false;
            try
            {
                con.Open();
                String query = String.Format("select * from employee_task where EmpId={0} and TaId={1} " , id , tId);
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.GetString("Completed") == "1") yesCheck.Checked = true;
                else noCheck.Checked = true;
               
                con.Close();


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }


        }

        //refresh the form to its initila state
        private void refreshButt_Click(object sender, EventArgs e)
        {
            this.Hide();
            EmployeeUpdate upd = new EmployeeUpdate(id);
            upd.Show();
        }


        //this button open a new form that handle changing password action
        private void changeButt_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChangePass pass = new ChangePass(id);
            pass.Show();
        }


       


        
        //this button is used to handle adding exisitng task of a specific project to the employee
        private void addButt_Click(object sender, EventArgs e)
        {
            if (comboTasks.SelectedIndex <= -1) MessageBox.Show("Select a task.");

            else
            {
                try
                {
                    con.Open();
                    String query = "select *  from task where ProId = (select ProId from employee where Id=  " + id + ") and Tname='" + comboTasks.SelectedItem.ToString() + "'";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    String tid = reader.GetString("Id");
                    con.Close();

                    con.Open();
                    query = query = "insert employee_task(EmpId , TaId) values (" + id + "," + tid + ")";
                    cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    comboTasks.SelectedItem = null;
                    comboTasks.Text = "";
                    display();



                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    con.Close();
                }
            }
        }

        //This button handle the creation of new task and assigning this task to the employee
        private void createButt_Click(object sender, EventArgs e)
        {

            try
            {
                con.Open();
                String query = "select * from projects where Id in (select ProId from employee where Id=" + id + ")";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                String proid = reader.GetString("Id");

                con.Close();

                this.Hide();
                TaskAdd tsk = new TaskAdd(id, proid);
                tsk.Show();



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
            
        }



        //This is the function that make a task completed if is not completed for particular employee
        private void compButt_Click(object sender, EventArgs e)
        {
            if (yesCheck.Checked == true)
            {
                MessageBox.Show("You have previously completed this task");
                yesCheck.Checked = false;
                noCheck.Checked = false;
                display();
            }

            else {

                try
                {
                    String tId = myTasksDGV.SelectedRows[0].Cells[0].Value.ToString();

                    con.Open();
                    String query = query = String.Format("update employee_task set Completed=true where EmpId={0} and  TaId={1}", id, tId);
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    yesCheck.Checked = false;
                    noCheck.Checked = false;
                    display();
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                    con.Close();
                }

            }
        }


        // this button is use to handle deleting a task from particular employee after task is selected form task DGV
        private void delButt_Click(object sender, EventArgs e)
        {
            try
            {
                String tId = myTasksDGV.SelectedRows[0].Cells[0].Value.ToString();

                con.Open();
                String query = query = String.Format("delete from employee_task where EmpId={0} and  TaId={1}", id, tId);
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                yesCheck.Checked = false;
                noCheck.Checked = false;
                display();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //this is a button that handle saving changes made in employeeUpdate form
        //specificaly email and photo changes and return employee form 
        private void savePutt_Click(object sender, EventArgs e)
        {
            if (textEmail.Text == "" || PhotoBox.Image == null)
            {
                MessageBox.Show("Missing information.");
            }

            else {

                try
                {
                    con.Open();
                    String query = String.Format("update employee  set  Email='{0}' ," +
                        "Photo='{1}' where Id={2} ", textEmail.Text , double_slash(path) , id);
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    this.Hide();
                    Employee emp = new Employee(id);
                    emp.Show();
                }

                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
