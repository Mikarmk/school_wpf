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
    public partial class classes : Window
    {
        private ObservableCollection<Class> Classes = new ObservableCollection<Class>();

        public classes()
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
                    string query = "SELECT Id, Name FROM Classes";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    List<Class> classesList = new List<Class>();
                    while (reader.Read())
                    {
                        classesList.Add(new Class
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                    Classes = new ObservableCollection<Class>(classesList);
                    dataGrid.ItemsSource = Classes;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
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
                    foreach (Class @class in Classes)
                    {
                        string query = "UPDATE Classes SET Name = @Name WHERE Id = @Id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Name", @class.Name);
                        command.Parameters.AddWithValue("@Id", @class.Id);
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
            if (dataGrid.SelectedItem is Class selectedClass)
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["SchoolAppConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Classes WHERE Id = @Id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Id", selectedClass.Id);
                        command.ExecuteNonQuery();
                    }
                    Classes.Remove(selectedClass);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error deleting class: " + ex.Message);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolAppConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT MAX(Id) FROM Classes";
                    SqlCommand command = new SqlCommand(query, connection);
                    int newId = (int)command.ExecuteScalar();
                    if (newId == 0)
                    {
                        newId = 1;
                    }
                    else
                    {
                        newId++;
                    }
                    string queryInsert = "INSERT INTO Classes (Id, Name) VALUES (@Id, @Name)";
                    SqlCommand commandInsert = new SqlCommand(queryInsert, connection);
                    commandInsert.Parameters.AddWithValue("@Id", newId);
                    commandInsert.Parameters.AddWithValue("@Name", "Новый класс");
                    commandInsert.ExecuteNonQuery();
                }
                LoadData();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error adding class: " + ex.Message);
            }
        }

        private void SaveData()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchoolAppConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (Class @class in Classes)
                    {
                        string query = "UPDATE Classes SET Name = @Name WHERE Id = @Id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Name", @class.Name);
                        command.Parameters.AddWithValue("@Id", @class.Id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message);
            }
        }
    }

    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
