﻿using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Projekt
{
    [DesignTimeVisible(false)]
    public partial class Mapa : ContentPage
    {
        public ObservableCollection<Position> positions = new ObservableCollection<Position>();
       

        public Mapa()
        {
            InitializeComponent();

        }
        void ListaPunktowPage2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page2());
        }
        void DodajZdjecie(object sender, EventArgs e)
        {
            DisplayAlert("Sukces", "Obrazek został dodany pomyślnie", "Anuluj");
        }
        public void ClearButton(object sender, EventArgs e)
        {
            maps.Pins.Clear();
            maps.MapElements.Clear();
            positions.Clear();
        }
        public void OnMapClicked(object sender, MapClickedEventArgs e)
        {

            Pin pin = new Pin
            {

                Label = "Nazwa",
                Address = "adres",
                Type = PinType.SavedPin,
                Position = new Position(e.Position.Latitude, e.Position.Longitude)
            };
            positions.Add(new Position(e.Position.Latitude, e.Position.Longitude));
            Polyline polyline = new Polyline
            {
                StrokeColor = Color.Blue,
                StrokeWidth = 6
            };
            foreach (Position pos in positions)
            {
                polyline.Geopath.Add(pos);
                
            }
            maps.Pins.Add(pin);
            maps.MapElements.Add(polyline);


        }

        private async void OnButton(object sender, EventArgs e) 
        {


            Pin pin = new Pin
            {

                Label = nazwaEntry.Text,
                Address = opisEntry.Text,
                Type = PinType.SavedPin,
                Position = new Position(Convert.ToDouble(szerokosc.Text), Convert.ToDouble(dlugosc.Text))

            };
            pin.MarkerClicked += async (s, args) =>
            {
                args.HideInfoWindow = true;
                string pinName = ((Pin)s).Label;
                string pytanie = ((Pin)s).Address;
                await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
                string result = await DisplayPromptAsync("Zagadka", $"{pytanie}", initialValue: "10", maxLength: 2, keyboard: Keyboard.Numeric);
               
            };

            positions.Add(new Position(Convert.ToDouble(szerokosc.Text), Convert.ToDouble(dlugosc.Text)));
            Polyline polyline = new Polyline
            {
                StrokeColor = Color.Blue,
                StrokeWidth = 6
            };
            foreach (Position pos in positions)
            {
                polyline.Geopath.Add(pos);

            }
            maps.Pins.Add(pin);
            maps.MapElements.Add(polyline);

            var json = JsonConvert.SerializeObject(new { X = pin.Position.Latitude, Y = pin.Position.Longitude });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("http://dearjean.ddns.net:44201/api/Points", content);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Komunikat", "Dodanie puntku przebiegło pomyślnie", "Anuluj");
            }
           
        }
    }
}
