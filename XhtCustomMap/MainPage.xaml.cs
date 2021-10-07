using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XhtCustomMap.Controls;

namespace XhtCustomMap
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MyMap.MapMoved += UpdateCoordinates;
        }

        ~MainPage()
        {
            MyMap.MapMoved -= UpdateCoordinates;
        }

        private void UpdateCoordinates(object sender, MapEventArgs e)
        {
            Latitude.Text = $"Latitude: {e.Latitude}";
            Longitude.Text = $"Longitude: {e.Longitude}";
            Radius.Text = $"Radius: {e.Radius}km";
        }
    }
}
