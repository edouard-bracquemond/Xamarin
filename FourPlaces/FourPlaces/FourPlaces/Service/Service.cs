
using FourPlaces.Models;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using Plugin.Media;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using System.Diagnostics;
using System.IO;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;

namespace FourPlaces
{
    public static class Service
    {
        public static string URL = "https://td-api.julienmialon.com/";

       
        public static async Task<bool> SendCommentService(string comment ,int id)
        {

            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL + "places/" + id +"/comments", string.Empty));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                CreateCommentRequest content = new CreateCommentRequest(comment);
                string data = JsonConvert.SerializeObject(content);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        if(await RefreshTokenService())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                            response = await client.PostAsync(uri, contentRequest);
                            if (response.IsSuccessStatusCode)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public static async Task<bool> RegistrationService(string email,string password,string lastname, string firstname)
        {
            
            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL + "auth/register", string.Empty));
                var json = JsonConvert.SerializeObject(new RegisterRequest(email, password, lastname, firstname));
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var reponseContent = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> result = JsonConvert.DeserializeObject<Response<LoginResult>>(reponseContent);
                    Barrel.Current.Add(key: "Login", data: result.Data, expireIn: TimeSpan.FromDays(1));
                    Barrel.Current.Empty(key: "User");
                    
                    return await UserService();

                }


                return false;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }    

        }

        public static async Task<bool> RefreshTokenService()
        {
            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL + "auth/refresh", string.Empty));
                RefreshRequest request = new RefreshRequest(Barrel.Current.Get<LoginResult>(key: "Login").RefreshToken);
                string data = JsonConvert.SerializeObject(request);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> result = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                    Barrel.Current.Add(key: "Login", data: result.Data, expireIn: TimeSpan.FromDays(1));
                    
                    return await UserService();
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        internal static Task<bool> PlaceAddService(string title, string description, int? imageId, double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public static async Task<bool> LoginService(string email, string password)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return true;
            }

            //Dev handles checking if cache is expired
            if (!Barrel.Current.IsExpired(key: "Login"))
            {
               
               return await UserService();
            }

            try {
                        HttpClient client = new HttpClient();
                        var uri = new Uri(string.Format(URL + "auth/login", string.Empty));
                        var json = JsonConvert.SerializeObject(new LoginRequest(email, password));
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = null;
                        response = await client.PostAsync(uri, content);
                        if (response.IsSuccessStatusCode)
                        {
                            var reponseContent = await response.Content.ReadAsStringAsync();
                            Response<LoginResult> result = JsonConvert.DeserializeObject<Response<LoginResult>>(reponseContent);
                            Barrel.Current.Add(key: "Login", data: result.Data, expireIn: TimeSpan.FromDays(1));
                            return await UserService();

                        }
                        return false;
                    } catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return false;
                    }
            
               
            
        }

        public static async Task<bool> UserService()
        {
            
            if (!CrossConnectivity.Current.IsConnected)
            {
                
                return true;
                
            }

            //Dev handles checking if cache is expired
            if (!Barrel.Current.IsExpired(key: "User"))
            {
                
                return true;
            }
            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL + "me", string.Empty));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    
                    var content = await response.Content.ReadAsStringAsync();
                    Response<UserItem> result = JsonConvert.DeserializeObject<Response<UserItem>>(content);
                    Barrel.Current.Add(key: "User", data: result.Data, expireIn: TimeSpan.FromDays(1));
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (await RefreshTokenService())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                        response = await client.GetAsync(uri);



                        if (response.IsSuccessStatusCode)
                        {

                            var content = await response.Content.ReadAsStringAsync();
                            Response<UserItem> result = JsonConvert.DeserializeObject<Response<UserItem>>(content);
                            Barrel.Current.Add(key: "User", data: result.Data, expireIn: TimeSpan.FromDays(1));
                            

                            return true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return false;
            }
            return false;


        }
        public static async Task<MediaFile> TakePhotoService(bool size)
        {
          
            await CrossMedia.Current.Initialize();
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {

               // await CrossMedia.Current.Initialize();
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    // Supply media options for saving our photo after it's taken.
                  
                    if (size)
                    {
                        var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                           // Directory = "Receipts",
                            //Name = $"{DateTime.UtcNow}.jpg",
                            PhotoSize = PhotoSize.Small,
                            SaveToAlbum = false
                        };
                        return await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                    }
                     else
                     {
                        var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            //    Directory = "Receipts",
                            //  Name = $"{DateTime.UtcNow}.jpg",
                            SaveToAlbum = false

                        };
                         return await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                    }
               


                }
               
                return null;
            }
            
            return null; 
            }
        public static async Task<MediaFile> PickPhotoService(bool size)
        {
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsPickPhotoSupported) {
                if (size)
                {
                    var mediaOptions = new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small

                    };
                    return await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                }
                else
                {
                    
                    return await CrossMedia.Current.PickPhotoAsync();
                }
                
              
                
            }
            return null;
        }
        public static async Task<bool> PlaceAddService(string title, string description, int imageId, double latitude, double longitude)
        {
            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL + "places", string.Empty));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                var json = JsonConvert.SerializeObject(new CreatePlaceRequest(title, description, imageId, latitude,longitude));
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    

                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (await RefreshTokenService())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                        response = await client.PostAsync(uri,content);



                        if (response.IsSuccessStatusCode)
                        {

                            

                            return true;
                        }
                    }
                }

                
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return false;
        }
         


        public static async Task<int> UploadImageService(MediaFile file)
        {
            HttpClient client = new HttpClient();
            byte[] imageData = File.ReadAllBytes(file.Path);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/images");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);

            MultipartFormDataContent requestContent = new MultipartFormDataContent();

            var imageContent = new ByteArrayContent(imageData);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            // Le deuxième paramètre doit absolument être "file" ici sinon ça ne fonctionnera pas
            requestContent.Add(imageContent, "file", "file.jpg");

            request.Content = requestContent;

            HttpResponseMessage response = await client.SendAsync(request);

            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                return res.Data.Id;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if(await RefreshTokenService())
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                    response = await client.SendAsync(request);
                    result = await response.Content.ReadAsStringAsync();

                
                    if (response.IsSuccessStatusCode)
                    {
                        Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                        return res.Data.Id;
                    }
                }
            }
            return 0;


        }
        public static async Task<bool> UpdateProfileService(string firstname, string lastname, int? imageId)
        {
            HttpClient client = new HttpClient();
            
            var uri = new Uri(string.Format(URL + "me", string.Empty));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
            UpdateProfileRequest request = new UpdateProfileRequest(firstname, lastname, imageId);
            string data = JsonConvert.SerializeObject(request);
            var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
            requestMessage.Content = contentRequest;

            var response = await client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                
                Barrel.Current.Empty(key: "User");
                return await UserService();
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    
                    if (await RefreshTokenService())
                    {
                       
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                        response = await client.SendAsync(requestMessage);
                        if (response.IsSuccessStatusCode)
                        {
                            Barrel.Current.Empty(key: "User");
                            return await UserService();
                        }
                    }
                }
            }
            return false;

        }
        public static async Task<MediaFile> GetImageService(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL + "images/"+id, string.Empty));
               
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    Response<MediaFile> result = JsonConvert.DeserializeObject<Response<MediaFile>>(content);
                    
                    return result.Data;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }
           
        }
         public static async Task<bool> PasswordEditService(string oldP, string newP)
        {
            HttpClient client = new HttpClient();

            var uri = new Uri(string.Format(URL + "me/password", string.Empty));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
            UpdatePasswordRequest request = new UpdatePasswordRequest(oldP,newP);
            string data = JsonConvert.SerializeObject(request);
            var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
            HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
            requestMessage.Content = contentRequest;

            var response = await client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {

               
                return true;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {

                    if (await RefreshTokenService())
                    {
                        
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<LoginResult>(key: "Login").AccessToken);
                        response = await client.SendAsync(requestMessage);
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }
        public static async Task<Position> GetCurrentLocation()
        {
          
                Position position = null;
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 100;

                    position = await locator.GetLastKnownLocationAsync();

                    if (position != null)
                    {
                        //got a cahched position, so let's use it.
                        return position;
                    }

                    if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                    {
                        //not available or enabled
                        return null;
                    }

                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to get location: " + ex);
                }

                if (position == null)
                    return null;

                var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                        position.Timestamp, position.Latitude, position.Longitude,
                        position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

                Debug.WriteLine(output);

                return position;
          
        }
        public static async Task<ListPlaces> GetPlacesService()
        {
                 
            if (!CrossConnectivity.Current.IsConnected && !Barrel.Current.IsExpired(key: "Places"))
            {
                return Barrel.Current.Get<ListPlaces>(key: "Places");
            }
            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL+"places", string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Response<List<PlaceItemSummary>> res = JsonConvert.DeserializeObject<Response<List<PlaceItemSummary>>>(content);
                    ListPlaces places = new ListPlaces(res.Data);
                    Barrel.Current.Add(key: "Places", data: places, expireIn: TimeSpan.FromDays(1));
                    return places;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public static async Task<PlaceItem> GetPlaceService(int id)
        {

            if (!CrossConnectivity.Current.IsConnected && !Barrel.Current.IsExpired(key: "Place"+id))
            {
                return Barrel.Current.Get<PlaceItem>(key: "Place"+id);
            }
            try
            {
                HttpClient client = new HttpClient();
                var uri = new Uri(string.Format(URL + "places/"+id, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Response<PlaceItem> res = JsonConvert.DeserializeObject<Response<PlaceItem>>(content);
                    
                    Barrel.Current.Add(key: "Places"+id, data: res.Data, expireIn: TimeSpan.FromDays(1));
                    return res.Data;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}
