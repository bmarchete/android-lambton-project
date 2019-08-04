using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Project
{
    public class FragUserInfo : Fragment
    {
        string userLogged = "a";
        bool updateDisplay = false;
        Button updateButton;
        View myView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //userLogged = savedInstanceState.GetString();
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            myView = inflater.Inflate(Resource.Layout.FragUserInfoLayout, container, false);
            //userLogged = 

            userInfo();

            updateButton = myView.FindViewById<Button>(Resource.Id.Update);
            updateButton.Click += ButtonClick;

            return myView;
        }

        private void ButtonClick(object sender, System.EventArgs e)
        {
            if (!updateDisplay)
                changeVisibility(true);
            else
            {
                Util util = new Util();
                Dialog myDialog;
                ViewGroup form = (ViewGroup)myView.FindViewById(Resource.Id.UserForm);

                if (!util.ValidateTextFields(form))
                {
                    myDialog = util.CreateButton(myView.Context, "Attention", "All fields must be filled");
                    myDialog.Show();
                }
                //else if (!validatePassword())
                //{
                //    myDialog = util.CreateButton(myView.Context, "Attention", "Passwords don't match");
                //    myDialog.Show();
                //}
                else
                {
                    updateUser();
                    myDialog = util.CreateButton(myView.Context, "Confirmation", "Your information has been updated.");
                    myDialog.Show();
                    userInfo();
                    changeVisibility(false);
                }
            }
        }

        
        private Dictionary<string, EditText> getUserView()
        {
            EditText name = (EditText)myView.FindViewById<EditText>(Resource.Id.TxtName);
            EditText age = (EditText)myView.FindViewById<EditText>(Resource.Id.TxtAge);
            EditText phone = (EditText)myView.FindViewById<EditText>(Resource.Id.TxtPhone);
            EditText password = (EditText)myView.FindViewById<EditText>(Resource.Id.TxtPassword);

            var userView = new Dictionary<string, EditText>
            {
                {"name", name},
                {"age", age},
                {"phone", phone},
                {"password", password}
            };

            return userView;
        }
        protected ICursor getUserInfo()
        {
            DBHelper myDB = new DBHelper(myView.Context);
            ICursor result = myDB.selectUser(userLogged);

            return result;
        }

        protected void updateUser()
        {
            var userView = getUserView();
            var userViewStr = new Dictionary<string, string>
            {
                {"name", userView["name"].Text},
                {"age", userView["age"].Text},
                {"phone", userView["phone"].Text},
                {"password", userView["password"].Text}
            };
            var whereClause = new Dictionary<string, string>
                {
                    { "EMAIL", userLogged }
                };
            DBHelper myDB = new DBHelper(myView.Context);
            myDB.updateSQL(userViewStr, "USERS",whereClause);
        }

        protected void userInfo()
        {
            var userView = getUserView();
            ICursor result = getUserInfo();

            userView["name"].Text = result.GetString(0);
            userView["age"].Text = result.GetString(1);
            userView["phone"].Text = result.GetString(2);
            userView["password"].Text = result.GetString(3);
        }

        private void changeVisibility(bool value)
        {
            var userView = getUserView();
            if (value)
            {
                userView["name"].Enabled = true;
                userView["age"].Enabled = true;
                userView["phone"].Enabled = true;
                userView["password"].Enabled = true;
                updateButton.Text = "Save";
                updateDisplay = true;
            }
            else
            {
                userView["name"].Enabled = false;
                userView["age"].Enabled = false;
                userView["phone"].Enabled = false;
                userView["password"].Enabled = false;
                updateButton.Text = "Update";
                updateDisplay = false;
            }
        }
    }
}