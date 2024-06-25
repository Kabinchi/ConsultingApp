using ConsultingApp.Models;
using ConsultingApp.Other;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConsultingApp
{
    public partial class EmployeesEditWindow : Window
    {
        private MyDbContext _context;
        private Employee _employee;
        private Action _reloadMethod;

        public EmployeesEditWindow(MyDbContext context, Employee employee, Action reloadMethod)
        {
            InitializeComponent();
            _context = context;
            _employee = employee;
            _reloadMethod = reloadMethod;
            InitializeFields();
        }

        private void InitializeFields()
        {
            FirstNameTextBox.Text = _employee.FirstName;
            LastNameTextBox.Text = _employee.LastName;
            PositionTextBox.Text = _employee.Position;
            EmailTextBox.Text = _employee.Email;
            PhoneTextBox.Text = _employee.Phone;
            HireDatePicker.SelectedDate = _employee.HireDate;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _employee.FirstName = FirstNameTextBox.Text;
            _employee.LastName = LastNameTextBox.Text;
            _employee.Position = PositionTextBox.Text;
            _employee.Phone = PhoneTextBox.Text;
            _employee.Email = EmailTextBox.Text;
            _employee.HireDate = HireDatePicker.SelectedDate ?? DateTime.MinValue; // Ensure you handle nullable DateTimes properly

            DataOperations.UpdateRow(_context, _employee, _reloadMethod);
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

        private static bool IsTextAllowed(string text, string pattern)
        {
            return Regex.IsMatch(text, pattern);
        }
    }
}
