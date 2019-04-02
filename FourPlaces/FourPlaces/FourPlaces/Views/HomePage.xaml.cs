using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.Api.Dtos;

using Xamarin.Forms.Xaml;

namespace FourPlaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : BaseContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(Navigation);
        }
      
       
        


        
    }
}
