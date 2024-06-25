using ConsultingApp.Models;
using ConsultingApp.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для InvoicesAddWindow.xaml
    /// </summary>
    public partial class InvoicesAddWindow : Window
    {
        private MyDbContext _context;
        private Action _reloadMethod;

        public InvoicesAddWindow(MyDbContext context, Action reloadMethod)
        {
            InitializeComponent();
            _context = context;
            _reloadMethod = reloadMethod;
            LoadProjects();
        }

        private void LoadProjects()
        {
            // Получим список клиентов из контекста базы данных
            var projects = _context.Projects.ToList();
            // Установим источник данных для ComboBox
            ProjectComboBox.ItemsSource = projects;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var NewInvoice = new Invoice
            {
                Project_ID = (int)ProjectComboBox.SelectedValue,
                DateIssued = DateTime.Parse(DateIssuedPicker.Text),
                Amount = int.Parse(AmountTextBox.Text),
                Status = (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString()
            };

            DataOperations.AddRow(_context, NewInvoice, _reloadMethod);
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

        private void AlphaNumericValidationTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!IsTextAllowed(textBox.Text, "^[a-zA-Zа-яА-ЯёЁ0-9 ]*$"))
            {
                MessageBox.Show("Пожалуйста, вводите только буквы (русские и латинские), цифры и пробелы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.Text = Regex.Replace(textBox.Text, "[^a-zA-Zа-яА-ЯёЁ0-9 ]", "");
                textBox.SelectionStart = textBox.Text.Length;
            }
        }


        private void PhonePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextAllowed(e.Text, "^[0-9]*$"))
            {
                MessageBox.Show("Пожалуйста, вводите только цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Handled = true;
            }
        }

        private void PhoneValidationTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string phonePattern = @"^7\d{0,10}$";

            if (!Regex.IsMatch(textBox.Text, phonePattern))
            {
                MessageBox.Show("Пожалуйста, вводите номер телефона в формате 7xxxxxxxxxx.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.Text = Regex.Replace(textBox.Text, @"[^7\d]", "");
                if (textBox.Text.Length > 11)
                {
                    textBox.Text = textBox.Text.Substring(0, 11);
                }
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void TextValidationTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!IsTextAllowed(textBox.Text, "^[a-zA-Zа-яА-ЯёЁ ]*$"))
            {
                MessageBox.Show("Пожалуйста, вводите только буквы и пробелы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.Text = Regex.Replace(textBox.Text, "[^a-zA-Zа-яА-ЯёЁ ]", "");
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void AdressValidationTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!IsTextAllowed(textBox.Text, "^[a-zA-Zа-яА-ЯёЁ0-9 ,.]*$"))
            {
                MessageBox.Show("Пожалуйста, вводите только буквы (русские и латинские), цифры, пробелы, точки и запятые.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.Text = Regex.Replace(textBox.Text, "[^a-zA-Zа-яА-ЯёЁ0-9 ,.]", "");
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private static bool IsTextAllowed(string text, string pattern)
        {
            return Regex.IsMatch(text, pattern);
        }
    }
}
