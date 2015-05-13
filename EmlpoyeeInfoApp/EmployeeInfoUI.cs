using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmlpoyeeInfoApp
{
    public partial class EmployeeInfoUI : Form
    {
        public EmployeeInfoUI()
        {
            InitializeComponent();
        }



        private bool isUpdateMode = false;

        public int employeeId;

        Employee aEmployee = new Employee();
        private void saveButton_Click(object sender, EventArgs e)
        {

            aEmployee.name = nameTextBox.Text;
            aEmployee.address = addressTextBox.Text;
            aEmployee.email = emailTextBox.Text;
            aEmployee.salary = Convert.ToDouble(salaryTextBox.Text);



         


            if (IsEmailExist(aEmployee.email))
            {
                MessageBox.Show("Email already Exists !!");
            }

            else
            {
                string ConnectionString =
                    @"SERVER = .\SQLEXPRESS; Database = EmployeeInfoDB; Integrated Security = True ";
                SqlConnection Connection = new SqlConnection(ConnectionString);

                Connection.Open();
                string insertQuery = "INSERT INTO Employee_Table VALUES('" + aEmployee.name + "','" +
                                     aEmployee.address +
                                     "','" + aEmployee.email + "','" + aEmployee.salary + "')";
                SqlCommand command = new SqlCommand(insertQuery, Connection);

                int rowAffect = command.ExecuteNonQuery();
                Connection.Close();

                if (rowAffect > 0)
                {
                    MessageBox.Show("Inserted Successfully");
                    ShowAllInfo();
                }
                else
                {
                    MessageBox.Show("Insertion FAILED!!");
                }

                if (isUpdateMode)
                {


                    // SqlConnection Connection = new SqlConnection(ConnectionString);

                    Connection.Open();

                    string query = "UPDATE Employee_Table SET Employee_Name = '" + aEmployee.name + "', Address='" +
                                   aEmployee.address + "', Email='" + aEmployee.email + "', Salary='" + aEmployee.salary +
                                   "'";

                    SqlCommand commandL = new SqlCommand(query, Connection);


                    int rowAffected = commandL.ExecuteNonQuery();
                    Connection.Close();

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Updated Successfully!");
                        saveButton.Text = "SAVE";

                        employeeId = 0;

                        isUpdateMode = false;

                        ShowAllInfo();

                    }
                    else
                    {
                        MessageBox.Show("Update Failed!");
                    }



                }

            }




        }


        public bool IsEmailExist(string email)
        {
            // Employee aEmployee = new Employee();
            aEmployee.email = emailTextBox.Text;
            string ConnectionString = @"SERVER = .\SQLEXPRESS; Database = EmployeeInfoDB; Integrated Security = True ";
            SqlConnection Connection = new SqlConnection(ConnectionString);

            Connection.Open();
            string query = @"SELECT * FROM Employee_Table WHERE Email = '" + email + "'";
            SqlCommand cmd = new SqlCommand(query, Connection);

            SqlDataReader reader = cmd.ExecuteReader();
            bool isEmailExist = false;
            while (reader.Read())
            {
                isEmailExist = true;
                break;
            }
            reader.Close();
            Connection.Close();
            return isEmailExist;


        }

        public void ShowAllInfo()
        {
            string ConnectionString = @"SERVER = .\SQLEXPRESS; Database = EmployeeInfoDB; Integrated Security = True ";
            SqlConnection Connection = new SqlConnection(ConnectionString);
            Connection.Open();

            string query = "SELECT * FROM Employee_Table";
            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();

            List<Employee> employeeList = new List<Employee>();

            while (reader.Read())
            {
                Employee aEmployee = new Employee();
                aEmployee.ID = int.Parse(reader["ID"].ToString());
                aEmployee.name = reader["Employee_Name"].ToString();
                aEmployee.address = reader["Address"].ToString();
                aEmployee.email = reader["Email"].ToString();
                aEmployee.salary = float.Parse(reader["Salary"].ToString());

                employeeList.Add(aEmployee);

            }
            reader.Close();
            Connection.Close();

            LoadEmployeeListView(employeeList);


        }

        public void LoadEmployeeListView(List<Employee> employees)
        {
            employeeListView.Items.Clear();

            foreach (Employee aEmployee in employees)
            {
                ListViewItem item = new ListViewItem(aEmployee.ID.ToString());
                item.SubItems.Add(aEmployee.name);
                item.SubItems.Add(aEmployee.address);
                item.SubItems.Add(aEmployee.email);
                item.SubItems.Add(aEmployee.salary.ToString());

                employeeListView.Items.Add(item);
            }

        }

        private void EmployeeInfoUI_Load(object sender, EventArgs e)
        {
            ShowAllInfo();
        }

        private void employeeListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void employeeListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = employeeListView.SelectedItems[0];
            int id = int.Parse(item.Text.ToString());
            Employee aEmployee = GetEmployeeByID(id);

            if (aEmployee != null)
            {
                isUpdateMode = true;
                saveButton.Text = "Update";

                employeeId = aEmployee.ID;

                nameTextBox.Text = aEmployee.name;
                addressTextBox.Text = aEmployee.address;
                emailTextBox.Text = aEmployee.email;
                salaryTextBox.Text = aEmployee.salary.ToString();
            }

        }


        public Employee GetEmployeeByID(int id)
        {
            string ConnectionString = @"SERVER = .\SQLEXPRESS; Database = EmployeeInfoDB; Integrated Security = True ";
            SqlConnection Connection = new SqlConnection(ConnectionString);
            Connection.Open();

            string query = "SELECT * FROM Employee_Table WHERE ID = '" + id + "'";
            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();

            List<Employee> employeeList = new List<Employee>();

            while (reader.Read())
            {
                Employee aEmployee = new Employee();
                aEmployee.ID = int.Parse(reader["ID"].ToString());
                aEmployee.name = reader["Employee_Name"].ToString();
                aEmployee.address = reader["Address"].ToString();
                aEmployee.email = reader["Email"].ToString();
                aEmployee.salary = float.Parse(reader["Salary"].ToString());

                employeeList.Add(aEmployee);

            }
            reader.Close();
            Connection.Close();

            return employeeList.FirstOrDefault();

        }

    }
}
