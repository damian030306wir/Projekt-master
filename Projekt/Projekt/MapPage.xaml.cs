using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;

namespace Projekt
{

    
    public partial class MapPage : ContentPage
    {

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
            customMap.Pins.Clear();
            customMap.MapElements.Clear();
            
           
        }
        private async void OnButton(object sender, EventArgs e)
        {
            CustomPin pin = new CustomPin
            {
                Type = PinType.SavedPin,
                Position = new Position(Convert.ToDouble(szerokosc.Text), Convert.ToDouble(dlugosc.Text)),
                Label = nazwaEntry.Text,
                Address = opisEntry.Text,
                Name = "Xamarin",
                Url = "http://xamarin.com/about/"
            };
            pin.MarkerClicked += async (s, args) =>
            {
                args.HideInfoWindow = true;
                string pinName = ((Pin)s).Label;
                string pytanie = ((Pin)s).Address;
                await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
                string result = await DisplayPromptAsync("Zagadka", $"{pytanie}", initialValue: "10", maxLength: 2, keyboard: Keyboard.Numeric);

            };

            customMap.CustomPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin);



            var json = JsonConvert.SerializeObject(new { X = pin.Position.Latitude, Y = pin.Position.Longitude });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("http://dearjean.ddns.net:44201/api/Points", content);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Komunikat", "Dodanie puntku przebiegło pomyślnie", "Anuluj");
            }

        }



        public MapPage()
        {
            InitializeComponent();

           
        }
    }
}
