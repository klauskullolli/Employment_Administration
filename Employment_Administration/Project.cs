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
    public partial class Project : Form
    {
        public Project()
        {   
            InitializeComponent();
     
        }

        //These are used to set the connection to MySql database
        private const string ConnectionString = @"server=localhost;user id=root;database=project; password=ciku123";
        MySqlConnection con = new MySqlConnection(ConnectionString);
       
        
        
        //This is event handling for add button related to the project 
        private void AddButt_Click(object sender, EventArgs e)
        {
            try
            {
                if (textName.Text == "") MessageBox.Show("Missing information.");

                else
                {
                    con.Open();

                    String query = "insert into projects(Pname) values('" + textName.Text + "')";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    display();
                    clear_project_text();
                } 
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                con.Close();
            }

          
        }


        //Used to fill the data grid view of the project with table from projects table of database
        private void display() {

            try
            {
                con.Open();
                String query = "select * from projects";
                MySqlDataAdapter mda = new MySqlDataAdapter(query, con);
                DataSet set = new DataSet();
                mda.Fill(set);
                projectDGV.DataSource = set.Tables[0];
                con.Close();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }

        // This is cross lable that terminate the program
        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //Filling event of project texboxes after clicking a particular row in data gride view
        private void projectDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textId.Text = projectDGV.SelectedRows[0].Cells[0].Value.ToString()  ;
            textName.Text = projectDGV.SelectedRows[0].Cells[1].Value.ToString();
            display2();
            clear_task_text();
        }


        //While the form is loaded diplay() function is called
        private void Project_Load(object sender, EventArgs e)
        {
            display();
            
        }

        //used to clear project textboxes 
        private void clear_project_text() {
            textId.Text = "";
            textName.Text = "";

        }

        //used to clear task textboxes 
        private void clear_task_text()
        {
            textId1.Text = "";
            textName1.Text = "";

        }

        //this the delete button function for the project part 
        private void DelButt_Click(object sender, EventArgs e)
        {
            try
            {
                if (textId.Text == "" || textName.Text == "") MessageBox.Show("Missing information.");

                else
                {
                    con.Open();

                    String query = "delete from projects where Id=" + textId.Text;
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    display();
                    display2();
                    clear_project_text();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }



        //this is te function that fill the tasks data grid view with data from task table for a specific project
        private void display2()
        {

            try
            {
                con.Open();
                String query = "select Id , Tname from task where ProId=" + textId.Text;
                MySqlDataAdapter mda = new MySqlDataAdapter(query, con);
                DataSet set = new DataSet();
                mda.Fill(set);
                taskDGV.DataSource = set.Tables[0];
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }


        // This function is used to handle event of tast addition button 
        private void addButt2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textName1.Text == "") MessageBox.Show("Missing information.");

                else
                {
                    con.Open();

                    String query = "insert into task(Tname , ProId) values ('" + textName1.Text + "','" + textId.Text + "')";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    display2();
                    clear_task_text();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }


        // This function is used to handle event of tast deleting  button 
        private void delButt2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textName1.Text == "" || textId1.Text == "") MessageBox.Show("Missing information.");

                else
                {
                    con.Open();

                    String query = "delete from task where Id=" + textId1.Text;
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    display2();
                    clear_task_text();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //Acction preformed after clicking the task grid view 
        private void taskDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textId1.Text = taskDGV.SelectedRows[0].Cells[0].Value.ToString();
            textName1.Text = taskDGV.SelectedRows[0].Cells[1].Value.ToString();
        }


        //this is a update action for edit button that preform update in database
        private void editButt_Click(object sender, EventArgs e)
        {
            try
            {
                if (textName1.Text == "" || textId1.Text == "") MessageBox.Show("Missing information.");


                else
                {
                    con.Open();

                    String query = "update task set Tname='" + textName1.Text + "'where Id=" + textId1.Text;
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    display2();
                    clear_task_text();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        //Refreshing window form 
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Project pro = new Project();
            pro.Show();


        }
        //this is the finish button that sends to administrator form again
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Administrator ad = new Administrator();
            ad.Show();
        }
    }
}
