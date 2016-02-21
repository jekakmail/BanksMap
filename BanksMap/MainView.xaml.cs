using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BanksMap.Markers;
using Database.Entity;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using BanksMap.Controls;

namespace BanksMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {

        public MainView()
        {
            InitializeComponent();

           
        }

        private void GMapControl_Loaded(object sender, RoutedEventArgs e)
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

        private void CbBanks_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ObservableCollection<Department> lstDepartmens = Database.Func.GetDepartments((sender as ComboBox).SelectedItem as Bank);
            
            //gMapCtrl_LoadPickers(Database.Func.GetDepartments((sender as ComboBox).SelectedItem as Bank));
        }

        void gMapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Переделать в соответствии с MVVM

            System.Windows.Point p = e.GetPosition(GMapCtrl);
            GMapMarker newMarker = new GMapMarker(GMapCtrl.Position);
            newMarker.Shape = new UserMarker(this, newMarker, "Пользовательский маркер");
            newMarker.Position = GMapCtrl.FromLocalToLatLng((int)p.X, (int)p.Y);
            GMapCtrl.Markers.Add(newMarker);
            
        }
        
    }
}
