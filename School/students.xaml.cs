using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace School
{
    public partial class students : Window
    {
        private ObservableCollection<Student> Students = new ObservableCollection<Student>();

        public students()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolAppConnectionString"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Name, Age, Grade FROM Students";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Students.Add(new Student
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Age = reader.GetInt32(2),
                            Grade = reader.GetDecimal(3)
                        });
                    }
                    dataGrid.ItemsSource = Students;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolAppConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (Student student in Students)
                    {
                        string query = "UPDATE Students SET Name = @Name, Age = @Age, Grade = @Grade WHERE Id = @Id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Name", student.Name);
                        command.Parameters.AddWithValue("@Age", student.Age);
                        command.Parameters.AddWithValue("@Grade", student.Grade);
                        command.Parameters.AddWithValue("@Id", student.Id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is Student selectedStudent)
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["SchoolAppConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Students WHERE Id = @Id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Id", selectedStudent.Id);
                        command.ExecuteNonQuery();
                    }
                    Students.Remove(selectedStudent);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error deleting student: " + ex.Message);
                }
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Grade { get; set; }
    }
}