using System;
using Android.Content;
using Android.Gms.Maps;
using Android.Locations;
using DarkIce.Toolkit.Core.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using XhtCustomMap.Controls;
using XhtCustomMap.Droid.Renderers;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer_Android))]
namespace XhtCustomMap.Droid.Renderers
{
    public class CustomMapRenderer_Android : MapRenderer
    {
        private CustomMap _formsMap;

        private Context _localContext;

        public CustomMapRenderer_Android(Context context) : base(context)
        {
            _localContext = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.CameraIdle -= CameraIdle;
            }

            if (e.NewElement != null)
            {
                _formsMap = (CustomMap)e.NewElement;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.CameraIdle += CameraIdle;
        }

        private void CameraIdle(object sender, EventArgs e)
        {
            _formsMap.OnMapMoved(new MapEventArgs(NativeMap.CameraPosition.Target.Latitude, NativeMap.CameraPosition.Target.Longitude, GetRadius()));
            $"Latitude: {NativeMap.CameraPosition.Target.Latitude}, Longitude: {NativeMap.CameraPosition.Target.Longitude}, Radius: {GetRadius()}km".Log();            
        }

        private double GetRadius()
        {
            var visibleRegion = NativeMap.Projection.VisibleRegion;

            var farRight = visibleRegion.FarRight;
            var farLeft = visibleRegion.FarLeft;
            var nearRight = visibleRegion.NearRight;
            var nearLeft = visibleRegion.NearLeft;

            float[] distanceWidth = new float[2];
            Location.DistanceBetween(
                    (farRight.Latitude + nearRight.Latitude) / 2,
                    (farRight.Longitude + nearRight.Longitude) / 2,
                    (farLeft.Latitude + nearLeft.Latitude) / 2,
                    (farLeft.Longitude + nearLeft.Longitude) / 2,
                    distanceWidth
                    );


            float[] distanceHeight = new float[2];
            Location.DistanceBetween(
                    (farRight.Latitude + nearRight.Latitude) / 2,
                    (farRight.Longitude + nearRight.Longitude) / 2,
                    (farLeft.Latitude + nearLeft.Latitude) / 2,
                    (farLeft.Longitude + nearLeft.Longitude) / 2,
                    distanceHeight
            );

            if (distanceWidth[0] > distanceHeight[0])
            {
                return distanceWidth[0] / 1000.0 / 2.0;
            }
            else
            {
                return distanceHeight[0] / 1000.0 / 2.0;
            }
        }
    }
}
