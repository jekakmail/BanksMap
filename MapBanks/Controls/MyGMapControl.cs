using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Catel;
using Catel.Data;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using MapBanks.ViewModels;

namespace MapBanks.Controls
{
    public class MyGMapControl : GMapControl, IAdvancedNotifyPropertyChanged
    {
        public ObservableCollection<GMapMarker> LstMarkers
        {
            get { return (ObservableCollection<GMapMarker>) GetValue(LstMarkersProperty); }
            set
            {
                SetValue(LstMarkersProperty, value);
                this.Markers.Clear();
                foreach (var item in value)
                {
                    this.Markers.Add(item);
                }
            }
        }

        public static DependencyProperty LstMarkersProperty;
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string NameProperty)
        {
            PropertyChangedEventHandler PropertyChanged = this.PropertyChanged;
            if(PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(NameProperty));
        }
        
        static MyGMapControl()
        {
            LstMarkersProperty = DependencyProperty.Register("LstMarkers", typeof (ObservableCollection<GMapMarker>),
                typeof (MyGMapControl) );

            
        }

        public MyGMapControl()
        {
            

    }
}
