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
    class ProfileViewModel : ViewModelBase
    {
        private string _email = "";
        private string _firstName = "";
        private string _lastName = "";
        private string _imageURL;
       // private Image _imageDefault;
        public ICommand ProfileEditingClicked { get; set; }
        public ICommand PasswordEditingClicked { get; set; }
        public INavigation Navigation { get; set; }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
      

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public string ImageURL { get => _imageURL; set => _imageURL = value; }

        public ProfileViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            Email = Barrel.Current.Get<UserItem>(key: "User").Email;
            FirstName = Barrel.Current.Get<UserItem>(key: "User").FirstName;
            LastName = Barrel.Current.Get<UserItem>(key: "User").LastName;
          
            if (Barrel.Current.Get<UserItem>(key: "User").ImageId != 0)
            {
                
                ImageURL = Service.URL + "images/" + Barrel.Current.Get<UserItem>(key: "User").ImageId;

            }
             
           

                 
            
           
            
            
           
            this.ProfileEditingClicked = new Command(async () => await ProfileEditing());
            this.PasswordEditingClicked = new Command(async () => await PasswordEditing());

        }
        

        public async Task ProfileEditing()
        {
            await Navigation.PushAsync(new ProfileEditPage());
        }
        public async Task PasswordEditing()
        {
            await Navigation.PushAsync(new PasswordEditPage());
        }


        public override async Task OnResume()
        {
            await base.OnResume();
            
            FirstName = Barrel.Current.Get<UserItem>(key: "User").FirstName;
            LastName = Barrel.Current.Get<UserItem>(key: "User").LastName;
            Email = Barrel.Current.Get<UserItem>(key: "User").Email;
            if (Barrel.Current.Get<UserItem>(key: "User").ImageId != 0)
            {

                ImageURL = Service.URL + "images/" + Barrel.Current.Get<UserItem>(key: "User").ImageId;
            }
       
        }
    }
}
