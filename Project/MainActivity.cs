using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;
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
        }

       
        private void ButtonClick(object sender, System.EventArgs e)
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
            DBHelper myDB = new DBHelper(this);
            if (!myDB.checkIfExist("USERS", new Dictionary<string, string>{{ "EMAIL", username.Text },{ "PASSWORD", password.Text }}))
            {
                Dialog myDialog = util.CreateButton(this, "Attention", "User name and password incorrect.\nTry again or create a new account.");
                myDialog.Show();
            }
            else
            {
                Intent mainPage = new Intent(this, typeof(MainPage));
                Util.setPref(this, "userLogged", username.Text);
                StartActivity(mainPage);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}