using System;
using System.Collections.Generic;
using System.Net;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Project.Model;

namespace Project.Adapters
{
    public class NewsListAdapter : BaseAdapter<News>
    {
        Context context;
        List<News> newsList;
        Fragment fragment;

        public NewsListAdapter(Context context, List<News> userList)
        {
            this.context = context;
            this.newsList = userList;
        }

        public NewsListAdapter(Context context, List<News> userList, Fragment fragment)
        {
            this.context = context;
            this.newsList = userList;
            this.fragment = fragment;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            NewsListAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as NewsListAdapterViewHolder;

            if (holder == null)
            {
               

                News news = newsList[position];

                holder = new NewsListAdapterViewHolder(context);
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

                if(Util.getPref(this.context, "currentFragment").Equals("FragMainNews"))
                {
                    view = inflater.Inflate(Resource.Layout.ListMainNews, parent, false);
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.ListFavNews, parent, false);
                }

                holder.Title = view.FindViewById<TextView>(Resource.Id.txtTitle);
                holder.ImageURL = view.FindViewById<TextView>(Resource.Id.TxtImageURL);
                holder.BtnFavNews = view.FindViewById<Button>(Resource.Id.BtnFavNews);
                holder.BtnOpenNews = view.FindViewById<Button>(Resource.Id.BtnOpenNews);
                holder.Thumbnail = view.FindViewById<ImageView>(Resource.Id.imageView);
                //holder.Description = view.FindViewById<TextView>(Resource.Id.textViewDescription);
                //holder.NewsURL = view.FindViewById<TextView>(Resource.Id.TxtNewsURL);

                holder.Title.Text = news.title;
                holder.NewsURL = news.url;
                holder.ImageURL.Text = news.urlToImage;
                //holder.Description.Text = news.description;
                holder.BtnFavNews.Click += (sender, args) => {
                    holder.btnFavorite();
                };
                holder.BtnOpenNews.Click += (sender, args) => {
                    holder.openWebVIew(this.context);
                };
                if (!string.IsNullOrWhiteSpace(news.urlToImage))
                {
                    var imageBitmap = GetImageBitmapFromUrl(news.urlToImage);
                    holder.Thumbnail.SetImageBitmap(imageBitmap);
                }
                holder.fragment = fragment;

                view.Tag = holder;
            }
            return view;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }

        public override int Count
        {
            get
            {
                return this.newsList.Count;
            }
        }

        public override News this[int position]
        {
            get
            {
                return newsList[position];
            }
        }
    }

    class NewsListAdapterViewHolder : Java.Lang.Object
    {
        private Context context;

        public NewsListAdapterViewHolder(Context context)
        {
            this.context = context;
        }

        public TextView Title { get; set; }
        public string NewsURL { get; set; }
        public TextView ImageURL { get; set; }
        public TextView FirstName { get; set; }
        public TextView Description { get; set; }
        public ImageView Thumbnail { get; set; }
        public Button BtnFavNews { get; set; }
        public Button BtnOpenNews { get; set; }
        public Fragment fragment { get; set; }

        [Obsolete]
        public void btnFavorite()
        {
            DBHelper myDB = new DBHelper(this.context);
            if(Util.getPref(this.context, "currentFragment").Equals("FragFavNews"))
            {
                myDB.deleteSQL("NEWS", new Dictionary<string, string>{{ "EMAIL", Util.getPref(context, "userLogged") },{ "NEWSURL", NewsURL }});
                Toast.MakeText(Application.Context, "News deleted!", ToastLength.Short).Show();

                if (Build.VERSION.SdkInt >= Build.VERSION_CODES.N)
                {
                    fragment.FragmentManager.BeginTransaction().Detach(fragment).CommitNow();
                    fragment.FragmentManager.BeginTransaction().Attach(fragment).CommitNow();
                }
                else
                {
                    fragment.FragmentManager.BeginTransaction().Detach(fragment).Attach(fragment).Commit();
                }
            }
            else
            {
                if (myDB.checkIfExist("NEWS", new Dictionary<string, string> { { "EMAIL", Util.getPref(context, "userLogged") }, { "NEWSURL", NewsURL } }))
                {
                    Toast.MakeText(Application.Context, "News already saved!", ToastLength.Short).Show();
                }
                else
                {
                    var newsInfo = new Dictionary<string, string>
                    {
                        { "EMAIL", Util.getPref(context, "userLogged")},
                        { "TITLE",  Title.Text.Replace("'","") },
                        { "NEWSURL", NewsURL },
                        { "IMAGEURL", ImageURL.Text }
                    };
                    myDB.insertSQL(newsInfo, "NEWS");
                    Toast.MakeText(Application.Context, "News saved!", ToastLength.Short).Show();
                }
            }
        }

        public void openWebVIew(Context context)
        {
            Intent webView = new Intent(context, typeof(WebViewActivity));
            webView.PutExtra("url", NewsURL);
            context.StartActivity(webView);
        }
    }
}