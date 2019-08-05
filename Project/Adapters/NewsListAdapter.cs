using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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

        public NewsListAdapter(Context context, List<News> userList)
        {
            this.context = context;
            this.newsList = userList;
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

                holder = new NewsListAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

                //replace with your item and your holder items
                //comment back in
                view = inflater.Inflate(Resource.Layout.ListMainNews, parent, false);
                //holder.Code = view.FindViewById<TextView>(Resource.Id.textViewCourseCode);
                //holder.Name = view.FindViewById<TextView>(Resource.Id.textViewCourseName);
                //holder.Thumbnail = view.FindViewById<ImageView>(Resource.Id.imageViewCourse);


                //holder.Code.Text = course.Code;
                //holder.Name.Text = course.Name;
                //holder.Thumbnail.SetImageResource(course.Thumbnail);

                holder.Title = view.FindViewById<TextView>(Resource.Id.txtTitle);
                //holder.NewsURL = view.FindViewById<TextView>(Resource.Id.TxtNewsURL);
                holder.ImageURL = view.FindViewById<TextView>(Resource.Id.TxtImageURL);
                holder.BtnFavNews = view.FindViewById<Button>(Resource.Id.BtnFavNews);
                holder.BtnOpenNews = view.FindViewById<Button>(Resource.Id.BtnOpenNews);
                //holder.Description = view.FindViewById<TextView>(Resource.Id.textViewDescription);
                //holder.Description = view.FindViewById<TextView>(Resource.Id.textViewMore);
                holder.Thumbnail = view.FindViewById<ImageView>(Resource.Id.imageView);


                holder.Title.Text = news.title;
                holder.NewsURL = news.url;
                holder.ImageURL.Text = news.urlToImage;
                holder.BtnFavNews.Click += (sender, args) => {
                    holder.addToFavorite();
                };
                holder.BtnOpenNews.Click += (sender, args) => {
                    holder.openWebVIew(this.context);
                };

                //holder.Description.Text = news.description;
                //holder.Description.Text = news.description;

                var imageBitmap = GetImageBitmapFromUrl(news.urlToImage);
                holder.Thumbnail.SetImageBitmap(imageBitmap);
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

        //Fill in cound here, currently 0
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

        public TextView Title { get; set; }
        public string NewsURL { get; set; }
        public TextView ImageURL { get; set; }
        public TextView FirstName { get; set; }
        public TextView Description { get; set; }
        public ImageView Thumbnail { get; set; }
        public Button BtnFavNews { get; set; }
        public Button BtnOpenNews { get; set; }


        public void addToFavorite()
        {
            // Here you can create a new DB intance and save the info 

            Toast.MakeText(Application.Context, "title: " + Title.Text, ToastLength.Short).Show();
        }

        public void openWebVIew(Context context)
        {
            Intent webView = new Intent(context, typeof(WebViewActivity));
            webView.PutExtra("url", NewsURL);
            context.StartActivity(webView);
        }
    }
}