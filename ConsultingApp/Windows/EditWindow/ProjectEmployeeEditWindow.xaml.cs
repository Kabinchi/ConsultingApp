using ConsultingApp.Models;
using ConsultingApp.Other;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ConsultingApp
{
    public partial class ProjectEmployeeEditWindow : Window
    {
        private MyDbContext _context;
        private ProjectEmployee _projectEmployee;
        private Action _reloadMethod;

        public ProjectEmployeeEditWindow(MyDbContext context, ProjectEmployee projectEmployee, Action reloadMethod)
        {
            InitializeComponent();
            _context = context;
            _projectEmployee = projectEmployee;
            _reloadMethod = reloadMethod;
            LoadProjects();
            LoadEmployees();
            SetFields();
        }

        private void LoadProjects()
        {
            var projects = _context.Projects.ToList();
            ProjectComboBox.ItemsSource = projects;
        }

        private void LoadEmployees()
        {
            var employees = _context.Employees.ToList();
            EmployeeComboBox.ItemsSource = employees;
        }

        private void SetFields()
        {
            ProjectComboBox.SelectedValue = _projectEmployee.Project_ID;
            EmployeeComboBox.SelectedValue = _projectEmployee.Employee_ID;

            foreach (ComboBoxItem item in RoleComboBox.Items)
            {
                if (item.Content.ToString() == _projectEmployee.Role)
                {
                    RoleComboBox.SelectedItem = item;
                    break;
                }
            }

            HoursWorkedTextBox.Text = _projectEmployee.HoursWorked.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _projectEmployee.Project_ID = (int)ProjectComboBox.SelectedValue;
            _projectEmployee.Employee_ID = (int)EmployeeComboBox.SelectedValue;
            _projectEmployee.Role = (RoleComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            _projectEmployee.HoursWorked = int.Parse(HoursWorkedTextBox.Text);

            DataOperations.UpdateRow(_context, _projectEmployee, _reloadMethod);
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
