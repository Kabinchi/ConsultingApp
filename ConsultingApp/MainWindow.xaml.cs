using System.Windows;
using Microsoft.EntityFrameworkCore;
using ConsultingApp.Models;
using System.Linq;

namespace ConsultingApp
{
    public partial class MainWindow : Window
    {
        private MyDbContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new MyDbContext();

            // Привязка событий к кнопкам
            ClientsButton.Click += (s, e) => LoadClients();
            EmployeesButton.Click += (s, e) => LoadEmployees();
            ProjectsButton.Click += (s, e) => LoadProjects();
            ServicesButton.Click += (s, e) => LoadServices();
            ProjectServicesButton.Click += (s, e) => LoadProjectServices();
            InvoicesButton.Click += (s, e) => LoadInvoices();

            // Загрузить данные по умолчанию
            LoadClients();
        }

        private void LoadClients()
        {
            DataGridTable.ItemsSource = _context.Clients.ToList();
        }

        private void LoadEmployees()
        {
            DataGridTable.ItemsSource = _context.Employees.ToList();
        }

        private void LoadProjects()
        {
            DataGridTable.ItemsSource = _context.Projects.ToList();
        }

        private void LoadServices()
        {
            DataGridTable.ItemsSource = _context.Services.ToList();
        }

        private void LoadProjectServices()
        {
            DataGridTable.ItemsSource = _context.ProjectServices
                .Include(ps => ps.Project)
                .Include(ps => ps.Service)
                .ToList();
        }

        private void LoadInvoices()
        {
            DataGridTable.ItemsSource = _context.Invoices
                .Include(i => i.Project)
                .ToList();
        }
    }
}
