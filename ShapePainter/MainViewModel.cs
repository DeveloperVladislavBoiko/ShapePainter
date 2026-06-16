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
                    MessageBox.Show($"Не удалось удалить фигуры: {ex.Message}", "Ошибка очистки", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        public async Task LoadShapesAsync()
        {
            await DbHelper.CreateDbAsync();

            var list = await DbHelper.GetShapesAsync();
            Shapes.Clear();
            foreach (var shape in list)
            {
                Shapes.Add(shape);
            }
        }

        public async Task AddRectangleAsync(double x, double y)
        {
            var newShape = new DbShape
            {
                X = x - 25,
                Y = y - 25,
                Width = 50,
                Height = 50,
                Type = ShapeType.Rectangle
            };

            await DbHelper.AddShapeAsync(newShape);
            Shapes.Add(newShape);
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