using System;
using System.Collections.Generic;
using System.Text;


namespace FourPlaces.Models
{

    public class ListPlaces
    {
        private List<PlaceItemSummary> places;

        public List<PlaceItemSummary> GetPlaces()
        {
            return places;
        }

        public void SetPlaces(List<PlaceItemSummary> value)
        {
            places = value;
        }

        public ListPlaces(List<PlaceItemSummary> places)
        {
            this.SetPlaces(places);
        }
        public void SortPlaces()
        {
            foreach (PlaceItemSummary place in places)
            {
                place.RelativePosition();
            }
            places.Sort();
        }
       
    }
}
