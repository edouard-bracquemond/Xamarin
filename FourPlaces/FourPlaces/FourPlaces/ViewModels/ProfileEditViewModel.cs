using FourPlaces.Models;
using MonkeyCache.SQLite;
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
    class ProfileEditViewModel : ViewModelBase
    {
      
        private string _firstName;
        private string _lastName;
        private string _imageURL;
        private int? _imageId;
        private string _error="";
        private MediaFile file=null;
        public ICommand PickClicked { get; set; }
        public ICommand TakeClicked { get; set; }
        public ICommand OkClicked { get; set; }
        public INavigation Navigation { get; set; }
        public string ImageURL { get => _imageURL; set => _imageURL = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        
        public int? ImageId { get => _imageId; set => _imageId = value; }
        public string Error { get => _error; set => _error = value; }

        public ProfileEditViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
           
            _firstName = Barrel.Current.Get<UserItem>(key: "User").FirstName;
            _lastName = Barrel.Current.Get<UserItem>(key: "User").LastName;
            _imageId = Barrel.Current.Get<UserItem>(key: "User").ImageId;
            
            if (Barrel.Current.Get<UserItem>(key: "User").ImageId != 0)
            {

                ImageURL = Service.URL + "images/" + Barrel.Current.Get<UserItem>(key: "User").ImageId;
                
            }
         

            this.PickClicked = new Command(async () => await Pick());
            this.TakeClicked = new Command(async () => await Take());
            this.OkClicked = new Command(async () => await Ok());
            
        }
        public async Task Pick()
        {
            file = await Service.PickPhotoService(true);
        }
        public async Task Take()
        {
            file = await Service.TakePhotoService(true);
        }
        private async Task Ok()
        {
            
            if(_firstName != Barrel.Current.Get<UserItem>(key: "User").FirstName||_lastName!= Barrel.Current.Get<UserItem>(key: "User").LastName || file != null)
            {
               
                if (file != null)
                {
                    
                    this.ImageId = await Service.UploadImageService(file);
                }
                if (await Service.UpdateProfileService(FirstName, LastName, ImageId))
                    {
                   
                    await Navigation.PopAsync();
                    }
                    else
                    {
                   
                    Error = "Problème de modification ";
                    }
               
            }
        }
    }
}
