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
    public class User
    {
        [JsonProperty(PropertyName = "login")]
        public string userName { get; set; }
        public override string ToString()
        {
            return userName;
        }
    }
}