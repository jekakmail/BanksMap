using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BanksMap.lib;
using GMap.NET.MapProviders;

namespace BanksMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ParserMyFin _p;
        public MainWindow()
        {
            InitializeComponent();
            _p = new ParserMyFin();
            lib.Convertation conv = new lib.Convertation();
            conv.ConvertDepartmentsAddress();
           
        }

        private void GMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GMapControl.MapProvider = GMapProviders.OpenStreetMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

            GMapControl.CanDragMap = true;
            GMapControl.DragButton = MouseButton.Left;
            GMapControl.Position = new GMap.NET.PointLatLng(53.902800, 27.561759);
            GMapControl.Bearing = 0;

            // МАСШТАБИРОВАНИЕ
            //Указываем значение максимального приближения.
            GMapControl.MaxZoom = 18;
            //Указываем значение минимального приближения.
            GMapControl.MinZoom = 2;
            //Указываем, что при загрузке карты будет использоваться 
            //16ти кратной приближение.
            GMapControl.Zoom = 17;
            //Устанавливаем центр приближения/удаления
            //курсор мыши.
            GMapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
        }
    }
}
