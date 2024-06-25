using ConsultingApp.Models;
using ConsultingApp.Other;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ConsultingApp
{
    public partial class InvoicesEditWindow : Window
    {
        private MyDbContext _context;
        private Action _reloadMethod;
        private Invoice _invoice;

        public InvoicesEditWindow(MyDbContext context, Invoice invoice, Action reloadMethod)
        {
            InitializeComponent();
            _context = context;
            _reloadMethod = reloadMethod;
            _invoice = invoice;
            LoadData();
        }

        private void LoadData()
        {
            // Загрузка данных в поля редактирования
            ProjectComboBox.ItemsSource = _context.Projects.ToList();
            ProjectComboBox.SelectedValue = _invoice.Project_ID;
            DateIssuedPicker.SelectedDate = _invoice.DateIssued;
            AmountTextBox.Text = _invoice.Amount.ToString();
            StatusComboBox.SelectedValue = _invoice.Status;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновление сущности Invoice
            _invoice.Project_ID = (int)ProjectComboBox.SelectedValue;
            _invoice.DateIssued = DateIssuedPicker.SelectedDate ?? DateTime.MinValue;
            _invoice.Amount = int.Parse(AmountTextBox.Text);
            _invoice.Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            DataOperations.UpdateRow(_context, _invoice, _reloadMethod);
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

        private static bool IsTextAllowed(string text, string pattern)
        {
            return Regex.IsMatch(text, pattern);
        }
    }
}
