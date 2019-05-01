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
    public class PhotoAlbumsClient
    {
        //This class originally used this api but it wasn't consistent
        //private const string BASE_URL = "https://jsonplaceholder.typicode.com";
        //private readonly HttpClient _client;

        public PhotoAlbumsClient()
        {
            //_client = new HttpClient();
        }

        public List<Comment> GetComments()
        {
            Thread.Sleep(30);
            Console.WriteLine("Comments retrieved");
            return new List<Comment>();
        }

        public async Task<List<Comment>> GetCommentsAsync()
        {
            await Task.Delay(30);
            Console.WriteLine("Comments retrieved");
            return new List<Comment>();
        }

        public List<Album> GetAlbums()
        {
            Thread.Sleep(80);
            Console.WriteLine("Albums retrieved");
            return new List<Album>();
        }

        public async Task<List<Album>> GetAlbumsAsync()
        {
            await Task.Delay(80);
            Console.WriteLine("Albums retrieved");
            return new List<Album>();
        }

        public List<Photo> GetPhotos()
        {
            Thread.Sleep(90);
            Console.WriteLine("Photos retrieved");
            return new List<Photo>();
        }

        public async Task<List<Photo>> GetPhotosAsync()
        {
            await Task.Delay(90);
            Console.WriteLine("Photos retrieved");
            return new List<Photo>();
        }

    }
}
