using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SampleFirebase.Extensions;
using SampleFirebase.Models;

namespace SampleFirebase.Controllers
{
    public class HomeController : Controller
    {
        // config init firebase
        private readonly string apiKey = "your api key";
        private readonly string App_secret = "your key secret";
        private readonly string authDomain = "<your project>.firebaseapp.com";
        private readonly string databaseURL = "https://<your project>.firebaseio.com";
        private readonly string projectId = "<your project id>";
        private readonly string storageBucket = "<your project>.appspot.com";
        private readonly string messagingSenderId = "<your message sender id>";

        private readonly HttpClient client = new HttpClient();
        private readonly string API_TICKER_24h = "https://api.binance.com/api/v1/ticker/24hr?symbol=BTCUSDT";

        // View Read From Database
        public async Task<IActionResult> Index()
        {
            var firebase = new FirebaseClient(databaseURL, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(App_secret)
            });

            // query from database
            var dinos = await firebase
              .Child("fir-work811")
              .OrderByKey()
              .OnceAsync<TaskModel>();

            return View(dinos);
        }

        // Read From API then post async to Firebase databsae
        public async Task<IActionResult> ReadFromApiAsync(string url)
        {
            if (string.IsNullOrEmpty(url)) url = API_TICKER_24h;
            // auth firebase
            var firebase = new FirebaseClient(databaseURL, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(App_secret)
            });

            // read data from api
            InitClient(url);

            var response = await client.GetAsJsonAsync(url);

            if (!response.IsSuccessStatusCode) return Json(Task.FromResult(0));

            string stringResult = await response.Content.ReadAsStringAsync();
            var taskModel = JsonConvert.DeserializeObject<TaskModel>(stringResult);
            DisposeClient();

            // Post data to database
            var dino = await firebase.Child("fir-work811").PostAsync(taskModel);
            return Json(dino);
        }

        [HttpPost]
        public async Task<IActionResult> InsertDataAsync(TaskModel taskModel)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            // auth firebase
            var firebase = new FirebaseClient(databaseURL, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(App_secret)
            });

            // Post data to database
            var dino = await firebase.Child("fir-work811").PostAsync(taskModel);
            return RedirectToAction("Index");
        }

        private void InitClient(string url)
        {
            client.BaseAddress = new System.Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void DisposeClient()
        {
            if (client != null) client.Dispose();
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
