using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using AsyncDemo.DataContracts;

namespace AsyncDemo.Clients
{
    public class JsonPlaceholderClient
    {
        private const string BASE_URL = "https://jsonplaceholder.typicode.com";
        private readonly HttpClient _commentsClient;
        private readonly HttpClient _albumsClient;
        private readonly HttpClient _photosClient;

        public JsonPlaceholderClient()
        {
            _commentsClient = new HttpClient();
            _albumsClient = new HttpClient();
            _photosClient = new HttpClient();
        }

        public List<Comment> GetComments()
        {
            //var response = _commentsClient.GetAsync($"{BASE_URL}/comments").Result;
            //var jsonString = response.Content.ReadAsStringAsync().Result;

            //Console.WriteLine("Comments retrieved");
            //return JsonConvert.DeserializeObject<List<Comment>>(jsonString);
            Thread.Sleep(30);
            Console.WriteLine("Comments retrieved");
            return new List<Comment>();
        }

        public async Task<List<Comment>> GetCommentsAsync()
        {
            //var response = await _commentsClient.GetAsync($"{BASE_URL}/comments");
            //var jsonString = await response.Content.ReadAsStringAsync();

            //Console.WriteLine("Comments retrieved");
            //return JsonConvert.DeserializeObject<List<Comment>>(jsonString);
            await Task.Delay(30);
            Console.WriteLine("Comments retrieved");
            return new List<Comment>();
        }

        public List<Album> GetAlbums()
        {
            //var response = _albumsClient.GetAsync($"{BASE_URL}/albums").Result;
            //var jsonString = response.Content.ReadAsStringAsync().Result;

            //Console.WriteLine("Albums retrieved");
            //return JsonConvert.DeserializeObject<List<Album>>(jsonString);
            Thread.Sleep(80);
            Console.WriteLine("Albums retrieved");
            return new List<Album>();
        }

        public async Task<List<Album>> GetAlbumsAsync()
        {
            //var response = await _albumsClient.GetAsync($"{BASE_URL}/albums");
            //var jsonString = await response.Content.ReadAsStringAsync();

            //Console.WriteLine("Albums retrieved");
            //return JsonConvert.DeserializeObject<List<Album>>(jsonString);
            await Task.Delay(80);
            Console.WriteLine("Albums retrieved");
            return new List<Album>();
        }

        public List<Photo> GetPhotos()
        {
            //var response = _photosClient.GetAsync($"{BASE_URL}/photos").Result;
            //var jsonString = response.Content.ReadAsStringAsync().Result;

            //Console.WriteLine("Photos retrieved");
            //return JsonConvert.DeserializeObject<List<Photo>>(jsonString);
            Thread.Sleep(90);
            Console.WriteLine("Photos retrieved");
            return new List<Photo>();
        }

        public async Task<List<Photo>> GetPhotosAsync()
        {
            //var response = await _photosClient.GetAsync($"{BASE_URL}/photos");
            //var jsonString = await response.Content.ReadAsStringAsync();

            //Console.WriteLine("Photos retrieved");
            //return JsonConvert.DeserializeObject<List<Photo>>(jsonString);
            await Task.Delay(90);
            Console.WriteLine("Photos retrieved");
            return new List<Photo>();
        }

    }
}
