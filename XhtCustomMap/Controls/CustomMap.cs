using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XhtCustomMap.Controls
{
    public class CustomMap : Map
    {
        public CustomMap(MapSpan region)
        {
            MoveToRegion(region);
        }

        public event EventHandler<MapEventArgs> MapMoved;

        public void OnMapMoved(MapEventArgs e)
        {
            MapMoved?.Invoke(this, e);
        }
    }

    public class MapEventArgs : EventArgs
    {
        public MapEventArgs(double latitude, double longitude, double radius)
        {
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
        }

        public double Latitude { private set; get; }

        public double Longitude { private set; get; }

        public double Radius { private set; get; }
    }
}
