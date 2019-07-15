using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Refit;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using Project.Model;
using Project.Api;
using System.Collections.Generic;

namespace Project
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ListView listView;
        ArrayAdapter listAdapter;

        IGitHubApi gitHubApi;
        List<User> users = new List<User>();
        List<string> user_names = new List<string>();
        Button buttonGet;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            listView = FindViewById<ListView>(Resource.Id.listView1);

            List<string> array = new List<string>();
            array.Add("Text1");
            array.Add("Text2");

            listAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, array);

            listView.Adapter = listAdapter;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

            gitHubApi = RestService.For<IGitHubApi>("https://api.github.com");

            buttonGet = FindViewById<Button>(Resource.Id.btn_list_users);
            buttonGet.Click += buttongGetEventAsync;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void buttongGetEventAsync(object sender, EventArgs e)
        {
            getUsersAsync();
        }

        private async Task getUsersAsync()
        {
            try
            {
                ApiResponse response = await gitHubApi.GetUser();
                users = response.items;

                foreach (User user in users)
                {
                    user_names.Add(user.ToString());
                }
                listAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, user_names);
                listView.Adapter = listAdapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.StackTrace, ToastLength.Long).Show();

            }
        }
    }
}