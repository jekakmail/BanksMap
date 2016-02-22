using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using MapBanks.Views;

namespace MapBanks.Markers
{
    public partial class DepartmentPicker
    {
        Popup _popup;
        Label _label;
        GMapMarker _marker;
        //MainWindow _mainWindow;

        public DepartmentPicker()
        {
            InitializeComponent();
        }

        public DepartmentPicker(GMapMarker marker, string title)
        {
            InitializeComponent();

            //this.InitializeComponent();

            //this._mainWindow = window;
            this._marker = marker;

            _popup = new Popup();
            _label = new Label();

            this.Unloaded += new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            //this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
            Loaded += new RoutedEventHandler(Target);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
            // this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

            _popup.Placement = PlacementMode.Mouse;
            {
                _label.Background = Brushes.Blue;
                _label.Foreground = Brushes.White;
                _label.BorderBrush = Brushes.WhiteSmoke;
                _label.BorderThickness = new Thickness(2);
                _label.Padding = new Thickness(5);
                _label.FontSize = 22;
                _label.Content = title;
            }
            _popup.Child = _label;
        }

        private void Target(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Picker.Source.CanFreeze)
            {
                Picker.Source.Freeze();
            }
        }

        void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            if (Picker.Source.CanFreeze)
            {
                Picker.Source.Freeze();
            }
        }

        void CustomMarkerDemo_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            this.Loaded -= new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged -= new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter -= new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave -= new MouseEventHandler(MarkerControl_MouseLeave);
            //this.MouseMove -= new MouseEventHandler(CustomMarkerDemo_MouseMove);
            this.MouseLeftButtonUp -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            this.MouseLeftButtonDown -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

            _marker.Shape = null;
            Picker.Source = null;
            Picker = null;
            _popup = null;
            _label = null;
        }

        void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        //void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
        //    {
        //        Point p = e.GetPosition(_mainWindow.GMapCtrl);
        //        _marker.Position = _mainWindow.GMapCtrl.FromLocalToLatLng((int)(p.X), (int)(p.Y));
        //    }
        //}

        void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                Mouse.Capture(this);
            }
        }

        void CustomMarkerDemo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
            }
        }

        void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
        {
            _marker.ZIndex -= 10000;
            _popup.IsOpen = false;
        }

        void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            _marker.ZIndex += 10000;
            _popup.IsOpen = true;
        }
    }
}
