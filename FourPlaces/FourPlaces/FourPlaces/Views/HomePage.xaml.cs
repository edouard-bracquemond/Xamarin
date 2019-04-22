using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms.Xaml;
using FourPlaces.Models;

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

        private async void ListPlaces_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            PlaceItemSummary place = (PlaceItemSummary)e.Item;
            await Navigation.PushAsync(new DetailPage(await Service.GetPlaceService(place.Id)));
        }





    }
}
