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
    public partial class teachers : Window
    {
        private ObservableCollection<Teacher> Teachers = new ObservableCollection<Teacher>();

        public teachers()
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
                    string query = "SELECT Id, Name, Subject FROM Teachers";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Teachers.Add(new Teacher
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Subject = reader.GetString(2)
                        });
                    }
                    dataGrid.ItemsSource = Teachers;
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
                    foreach (Teacher teacher in Teachers)
                    {
                        string query = "UPDATE Teachers SET Name = @Name, Subject = @Subject WHERE Id = @Id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Name", teacher.Name);
                        command.Parameters.AddWithValue("@Subject", teacher.Subject);
                        command.Parameters.AddWithValue("@Id", teacher.Id);
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
            if (dataGrid.SelectedItem is Teacher selectedTeacher)
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["SchoolAppConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Teachers WHERE Id = @Id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Id", selectedTeacher.Id);
                        command.ExecuteNonQuery();
                    }
                    Teachers.Remove(selectedTeacher);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error deleting teacher: " + ex.Message);
                }
            }
        }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
