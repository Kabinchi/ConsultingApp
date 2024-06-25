using ConsultingApp.Models;
using ConsultingApp;
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
using ConsultingApp.Other;

namespace ConsultingApp
{
    /// <summary>
    /// Логика взаимодействия для ProjectsWindow.xaml
    /// </summary>
    public partial class ProjectsWindow : Window
    {
        private MyDbContext _context;

        public ProjectsWindow()
        {
            InitializeComponent();
            _context = new MyDbContext();
            LoadProjectsData();
        }

        private void LoadProjectsData()
        {
            using (var context = new MyDbContext())
            {
                var projects = context.Projects.Include(i => i.Client).ToList();
                DataGridTable.ItemsSource = projects;
            }
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
            var addProjectWindow = new ProjectsAddWindow(_context, LoadProjectsData);
            addProjectWindow.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProject = (Project)DataGridTable.SelectedItem;
            if (selectedProject != null)
            {
                // Отключаем отслеживание сущности Client при загрузке проекта
                var project = _context.Projects.AsNoTracking().FirstOrDefault(p => p.Project_ID == selectedProject.Project_ID);
                var editWindow = new ProjectsEditWindow(_context, project, LoadProjectsData);
                editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProject = (Project)DataGridTable.SelectedItem;
            if (selectedProject != null)
            {
                DataOperations.DeleteRow<Project>(_context, selectedProject.Project_ID, LoadProjectsData);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            var clientsWindow = new ClientsWindow();
            clientsWindow.Show();
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
