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
        SearchView newsSearchView;
        ListView newsListView;
        List<News> newsList = new List<News>();
        INewsApi newsApi;
        View myView;
        Button favNewsBtn;
        Button openNewsBtn;
        Util util = new Util();
        string userLogged;
        ProgressBar progressBarSpinner;
        LinearLayout mainNewsLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            myView = inflater.Inflate(Resource.Layout.FragMainNewsLayout, container, false);
            userLogged = util.getPref(myView.Context, "userLogged");

            newsListView = myView.FindViewById<ListView>(Resource.Id.listViewHomeNews);
            newsSearchView = myView.FindViewById<SearchView>(Resource.Id.searchViewHomeNews);
            progressBarSpinner = myView.FindViewById<ProgressBar>(Resource.Id.progressBar1);
            mainNewsLayout = myView.FindViewById<LinearLayout>(Resource.Id.mainNewsLayout);

            //spinner.Progress

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
                progressBarSpinner.Visibility = ViewStates.Invisible;
                mainNewsLayout.RemoveView(progressBarSpinner);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Context, ex.StackTrace, ToastLength.Long).Show();
            }
        }

        private void favNewsBtnClick(object sender, System.EventArgs e)
        {

            
            TextView title = myView.FindViewById<TextView>(Resource.Id.textViewTitle);
            TextView newsURL = myView.FindViewById<TextView>(Resource.Id.TxtNewsURL);
            TextView imageURL = myView.FindViewById<TextView>(Resource.Id.TxtImageURL);
            
            var favNews = new Dictionary<string, string>
            {
                { "EMAIL", userLogged},
                { "TITLE", title.Text },
                { "NEWSURL", newsURL.Text },
                { "IMAGEURL", imageURL.Text }
            };

            DBHelper myDB = new DBHelper(myView.Context);
            myDB.insertSQL(favNews, "NEWS");
            
        }
    }
}