using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Catel.Collections;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using MapBanks.Markers;

namespace MapBanks.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GMapCtrl_OnLoaded(object sender, RoutedEventArgs e)
        {
            GMapCtrl.MapProvider = GMapProviders.OpenStreetMap;
            GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

            GMapCtrl.CanDragMap = true;
            //GMapControl.DragButton = MouseButton.Left;
            GMapCtrl.Position = new GMap.NET.PointLatLng(53.902800, 27.561759);
            GMapCtrl.Bearing = 0;

            // МАСШТАБИРОВАНИЕ
            //Указываем значение максимального приближения.
            GMapCtrl.MaxZoom = 18;
            //Указываем значение минимального приближения.
            GMapCtrl.MinZoom = 2;
            //Указываем, что при загрузке карты будет использоваться 
            //16ти кратной приближение.
            GMapCtrl.Zoom = 17;
            //Устанавливаем центр приближения/удаления
            //курсор мыши.
            GMapCtrl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            GMapCtrl.ShowCenter = false;

            //GMapControl.MouseLeftButtonDown += gMapControl_MouseLeftButtonDown;
            
            
        }

        private void GMapCtrl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(GMapCtrl);
            GMapMarker newMarker = new GMapMarker(GMapCtrl.Position);
            newMarker.Shape = new UserMarker(this, newMarker, "Пользовательский маркер");
            newMarker.Position = GMapCtrl.FromLocalToLatLng((int)p.X, (int)p.Y);
            GMapCtrl.Markers.Add(newMarker);
        }
    }
    
}
