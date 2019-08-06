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

            Util.setPref(myView.Context, "currentFragment", this.GetType().Name);
            userLogged = Util.getPref(myView.Context, "userLogged");

            newsListView = myView.FindViewById<ListView>(Resource.Id.listViewHomeNews);
            newsSearchView = myView.FindViewById<SearchView>(Resource.Id.searchViewHomeNews);
            progressBarSpinner = myView.FindViewById<ProgressBar>(Resource.Id.progressBar1);
            mainNewsLayout = myView.FindViewById<LinearLayout>(Resource.Id.mainNewsLayout);

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

            newsApi = RestService.For<INewsApi>("https://android-lambton-api.herokuapp.com");
            newsSearchView.QueryTextChange += MySearchView_QueryTextChange;
            newsList.Clear();
            getNewsAsync();

            return myView;
        }
        private void MySearchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            string searchedValue = e.NewText;
            List<News> searchArray = new List<News>();
            foreach (var item in newsList)
            {
                if (item.title.ToUpper().Contains(searchedValue.ToUpper()))
                    searchArray.Add(item);

            }
            newsListView.Adapter = new NewsListAdapter(this.Context, searchArray);
        }
        private async Task getNewsAsync()
        {
            try
            {
                ApiNewsResponse response = await newsApi.GetNews();
                newsList = response.articles;
                newsListView.Adapter = new NewsListAdapter(this.Context, newsList);
                progressBarSpinner.Visibility = ViewStates.Invisible;
                mainNewsLayout.RemoveView(progressBarSpinner);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Context, ex.StackTrace, ToastLength.Long).Show();
            }
        }
    }
}