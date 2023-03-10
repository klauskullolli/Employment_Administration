using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Employment_Administration
{
    public partial class Administrator : Form
    {
        public Administrator()
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


        // After clicking upload button the slected photo is uploaded
        private void UploadButt_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                textPhoto.Text = double_slash(open.FileName);

                PhotoBox.Image = new Bitmap(open.FileName);
            }
        }

      


        //Fill your task combo box according to data of database for a particular employee
        private void yTask_combo_fill()
        {
            comboTaskYour.Items.Clear();
            comboTaskYour.Text = "TASKS";
            try
            {
                con.Open();
                String query = "select * from task where Id in ( select TaId from employee_task where EmpId =" + textId.Text+")";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String Pname = reader.GetString("Tname");
                   comboTaskYour.Items.Add(Pname);

                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Action performed when a specific row in DGV is selected
        //All values are shown respectively to specific text boxes and picture box
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textId.Text=employeeDGV.SelectedRows[0].Cells[0].Value.ToString();
            textName.Text = employeeDGV.SelectedRows[0].Cells[1].Value.ToString();
            textSurname.Text= employeeDGV.SelectedRows[0].Cells[2].Value.ToString();
            textEmail.Text = employeeDGV.SelectedRows[0].Cells[3].Value.ToString();
            textPass.Text = employeeDGV.SelectedRows[0].Cells[4].Value.ToString();
            textPhoto.Text= double_slash(employeeDGV.SelectedRows[0].Cells[5].Value.ToString());
            PhotoBox.Image = new Bitmap(textPhoto.Text);
            
            try
            {
                String Pname;
                con.Open();
                String query = "select * from projects where Id=" + employeeDGV.SelectedRows[0].Cells[6].Value.ToString();
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                Pname = reader.GetString("Pname");

                con.Close();

                comboProject.Text = Pname;

                yTask_combo_fill();

            }

            catch (Exception ex) {

                MessageBox.Show(ex.Message);
                con.Close();
            }
           



        }


        //this privat variable is needed to save project id if is given the project name
        private String proId;


        //action performed after selecting particular project combo box value  
        private void comboProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            textTaskId.Text = "";
            task_combo_fill();
          
        }


        //this is a function that give value proId according to project id seleced in peoject combo box 
       private void project_Id() {

            try
            {
                con.Open();
                String query = "select * from projects where Pname='" + comboProject.Text + "';";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                reader.Read();
                proId = reader.GetString("Id");

                con.Close();




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }


        }

        //Fill project combo box according to data of database
        private void project_combo_fill() {

            try
            {
                con.Open();
                String query = "select * from projects";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    String Pname = reader.GetString("Pname");
                    comboProject.Items.Add(Pname);

                }
                con.Close();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        } 
    


        //This a function that fill tasks combobox according to selected project combo box value 
        private void task_combo_fill()
        {
            comboTasks.Items.Clear();
            comboTasks.Text = "SELECT TASK";
            try
            {
                con.Open();
                String query = "select * from task where ProId in (select  Id from projects where Pname ='" +comboProject.Text + "')" ;
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
                con.Close();
            }
        }


        //Action  performend when this window is loaded
        private void Administrator_Load(object sender, EventArgs e)
        {   
            //fill project combo box with data
            project_combo_fill();
            //fill project DGV with data
            display();
        }


  


        //Action performed after selectin tasks combo box
        private void comboTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                String query = "select * from task where Tname ='" + comboTasks.Text + "'";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
               
                //fill taks id text box with task id 
                textTaskId.Text = reader.GetString("Id");
                   

                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }


        //use to convert the path of a photo to double slashed path because is lost the slash 
        //where is single when is inserted into table
        
        private string double_slash(String str) {
            String[] arry = str.Split('\\') ; 
            return string.Join("\\\\" , arry);
        
        }



        //this function handle the addition event of datas in employee table
        private void AddButt_Click(object sender, EventArgs e)
        {
            try
            {
                if (textName.Text == "" || textSurname.Text == "" || textEmail.Text == "" || textPass.Text == "" || textPhoto.Text == "")
                    MessageBox.Show("Missing information.");
                else
                {
                    project_Id();
                    con.Open();
                    String query = String.Format("insert into employee(Name , Surname , Email , Password , Photo , ProId) values ('{0}', '{1}', '{2}','{3}','{4}',{5})", textName.Text, textSurname.Text, textEmail.Text, textPass.Text, textPhoto.Text, proId);
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    display();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }


        //this is a function that display data from employee table according to database 
        private void display()
        {

            try
            {
                con.Open();
                String query = "select * from employee";
                MySqlDataAdapter mda = new MySqlDataAdapter(query, con);
                DataSet set = new DataSet();
                mda.Fill(set);
                employeeDGV.DataSource = set.Tables[0];
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }


        //this function is used to empty all text boxes and turn into initial state
        private void clear_employee_info() {
            textId.Text = "";
            textName.Text = "";
            textSurname.Text = "";
            textEmail.Text = "";
            textPass.Text = "";
            textPhoto.Text = "";
            PhotoBox.Image = null;
            comboProject.Text = "SELECT PROJECT";
            comboTasks.Items.Clear();
            comboTasks.Text = "SELECT TASK";
        }



        //This function handle deleting action in database of a specific employee
        private void DelButt_Click(object sender, EventArgs e)
        {

            try
            {
                if (textName.Text == "" || textId.Text == "" || textSurname.Text=="" || textEmail.Text == ""|| textPass.Text == "") 
                    MessageBox.Show("Missing information.");

                else
                {
                    con.Open();

                    String query = "delete from employee where Id=" + textId.Text;
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    display();
                    clear_employee_info();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //This function handle updating action in database of a specific employee
        private void EditButt_Click(object sender, EventArgs e)
        {
            try
            {
                if (textName.Text == "" || textId.Text == "" || textSurname.Text == "" || textEmail.Text == "" || textPass.Text == ""|| textPhoto.Text == "") 
                    MessageBox.Show("Missing information.");


                else
                {
                    project_Id();
                    con.Open();
                    String query = String.Format("update employee  set Name='{0}' , Surname='{1}', Email='{2}' , Password='{3}'," +
                        "Photo='{4}', ProId={5} where Id={6} " , textName.Text , textSurname.Text, textEmail.Text, textPass.Text , textPhoto.Text , proId ,textId.Text);
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    display();
                    clear_employee_info();
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //This button that perform database addition function of tasks for a specific employee
        private void taskAddButt_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                String query = string.Format("insert into employee_task(EmpId ,TaId) values({0},{1}) " , textId.Text,  textTaskId.Text) ;
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();

                yTask_combo_fill();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                con.Close();

            }
        }


        //Action perormed when a particular value is selected on your task combo box 
        private void comboTaskYour_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                con.Open();
                String query = "select * from task where Tname ='" + comboTaskYour.Text + "'";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                //fill taks id text box with task id 
                textTaskId.Text = reader.GetString("Id");


                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }


        //This is the refresh button. Load again the administrator form 
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Administrator ad = new Administrator();
            ad.Show();
        }

        //This the button that remove tasks from the emlpoyee
        private void taskReBott_Click(object sender, EventArgs e)
        {
            try
            {
                if (textTaskId.Text=="" || textId.Text=="")
                    MessageBox.Show("Missing information.");
                else {
                    con.Open();
                    String query = String.Format("delete from employee_task where EmpId={0} and TaId={1}", textId.Text, textTaskId.Text);
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    yTask_combo_fill();
                    textTaskId.Text = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }


        //Button that handle action of sending to project form to create update delete projects and tasks
        private void proEditButt_Click(object sender, EventArgs e)
        {
            this.Hide();
            Project pro = new Project();
            pro.Show();
        }

        private void textId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
