using System;
using System.Collections.Generic;
using System.Text;
using TD.Api.Dtos;

namespace FourPlaces.Models
{

    class Places
    {
        private List<PlaceItemSummary> places { get; set; }
        public Places(List<PlaceItemSummary> places)
        {
            this.places = places;
        }
    }
}
