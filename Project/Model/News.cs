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
using Newtonsoft.Json;

namespace Project.Model
{
    public class News
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }

        [JsonProperty(PropertyName = "urlToImage")]
        public string urlToImage { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string url { get; set; }

        public override string ToString()
        {
            return title;
        }
    }
}