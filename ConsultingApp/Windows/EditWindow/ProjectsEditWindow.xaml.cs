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
    public partial class ProjectsEditWindow : Window
    {
        private MyDbContext _context;
        private Project _project;
        private Action _reloadMethod;

        public ProjectsEditWindow(MyDbContext context, Project project, Action reloadMethod)
        {
            InitializeComponent();
            _context = context;
            _project = project;
            _reloadMethod = reloadMethod;
            LoadClients();
            LoadProjectData();
        }

        private void LoadClients()
        {
            var clients = _context.Clients.ToList();
            ClientComboBox.ItemsSource = clients;
            ClientComboBox.SelectedValue = _project.Client_ID;
        }

        private void LoadProjectData()
        {
            NameTextBox.Text = _project.Name;
            DescriptionTextBox.Text = _project.Description;
            StartDatePicker.SelectedDate = _project.StartDate;
            EndDatePicker.SelectedDate = _project.EndDate;

            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == _project.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _project.Client_ID = (int)ClientComboBox.SelectedValue;
            _project.Name = NameTextBox.Text;
            _project.Description = DescriptionTextBox.Text;
            _project.StartDate = StartDatePicker.SelectedDate.Value;
            _project.EndDate = EndDatePicker.SelectedDate.Value;
            _project.Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            DataOperations.UpdateRow(_context, _project, _reloadMethod);
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextValidationTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!IsTextAllowed(textBox.Text, "^[a-zA-Zа-яА-ЯёЁ0-9 ]*$"))
            {
                MessageBox.Show("Пожалуйста, вводите только буквы (русские и латинские), цифры и пробелы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.Text = Regex.Replace(textBox.Text, "[^a-zA-Zа-яА-ЯёЁ0-9 ]", "");
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private bool IsTextAllowed(string text, string pattern)
        {
            return Regex.IsMatch(text, pattern);
        }
    }
}
