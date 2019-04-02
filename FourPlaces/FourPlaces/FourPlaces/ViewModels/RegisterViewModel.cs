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
        private string _error;
        public ICommand RegistrationClicked { get; set; }

        public INavigation navigation { get; set; }

        public RegisterViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            this.RegistrationClicked = new Command(async () => await Registration());
        }

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
                    await navigation.PushAsync(new HomePage());
                }
                else
                {
                    Error = "Echec de l'enregistrement";
                }
            }
        }
    }
}
