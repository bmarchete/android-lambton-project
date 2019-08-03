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

            Button login = FindViewById<Button>(Resource.Id.Login);
            Button signin = FindViewById<Button>(Resource.Id.SignIn);

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
            //else if (!validatePassword())
            //{
            //    myDialog = util.CreateButton(this, "Attention", "Passwords don't match");
            //    myDialog.Show();
            //}
            else
            {
                if (regUser())
                {
                    myDialog = util.CreateButton(this, "Confirmation", "Thank you!\nYou've been registered");
                    Intent mainPage = new Intent(this, typeof(MainActivity));
                    StartActivity(mainPage);
                }
            }

        }

        protected bool regUser()
        {
            EditText email = FindViewById<EditText>(Resource.Id.Email);
            EditText password = FindViewById<EditText>(Resource.Id.Password);
            EditText name = FindViewById<EditText>(Resource.Id.Name);
            EditText age = FindViewById<EditText>(Resource.Id.Age);
            EditText phone = FindViewById<EditText>(Resource.Id.Phone);

            DBHelper myDB = new DBHelper(this);
            if (myDB.checkEmailIDExisit(email.Text))
            {
                Dialog myDialog = util.CreateButton(this, "Attention", "Email already registered.\nDo login or try another email");
                myDialog.Show();
                return false;
            }
            else
            {
                myDB.insertUser(email.Text, password.Text, name.Text, age.Text, phone.Text);
                return true;
            }
        }
    }
}