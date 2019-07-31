using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Project.Api;
using Project.Model;

using Refit;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Project.Adapters;

namespace Project
{
    [Activity(Label = "Home")]
    public class Home : Activity
    {
        ListView newsListView;

        SearchView newsSearchView;
        INewsApi newsApi;

        List<News> newsList = new List<News>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.home);

            newsListView = FindViewById<ListView>(Resource.Id.listViewHomeNews);
            newsSearchView = FindViewById<SearchView>(Resource.Id.searchViewHomeNews);

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

            newsApi = RestService.For<INewsApi>("https://android-lambton-api.herokuapp.com");

            //dumb list
            //newsList.Add(new News() { title = "Title", description = "Description" });

            //var myAdapter = new NewsListAdapter(this, newsList);

            //newsListView.Adapter = myAdapter;

            newsSearchView.QueryTextChange += MySearchView_QueryTextChange;


            getNewsAsync();



        }


        private void MySearchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {

           
        }


        private async Task getNewsAsync()
        {
            try
            {
                ApiNewsResponse response = await newsApi.GetNews();
                newsList = response.articles;

                var myAdapter = new NewsListAdapter(this, newsList);

                newsListView.Adapter = myAdapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.StackTrace, ToastLength.Long).Show();

            }
        }
    }
}