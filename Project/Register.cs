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

namespace Project
{
    [Activity(Label = "Register")]
    public class Register : Activity
    {
        Util util = new Util();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.registerLayout);

            Button login = FindViewById<Button>(Resource.Id.BtnLogin);
            Button signin = FindViewById<Button>(Resource.Id.BtnSignIn);

            signin.Click += ButtonClick;
            login.Click += delegate
            {
                Intent mainPage = new Intent(this, typeof(MainActivity));
                StartActivity(mainPage);
            };
        }

        void ButtonClick(object sender, System.EventArgs e)
        {
            Dialog myDialog;
            ViewGroup form = (ViewGroup)FindViewById(Resource.Id.SigninForm);

            if (!util.ValidateTextFields(form))
            {
                myDialog = util.CreateButton(this, "Attention", "All fields must be filled");
                myDialog.Show();
            }
            else if(!Util.validateEmail(FindViewById<EditText>(Resource.Id.TxtEmail).Text))
            {
                myDialog = util.CreateButton(this, "Attention", "Invalid email format");
                myDialog.Show();
            }
            else if(!Util.strIsPositiveNumber(FindViewById<EditText>(Resource.Id.TxtAge).Text))
            {
                myDialog = util.CreateButton(this, "Attention", "Age accept only positive numbers");
                myDialog.Show();
            }
            else
            {
                if (regUser())
                {
                    Toast.MakeText(this, "User registered", ToastLength.Long).Show();
                    Intent mainPage = new Intent(this, typeof(MainActivity));
                    StartActivity(mainPage);
                }
            }
        }

        protected bool regUser()
        {
            EditText email = FindViewById<EditText>(Resource.Id.TxtEmail);
            EditText password = FindViewById<EditText>(Resource.Id.TxtPassword);
            EditText name = FindViewById<EditText>(Resource.Id.TxtName);
            EditText age = FindViewById<EditText>(Resource.Id.TxtAge);
            EditText phone = FindViewById<EditText>(Resource.Id.TxtPhone);

            DBHelper myDB = new DBHelper(this);
            if(myDB.checkIfExist("USERS", new Dictionary<string, string> { { "EMAIL", email.Text } }))
            {
                Dialog myDialog = util.CreateButton(this, "Attention", "Email already registered.\nDo login or try another email");
                myDialog.Show();

                return false;
            }
            else
            {
                var userInfo = new Dictionary<string, string>
                {
                    { "EMAIL", email.Text },
                    { "PASSWORD", password.Text },
                    { "NAME", name.Text },
                    { "AGE", age.Text },
                    { "PHONE", phone.Text }
                };
                myDB.insertSQL(userInfo, "USERS");

                return true;
            }
        }
    }
}