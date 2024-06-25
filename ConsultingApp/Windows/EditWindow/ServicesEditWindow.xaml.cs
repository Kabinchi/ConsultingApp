using System;
using System.Collections.Generic;
using ConsultingApp.Models;
using ConsultingApp.Other;
using System;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConsultingApp
{
    /// <summary>
    /// Логика взаимодействия для ServicesEditWindow.xaml
    /// </summary>
    public partial class ServicesEditWindow : Window
    {
        private MyDbContext _context;
        private Service _serviceToEdit;
        private Action _reloadMethod;

        public ServicesEditWindow(MyDbContext context, Service serviceToEdit, Action reloadMethod)
        {
            InitializeComponent();
            _context = context;
            _serviceToEdit = serviceToEdit;
            _reloadMethod = reloadMethod;

            // Установка начальных значений полей в окне редактирования
            NameTextBox.Text = _serviceToEdit.Name;
            DescriptionTextBox.Text = _serviceToEdit.Description;
            RateTextBox.Text = _serviceToEdit.Rate.ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем поля объекта _serviceToEdit перед сохранением
            _serviceToEdit.Name = NameTextBox.Text;
            _serviceToEdit.Description = DescriptionTextBox.Text;
            _serviceToEdit.Rate = int.Parse(RateTextBox.Text);

            DataOperations.UpdateRow(_context, _serviceToEdit, _reloadMethod);
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
