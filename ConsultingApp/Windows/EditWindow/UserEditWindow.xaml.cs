using ConsultingApp.Models;
using ConsultingApp.Other;
using System;
using System.Windows;
using System.Windows.Controls;
using ConsultingApp.Models;
using ConsultingApp.Other;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConsultingApp
{
    public partial class UserEditWindow : Window
    {
        private MyDbContext _context;
        private Action _reloadMethod;
        private User _user;

        public UserEditWindow(MyDbContext context, User user, Action reloadMethod)
        {
            InitializeComponent();
            _context = context;
            _user = user;
            _reloadMethod = reloadMethod;

            // Заполнение полей данными текущего пользователя для редактирования
            NameTextBox.Text = _user.Name;
            LoginTextBox.Text = _user.Login;
            PasswordTextBox.Text = _user.Password;

            // Выбор роли в ComboBox
            if (_user.Role == "Админ")
                RoleComboBox.SelectedIndex = 0;
            else if (_user.Role == "Пользователь")
                RoleComboBox.SelectedIndex = 1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновление данных пользователя
            _user.Name = NameTextBox.Text;
            _user.Login = LoginTextBox.Text;
            _user.Password = PasswordTextBox.Text;
            _user.Role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            DataOperations.UpdateRow(_context, _user, _reloadMethod);
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
