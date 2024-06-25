using ConsultingApp.Models;
using ConsultingApp.Other;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

namespace ConsultingApp
{
    /// <summary>
    /// Логика взаимодействия для ProjectEmployeeWindow.xaml
    /// </summary>
    public partial class ProjectEmployeeWindow : Window
    {
        private MyDbContext _context;

        public ProjectEmployeeWindow()
        {
            InitializeComponent();
            _context = new MyDbContext();
            LoadProjectEmployeeData();
        }

        private void LoadProjectEmployeeData()
        {
            var data = _context.ProjectEmployees.Include(pe => pe.Project).Include(pe => pe.Employee).ToList();

            DataGridTable.ItemsSource = data;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            var selectedFilter = FilterComboBox.SelectedItem as ComboBoxItem;

            if (selectedFilter == null)
            {
                MessageBox.Show("Пожалуйста, выберите фильтр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var filterValue = selectedFilter.Content.ToString();

            using (var context = new MyDbContext())
            {
                IQueryable<Client> query = context.Clients;

                switch (filterValue)
                {
                    case "Название компании":
                        query = query.Where(c => c.Name.ToLower().Contains(searchText));
                        break;
                    case "Контактные данные":
                        query = query.Where(c => c.ContactPerson.ToLower().Contains(searchText));
                        break;
                    case "Телефон":
                        query = query.Where(c => c.Phone.ToLower().Contains(searchText));
                        break;
                    case "Почта":
                        query = query.Where(c => c.Email.ToLower().Contains(searchText));
                        break;
                    case "Адрес":
                        query = query.Where(c => c.Address.ToLower().Contains(searchText));
                        break;
                }

                DataGridTable.ItemsSource = query.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addProjectEmployeeWindow = new ProjectEmployeeAddWindow(_context, LoadProjectEmployeeData);
            addProjectEmployeeWindow.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProjectEmployee = (ProjectEmployee)DataGridTable.SelectedItem;
            if (selectedProjectEmployee != null)
            {
                var editWindow = new ProjectEmployeeEditWindow(_context, selectedProjectEmployee, LoadProjectEmployeeData);
                editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProjectEmployee = (ProjectEmployee)DataGridTable.SelectedItem;
            if (selectedProjectEmployee != null)
            {
                DataOperations.DeleteRow<ProjectEmployee>(_context, selectedProjectEmployee.ProjectEmployee_ID, LoadProjectEmployeeData);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            var clientsWindow = new ClientsWindow();
            clientsWindow.Show();
            Close();
        }

        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            var projectsWindow = new ProjectsWindow();
            projectsWindow.Show();
            Close();
        }

        private void ServicesButton_Click(object sender, RoutedEventArgs e)
        {
            var servicesWindow = new ServicesWindow();
            servicesWindow.Show();
            Close();
        }

        private void ProjectServicesButton_Click(object sender, RoutedEventArgs e)
        {
            var projectServicesWindow = new ProjectServicesWindow();
            projectServicesWindow.Show();
            Close();
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            var employeesWindow = new EmployeesWindow();
            employeesWindow.Show();
            Close();
        }

        private void InvoicesButton_Click(object sender, RoutedEventArgs e)
        {
            var invoicesWindow = new InvoicesWindow();
            invoicesWindow.Show();
            Close();
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            var usersWindow = new UserWindow();
            usersWindow.Show();
            Close();
        }

        private void ProjectEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var projectEmployeeWindow = new ProjectEmployeeWindow();
            projectEmployeeWindow.Show();
            Close();
        }
    }
}
