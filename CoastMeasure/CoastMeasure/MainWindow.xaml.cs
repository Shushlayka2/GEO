using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using eventHandler = CoastMeasureEventHandler;

namespace CoastMeasure
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        eventHandler.CoasMeasureEventHandler handler = new eventHandler.CoasMeasureEventHandler();
        CoastReflection coastReflection;

        public MainWindow()
        {
            InitializeComponent();
            handler.InitWaterPlace((BitmapImage)this.Resources["bmapImg"]);
            coastReflection = new CoastReflection(handler.GetCoastReflection(740, 410));
        }

        private double oldPos = 0;
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            if (Math.Abs(e.NewValue - oldPos) > 1)
            {
                handler.FillWaterAsync(e.NewValue, this, coastReflection);
                oldPos = e.NewValue;
            }
        }
    }
}
