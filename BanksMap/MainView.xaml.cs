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
            //this.DataContext = this;
        }

        private void GMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GMapControl.MapProvider = GMapProviders.OpenStreetMap;
            GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
                
            GMapControl.CanDragMap = true;
            //GMapControl.DragButton = MouseButton.Left;
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
            GMapControl.ShowCenter = false;

            //GMapControl.MouseLeftButtonDown += gMapControl_MouseLeftButtonDown;

        }

        private void CbBanks_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Department> lstDepartmens = Database.Func.GetDepartments((sender as ComboBox).SelectedItem as Bank);
            GMapControl.Markers.Clear();
            

                foreach (var depart in lstDepartmens)
                {
                    GMapMarker point = new GMapMarker(new PointLatLng(depart.latitude,depart.longitude));
                     
                    point.Shape = new DepartmentMarker(this, point, depart.Name);
                   //point.Offset = new Point(-16, -32);
                    point.ZIndex = int.MaxValue;

                    GMapControl.Markers.Add(point);
                   
                }
            
        }

        void gMapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Переделать в соответствии с MVVM

            System.Windows.Point p = e.GetPosition(GMapControl);
            GMapMarker NewMarker = new GMapMarker(GMapControl.Position);
            NewMarker.Shape = new UserMarker(this, NewMarker, "Пользовательский маркер");
            NewMarker.Position = GMapControl.FromLocalToLatLng((int)p.X, (int)p.Y);
            GMapControl.Markers.Add(NewMarker);

        }
    }
}
