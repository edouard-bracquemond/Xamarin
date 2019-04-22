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
    class RegisterViewModel : ViewModelBase
    {
        private string _email ="";
        private string _firstName="";
        private string _lastName="";
        private string _password="";
        private int? _imageId = null;
        private string _error;
        public ICommand RegistrationClicked { get; set; }
        public ICommand PickClicked { get; set; }
        public ICommand TakeClicked { get; set; }
        public INavigation Navigation { get; set; }
        private MediaFile file = null;
        public RegisterViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.RegistrationClicked = new Command(async () => await Registration());
            this.PickClicked = new Command(async () => await Pick());
            this.TakeClicked = new Command(async () => await Take());
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public int? ImageId
        {
            get => _imageId;
            set => SetProperty(ref _imageId, value);
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

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }
        public async Task Pick()
        {
            file = await Service.PickPhotoService(true);
        }
        public async Task Take()
        {
            file = await Service.TakePhotoService(true);
        }
        public async Task Registration()
        {
            Error = "";
            if (Password == "" || LastName == "" || FirstName == "" || Email == "")
            {
                Error = "Champs incorrects";
            }
            else{
                if (await Service.RegistrationService(Email, Password, LastName, FirstName))
                {
                    if (file != null)
                    {
                        this.ImageId = await Service.UploadImageService(file);
                        Console.WriteLine(ImageId + "   _____________________________________________________________________________>");
                        if (await Service.UpdateProfileService(FirstName, LastName, ImageId))
                        {
                            await Navigation.PushAsync(new HomePage());
                        }
                        else
                        {
                            Error = "Problème avec l'image ";
                        }
                    }
                    else
                    {
                        await Navigation.PushAsync(new HomePage());
                    }
                }
                else
                {
                    Error = "Echec de l'enregistrement";
                }
            }
        }
    }
}
