﻿using MonkeyCache.SQLite;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class LoginViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand ConnectionClicked{ set; get; }
        public ICommand RegisterClicked { set; get; }
        private string _password="";
        private string _email="";
        private string _error;
        INavigation Navigation { get; set; }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }
        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.ConnectionClicked = new Command(async () => await GotoHome());
            this.RegisterClicked = new Command(async () => await GotoRegister());
        }
        public async Task GotoRegister()
        {
            await Navigation.PushAsync(new RegisterPage());
        }
        public async Task GotoHome()
        {
            Error = "";
            if (Barrel.Current.Exists(key: "Login")&&Email!=""&&Password!="")
            {
                
                Barrel.Current.Empty(key: "Login");
                Barrel.Current.Empty(key: "User");
            }
            if (await Service.LoginService(Email, Password))
            {
                await Navigation.PushAsync(new HomePage());
            }
            else
            {
                Error = "Problème de connexion";
            }
        }
    }
}
