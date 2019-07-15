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
using Project.Model;

namespace Project.Api
{
    public class ApiResponse
    {
        [JsonProperty(PropertyName = "total_count")]
        public string totalCount { get; set; }
        [JsonProperty(PropertyName = "incomplete_results")]
        public string incompleteResults { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<User> items { get; set; }
        public override string ToString()
        {
            return totalCount;
        }
    }
}