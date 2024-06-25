using ConsultingApp.Models;
using ConsultingApp;
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
using ConsultingApp.Other;

namespace ConsultingApp
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private MyDbContext _context;

        public UserWindow()
        {
            InitializeComponent();
            _context = new MyDbContext();
            LoadUserData();
        }

        private void LoadUserData()
        {
            DataGridTable.ItemsSource = _context.Users.ToList();
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
            var addUserWindow = new UserAddWindow(_context, LoadUserData);
            addUserWindow.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (User)DataGridTable.SelectedItem;
            if (selectedUser != null)
            {
                var editWindow = new UserEditWindow(_context, selectedUser, LoadUserData);
                editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (User)DataGridTable.SelectedItem;
            if (selectedUser != null)
            {
                DataOperations.DeleteRow<User>(_context, selectedUser.Users_ID, LoadUserData);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        private void ProjectEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var projectEmployeeWindow = new ProjectEmployeeWindow();
            projectEmployeeWindow.Show();
            Close();
        }
    }
}
