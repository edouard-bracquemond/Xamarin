using MonkeyCache.SQLite;
using Plugin.Geolocator.Abstractions;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class PlaceAddViewModel : ViewModelBase
    {
        private string _title="";
        private string _description="";
        private int _imageId=0;
        private double _latitude=0;
        private double _longitude = 0;
        private string _error = "";
        private MediaFile file = null;
        public INavigation Navigation { get; set; }
        public ICommand PickClicked { get; set; }
        public ICommand TakeClicked { get; set; }
        public ICommand OkClicked { get; set; }
        public string Title { get => _title; set => _title = value; }
        public string Description { get => _description; set => _description = value; }
        public int ImageId { get => _imageId; set => _imageId = value; }
        public double Longitude { get => _longitude; set => _longitude = value; }
        public double Latitude { get => _latitude; set => _latitude = value; }
        public string Error { get => _error; set => _error = value; }

     
        public PlaceAddViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            
            this.PickClicked = new Command(async () => await Pick());
            this.TakeClicked = new Command(async () => await Take());
            this.OkClicked = new Command(async () => await Ok());
            
            _latitude = Barrel.Current.Get<Plugin.Geolocator.Abstractions.Position>(key: "Position").Latitude;
            _longitude = Barrel.Current.Get<Plugin.Geolocator.Abstractions.Position>(key: "Position").Longitude;
        }
        public async Task Pick()
        {
            file = await Service.PickPhotoService(false);
        }
        public async Task Take()
        {
            file = await Service.TakePhotoService(false);
        }
        private async Task Ok()
        {
           
           
            if (_title != "" && _description != "" && file != null && _longitude!=0 && _latitude!=0 )
                {


            
                    _imageId = await Service.UploadImageService(file);
                Console.WriteLine(_imageId);

                try
                {
                    if (await Service.PlaceAddService(_title, _description, _imageId, _latitude, _longitude))
                    {
                        Console.WriteLine("TOTO");


                        await Navigation.PopAsync();




                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }






            }
            else
            {
               
                Error = "Problème, vérifiez les champs";
            }

        }
           
        }


    
}
