using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace School
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
            new teachers().ShowDialog();
        }

        private void StudentsButton_Click(object sender, RoutedEventArgs e)
        {
            new students().ShowDialog();
        }

        private void ClassesButton_Click(object sender, RoutedEventArgs e)
        {
            new classes().ShowDialog();
        }
    }
}