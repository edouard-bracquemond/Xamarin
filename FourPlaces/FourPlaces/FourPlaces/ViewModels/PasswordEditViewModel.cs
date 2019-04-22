using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class PasswordEditViewModel : ViewModelBase
    {
        private string _oldPassword="";
        private string _newPassword="";
        private string _error="";
        INavigation Navigation;
        private ICommand okClicked;

        public string OldPassword { get => _oldPassword; set => _oldPassword = value; }
        public string NewPassword { get => _newPassword; set => _newPassword = value; }
        public string Error { get => _error; set => _error = value; }
        public ICommand OkClicked { get => okClicked; set => okClicked = value; }

        public PasswordEditViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.OkClicked = new Command(async () => await Ok());
            
        }
        private async Task Ok()
        {
            if (NewPassword != "")
            {
                if(!await Service.PasswordEditService(OldPassword, NewPassword))
                {
                    Error = "Erreur lors du changement vérifiez le mot de passe renseigné";
                }
                else {
                    await Navigation.PopAsync();
                }
            }
            else
            {
                Error = "Le mot de passe ne peut pas être vide";
            }
        }


    }
}
