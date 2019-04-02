using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace FourPlaces.ViewModels
{
    class DetailViewModel : ViewModelBase
    {
        public INavigation navigation { get; set; }
        private PlaceItemSummary place;
        public DetailViewModel(PlaceItemSummary place)
        {
            this.place = place;
        }
    }
}
