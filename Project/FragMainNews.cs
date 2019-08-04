using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Project.Api;
using Project.Model;
using Project.Adapters;
using Newtonsoft.Json;
using Refit;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;

namespace Project
{
    public class FragMainNews : Fragment
    {
        ListView newsListView;
        SearchView newsSearchView;
        List<News> newsList = new List<News>();
        INewsApi newsApi;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View myView = inflater.Inflate(Resource.Layout.FragMainNewsLayout, container, false);

            newsListView = myView.FindViewById<ListView>(Resource.Id.listViewHomeNews);
            newsSearchView = myView.FindViewById<SearchView>(Resource.Id.searchViewHomeNews);

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

            newsApi = RestService.For<INewsApi>("https://android-lambton-api.herokuapp.com");
            newsSearchView.QueryTextChange += MySearchView_QueryTextChange;
            getNewsAsync();

            return myView;
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
                var myAdapter = new NewsListAdapter(this.Context, newsList);
                newsListView.Adapter = myAdapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Context, ex.StackTrace, ToastLength.Long).Show();
            }
        }
    }
}