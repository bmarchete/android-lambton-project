using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Project.Model;
using Project.Adapters;
using Android.Database;

namespace Project
{
    public class FragFavNews : Fragment
    {
        SearchView newsSearchView;
        ListView newsListView;
        List<News> newsList = new List<News>();
        View myView;
        ProgressBar progressBarSpinner;
        LinearLayout mainNewsLayout;
        string userLogged;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            myView = inflater.Inflate(Resource.Layout.FragFavNewsLayout, container, false);

            Util.setPref(myView.Context, "currentFragment", this.GetType().Name);
            userLogged = Util.getPref(myView.Context, "userLogged");

            newsListView = myView.FindViewById<ListView>(Resource.Id.listViewHomeNews);
            newsSearchView = myView.FindViewById<SearchView>(Resource.Id.searchViewHomeNews);
            progressBarSpinner = myView.FindViewById<ProgressBar>(Resource.Id.progressBar1);
            mainNewsLayout = myView.FindViewById<LinearLayout>(Resource.Id.mainNewsLayout);

            newsSearchView.QueryTextChange += MySearchView_QueryTextChange;
            newsList.Clear();
            getNewsDB();
            
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
            newsListView.Adapter = new NewsListAdapter(this.Context, searchArray, this);
        }

        private void getNewsDB()
        {
            try
            {
                DBHelper myDB = new DBHelper(myView.Context);
                string[] fields = { "TITLE", "NEWSURL", "IMAGEURL" };
                ICursor result = myDB.selectStm("NEWS", fields, new Dictionary<string, string> { { "EMAIL", userLogged} });
                while (!result.IsAfterLast)
                {
                    var news = new News();
                    news.title = result.GetString(0);
                    news.url = result.GetString(1);
                    news.urlToImage = result.GetString(2);
                    newsList.Add(news);

                    result.MoveToNext();
                }
                newsListView.Adapter = new NewsListAdapter(this.Context, newsList,this);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Context, "No favorites to exhibit", ToastLength.Long).Show();
            }
            progressBarSpinner.Visibility = ViewStates.Invisible;
            mainNewsLayout.RemoveView(progressBarSpinner);
        }
    }
}