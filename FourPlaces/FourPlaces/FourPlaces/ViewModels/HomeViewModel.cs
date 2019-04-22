using FourPlaces.Models;
using MonkeyCache.SQLite;
using Plugin.Geolocator.Abstractions;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        public INavigation Navigation;
        public ICommand AddPlaceClicked { set; get; }
        public ICommand ProfileClicked { set; get; }
        public ICommand ReloadClicked { set; get; }
        private List<PlaceItemSummary> _places;
        private string _error;
        public List<PlaceItemSummary> Places
        {
            get => _places;
            set => SetProperty(ref _places, value);
        }
        public HomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.AddPlaceClicked = new Command(async () => await GotoAddPlace());
            this.ProfileClicked = new Command(async () => await GotoProfile());
            this.ReloadClicked = new Command(async () => await OnResume());

          //  this._places = Service.

        }
        public async Task GotoAddPlace()
        {
            Position position = await Service.GetCurrentLocation();

            if (position != null)
            {
                Barrel.Current.Add(key: "Position", data: position, expireIn: TimeSpan.FromDays(1));
            }
            await Navigation.PushAsync(new PlaceAddPage());
        }
        public async Task GotoProfile()
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        public override async Task OnResume()
        {
              await base.OnResume();
              _error = "";
              ListPlaces places = await Service.GetPlacesService();
              if (places != null)
              {
                Console.WriteLine("YEP");
                  foreach (PlaceItemSummary place in places.GetPlaces())
                  {
                      place.ImageURL =Service.URL + "images/" + place.ImageId;
                    Console.WriteLine(place.ImageURL);
                  }
                  Position position = await Service.GetCurrentLocation();

                  if (position != null)
                  {
                      Barrel.Current.Add(key: "Position", data: position, expireIn: TimeSpan.FromDays(1));
                  }
                  else
                  {
                      _error = "Pas de géolocalisation";
                  }
                  places.SortPlaces();
                  Places = places.GetPlaces();
                      }
              else
              {
                Console.WriteLine("NOP");
                 _error = "Problème";
              }
        }
    }
}
