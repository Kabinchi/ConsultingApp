using ConsultingApp.Models;
using ConsultingApp.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ConsultingApp
{
    /// <summary>
    /// Логика взаимодействия для ProjectServicesEditWindow.xaml
    /// </summary>
    public partial class ProjectServicesEditWindow : Window
    {
        private MyDbContext _context;
        private ProjectService _projectService;
        private Action _reloadMethod;

        public ProjectServicesEditWindow(MyDbContext context, ProjectService projectService, Action reloadMethod)
        {
            InitializeComponent();
            _context = context;
            _projectService = projectService;
            _reloadMethod = reloadMethod;
            LoadProjects();
            LoadServices();
            SetFields();
        }

        private void LoadProjects()
        {
            var projects = _context.Projects.ToList();
            ProjectComboBox.ItemsSource = projects;
        }

        private void LoadServices()
        {
            var services = _context.Services.ToList();
            ServiceComboBox.ItemsSource = services;
        }

        private void SetFields()
        {
            ProjectComboBox.SelectedValue = _projectService.Project_ID;
            ServiceComboBox.SelectedValue = _projectService.Service_ID;
            QuantityTextBox.Text = _projectService.Quantity.ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _projectService.Project_ID = (int)ProjectComboBox.SelectedValue;
            _projectService.Service_ID = (int)ServiceComboBox.SelectedValue;
            _projectService.Quantity = int.Parse(QuantityTextBox.Text);

            DataOperations.UpdateRow(_context, _projectService, _reloadMethod);
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NumberValidationTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!IsTextAllowed(textBox.Text, "^[0-9]*$"))
            {
                MessageBox.Show("Пожалуйста, вводите только цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.Text = Regex.Replace(textBox.Text, "[^0-9]", "");
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private static bool IsTextAllowed(string text, string pattern)
        {
            return Regex.IsMatch(text, pattern);
        }
    }
}
