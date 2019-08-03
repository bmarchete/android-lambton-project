using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Refit;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using Project.Model;
using Project.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;


namespace Project
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        EditText username;
        EditText password;
        Button login;
        Button signin;
        Util util = new Util();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            username = FindViewById<EditText>(Resource.Id.UserName);
            password = FindViewById<EditText>(Resource.Id.Password);
            login = FindViewById<Button>(Resource.Id.Login);
            signin = FindViewById<Button>(Resource.Id.SignIn);

            login.Click += ButtonClick;
            signin.Click += delegate
            {
                Intent registerPage = new Intent(this, typeof(Register));
                StartActivity(registerPage);
            };

            //buttonGet = FindViewById<Button>(Resource.Id.btn_list_users);
            //buttonGet.Click += buttongGetEventAsync;
        }

        void ButtonClick(object sender, System.EventArgs e)
        {
            Dialog myDialog;
            ViewGroup form = (ViewGroup)FindViewById(Resource.Id.LoginForm);
            if (!util.ValidateTextFields(form))
            {
                myDialog = util.CreateButton(this, "Attention", "Please enter your user name or password");
                myDialog.Show();
            }
            else
            {
                logUser();
            }
        }
        protected void logUser()
        {
            EditText username = FindViewById<EditText>(Resource.Id.UserName);
            EditText password = FindViewById<EditText>(Resource.Id.Password);

            DBHelper myDB = new DBHelper(this);
            if (!myDB.checkLogin(username.Text, password.Text))
            {
                Dialog myDialog = util.CreateButton(this, "Attention", "User name and password incorrect.\nTry again or create a new account.");
                myDialog.Show();
            }
            else
            {
                Intent welcomePage = new Intent(this, typeof(Home));
                welcomePage.PutExtra("userName", username.Text);
                StartActivity(welcomePage);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //private void buttongGetEventAsync(object sender, EventArgs e)
        //{
        //    Intent home = new Intent(this, typeof(Home));
        //    StartActivity(home);
            
        //}
        
    }
}