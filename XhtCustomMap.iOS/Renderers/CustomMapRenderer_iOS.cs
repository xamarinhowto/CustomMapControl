using System;
using CoreLocation;
using DarkIce.Toolkit.Core.Utilities;
using MapKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using XhtCustomMap.Controls;
using XhtCustomMap.Renderers.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer_iOS))]
namespace XhtCustomMap.Renderers.iOS
{
    public class CustomMapRenderer_iOS : MapRenderer
    {
        private MKMapView _nativeMap;
        private CustomMap _formsMap;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            _nativeMap = Control as MKMapView;

            if (_nativeMap == null)
            {
                _nativeMap = new MKMapView();
                SetNativeControl(_nativeMap);
            }

            if (e.OldElement != null)
            {
                _nativeMap.RegionChanged -= RegionChanged;
            }

            if (e.NewElement != null)
            {
                _formsMap = (CustomMap)e.NewElement;
                _nativeMap.RegionChanged += RegionChanged;
            }
        }

        private void RegionChanged(object sender, MKMapViewChangeEventArgs e)
        {
            _formsMap.OnMapMoved(new MapEventArgs(_nativeMap.CenterCoordinate.Latitude, _nativeMap.CenterCoordinate.Longitude, GetRadius()));
            $"Latitude: {_nativeMap.CenterCoordinate.Latitude}, Longitude: {_nativeMap.CenterCoordinate.Longitude}, Radius: {GetRadius()}km".Log();
        }

        private double GetRadius()
        {
            var span = _nativeMap.Region.Span;
            var center = _nativeMap.Region.Center;

            var loc1 = new CLLocation(latitude: center.Latitude - span.LatitudeDelta * 0.5, longitude: center.Longitude);
            var loc2 = new CLLocation(latitude: center.Latitude + span.LatitudeDelta * 0.5, longitude: center.Longitude);
            var loc3 = new CLLocation(latitude: center.Latitude, longitude: center.Longitude - span.LongitudeDelta * 0.5);
            var loc4 = new CLLocation(latitude: center.Latitude, longitude: center.Longitude + span.LongitudeDelta * 0.5);

            var metresInLatitude = loc1.DistanceFrom(loc2);
            var metresInLongitude = loc3.DistanceFrom(loc4);

            var diameter = Math.Min(metresInLatitude, metresInLongitude);
            return diameter / 1000.0 / 2.0;
        }
    }
}
