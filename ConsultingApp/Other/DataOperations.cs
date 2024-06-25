using System;
using System.Windows;
using ConsultingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultingApp.Other
{
    public static class DataOperations
    {
        public static void AddRow<T>(MyDbContext context, T entity, Action reloadMethod) where T : class
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
            reloadMethod?.Invoke();
            MessageBox.Show("Запись успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void UpdateRow<T>(MyDbContext context, T entity, Action reloadMethod) where T : class
        {
            // Пытаемся прикрепить сущность к контексту, если она уже не отслеживается
            if (!context.Set<T>().Local.Contains(entity))
            {
                context.Set<T>().Attach(entity);
            }

            context.Entry(entity).State = EntityState.Modified; // Устанавливаем состояние Modified
            context.SaveChanges();
            reloadMethod?.Invoke();
            MessageBox.Show("Запись успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static void DeleteRow<T>(MyDbContext context, int id, Action reloadMethod) where T : class
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var entity = context.Set<T>().Find(id);
                    if (entity != null)
                    {
                        context.Set<T>().Remove(entity);
                        context.SaveChanges();
                        reloadMethod?.Invoke();
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlException && (sqlException.Number == 547 || sqlException.Number == 2601))
                    {
                        MessageBox.Show("На эту запись ссылаются другие записи. Удаление невозможно.", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении записи: " + ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
