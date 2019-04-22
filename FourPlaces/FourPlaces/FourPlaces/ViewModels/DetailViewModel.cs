using FourPlaces.Models;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FourPlaces.ViewModels
{
    class DetailViewModel : ViewModelBase
    {
        public INavigation Navigation { get; set; }
        public string Title { get => _title; set => _title = value; }
        public string Description { get => _description; set => _description = value; }
        public double Longitude { get => _longitude; set => _longitude = value; }
        public double Latitude { get => _latitude; set => _latitude = value; }
        public string ImageURL { get => _imageURL; set => _imageURL = value; }
   
        private List<CommentItem> _comments;
        private string _imageURL;
        private Map _map;
        private string _title;
        private string _description;
        private double _longitude;
        private double _latitude;
        
        private PlaceItem place;
        private string _error;
        private string _newComment;

        public ICommand SendCommentClicked { set; get; }
        public string NewComment { get => _newComment; set => _newComment = value; }
        public List<CommentItem> Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        public string Error { get => _error; set => _error = value; }
        public Map Map { get => _map; set => _map = value; }

        private int _id;
       
        Position position;
        public DetailViewModel(PlaceItem placeS)
        {

            place = placeS;
            _id = placeS.Id;
            _title = placeS.Title;
            _description = placeS.Description;
            _longitude = placeS.Longitude;
            _latitude = placeS.Latitude;
            _imageURL = Service.URL + "images/" + placeS.ImageId;
            Comments = new List<CommentItem>(placeS.Comments);
            foreach (CommentItem c in Comments)
            {
               c.ImageURL = Service.URL + "images/" + c.Author.ImageId;

            }

            position = new Position(Latitude, Longitude);
            Map = new Map();
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(position,Distance.FromKilometers(150)));
           
            var pin = new Pin
            {
                Position = position,
                Label = Title
            };
            Map.Pins.Add(pin);
            SendCommentClicked = new Command(async () => { await SendComment(); });

        }
        
        public override async Task OnResume()
        {
            await base.OnResume();
            place = await Service.GetPlaceService(_id);
            Comments = place.Comments;
            foreach (CommentItem c in Comments)
            {
                c.ImageURL = Service.URL + "images/" + c.Author.ImageId;

            }
            position = new Position(Latitude, Longitude);
            var pin = new Pin
            {
                Position = position,
                Label = Title
            };
            Map.Pins.Add(pin);
        }
        private async Task SendComment()
        {
            if (NewComment != "" && NewComment != null)
            {
                if (await Service.SendCommentService(NewComment, _id))
                {
                    NewComment = "";
                    await this.OnResume();
                }
                else
                {
                    Error = "impossible de commenter";
                }
            }
            else
            {
                Error = "vérifiez le champs";
            }
        }
    }
}
