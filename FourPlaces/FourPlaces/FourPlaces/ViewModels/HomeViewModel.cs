using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        INavigation navigation;
        private List<PlaceItemSummary> _places;
        
        public List<PlaceItemSummary> Places
        {
            get => _places;
            set => SetProperty(ref _places, value);
        }
        public HomeViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            
        }

      
    }
}
