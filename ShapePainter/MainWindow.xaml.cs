using System;
using System.Windows;
using System.Windows.Input;

namespace ShapePainter
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Попытка загрузить фигуры при старте (теперь они сами отобразятся!)
                await viewModel.LoadShapesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Антисмачная ошибка при инициализации или загрузке БД:\n\n{ex.Message}\n\nСтек вызовов:\n{ex.StackTrace}",
                    "Критическая ошибка запуска",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Получаем координаты относительно холста
                Point clickPoint = e.GetPosition((IInputElement)sender);

                // Передаем координаты во ViewModel для локального добавления
                viewModel.AddRectangleLocal(clickPoint.X, clickPoint.Y);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при попытке нарисовать фигуру:\n\n{ex.Message}",
                    "Ошибка взаимодействия",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}