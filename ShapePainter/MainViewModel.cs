using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace ShapePainter
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<DbShape> Shapes { get; set; } = new ObservableCollection<DbShape>();

        public RelayCommand ClearAllCommand { get; init; }
        public RelayCommand SaveCommand { get; init; } // Новая команда сохранения

        public MainViewModel()
        {
            ClearAllCommand = new RelayCommand(async (o) =>
            {
                try
                {
                    await ClearAllShapesAsync();
                }
                catch (Exception ex)
                {
                    // ИСПРАВЛЕНО: Убран лишний знак доллара
                    MessageBox.Show($"Не удалось удалить фигуры: {ex.Message}", "Ошибка очистки", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            // Реализация команды сохранения
            SaveCommand = new RelayCommand(async (o) =>
            {
                try
                {
                    await SaveAllShapesAsync();
                    MessageBox.Show("Все фигуры успешно сохранены в базе данных!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // ИСПРАВЛЕНО: Убран лишний знак доллара
                    MessageBox.Show($"Не удалось сохранить фигуры: {ex.Message}", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        public async Task LoadShapesAsync()
        {
            await DbHelper.CreateDbAsync();

            var list = await DbHelper.GetShapesAsync();

            // Выполняем очистку и наполнение строго в глав ном (UI) потоке приложения
            Application.Current.Dispatcher.Invoke(() =>
            {
                Shapes.Clear();
                foreach (var shape in list)
                {
                    Shapes.Add(shape);
                }
            });

            OnPropertyChanged(nameof(Shapes));
        }

        // Локальное добавление фигуры без обращения к БД на каждый клик
        public void AddRectangleLocal(double x, double y)
        {
            var newShape = new DbShape
            {
                X = x - 25,
                Y = y - 25,
                Width = 50,
                Height = 50,
                Type = ShapeType.Rectangle
            };

            Shapes.Add(newShape);
        }

        // Метод сохранения текущего набора фигур в БД
        private async Task SaveAllShapesAsync()
        {
            // Сначала очищаем старые записи в таблице, чтобы не дублировать
            await DbHelper.ClearAllShapesAsync();

            // Переносим всё из памяти в базу
            foreach (var shape in Shapes)
            {
                await DbHelper.AddShapeAsync(shape);
            }
        }

        private async Task ClearAllShapesAsync()
        {
            await DbHelper.ClearAllShapesAsync();
            Shapes.Clear();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}