﻿using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourPlaces
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : BaseContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            BindingContext = new LoginViewModel(Navigation);

        }
       
    }
}