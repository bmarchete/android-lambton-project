using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Refit;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using Project.Model;
using Project.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ListView listView;
        ArrayAdapter listAdapter;

       
        INewsApi newsApi;
       
        List<News> news = new List<News>();

        List<string> user_names = new List<string>();
        List<string> news_title = new List<string>();

        Button buttonGet;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            listView = FindViewById<ListView>(Resource.Id.listView1);

            List<string> array = new List<string>();
            array.Add("Text1");
            array.Add("Text2");

            listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, array);

            listView.Adapter = listAdapter;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

            newsApi = RestService.For<INewsApi>("https://android-lambton-api.herokuapp.com");

            buttonGet = FindViewById<Button>(Resource.Id.btn_list_users);
            buttonGet.Click += buttongGetEventAsync;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void buttongGetEventAsync(object sender, EventArgs e)
        {
            
            getNewsAsync();

        }

        
        private async Task getNewsAsync()
        {
            try
            {
                ApiNewsResponse response = await newsApi.GetNews();
                news = response.articles;

                foreach (News newsItem in news)
                {
                    news_title.Add(newsItem.ToString());
                }
                listAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, news_title);
                listView.Adapter = listAdapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.StackTrace, ToastLength.Long).Show();

            }
        }
    }
}