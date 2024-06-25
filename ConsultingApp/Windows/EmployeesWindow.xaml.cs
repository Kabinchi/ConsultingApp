using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using ConsultingApp.Models;
using ConsultingApp;
using ConsultingApp.Other;

namespace ConsultingApp
{
    /// <summary>
    /// Логика взаимодействия для EmployeesWindow.xaml
    /// </summary>
    public partial class EmployeesWindow : Window
    {
        private MyDbContext _context;
        private Action _reloadMethod;

        public EmployeesWindow()
        {
            InitializeComponent();
            _context = new MyDbContext();
            _reloadMethod = LoadEmployeesData; // Пример инициализации делегата
            LoadEmployeesData();
        }


        private void LoadEmployeesData()
        {
            DataGridTable.ItemsSource = _context.Employees.ToList();
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
            var addEmployeeWindow = new EmployeesAddWindow(_context, LoadEmployeesData);
            addEmployeeWindow.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = (Employee)DataGridTable.SelectedItem;
            if (selectedEmployee != null)
            {
                var editWindow = new EmployeesEditWindow(_context, selectedEmployee, _reloadMethod);
                editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = (Employee)DataGridTable.SelectedItem;
            if (selectedEmployee != null)
            {
                DataOperations.DeleteRow<Employee>(_context, selectedEmployee.Employee_ID, LoadEmployeesData);
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

