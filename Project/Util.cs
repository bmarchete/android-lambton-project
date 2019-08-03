﻿using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Project
{
    class Util
    {
        public bool ValidateTextFields(ViewGroup form)
        {
            for (int i = 0; i < form.ChildCount; i++)
            {
                View field = form.GetChildAt(i);
                if (field.GetType() == typeof(EditText))
                {
                    if (((EditText)field).Text == "")
                        return false;
                }
            }
            return true;
        }

        public Dialog CreateButton(Context context, string title, string message)
        {
            Android.App.AlertDialog.Builder myAlert = new Android.App.AlertDialog.Builder(context);
            myAlert.SetTitle(title);
            myAlert.SetMessage(message);
            myAlert.SetPositiveButton("OK", OkAction);
            //myAlert.SetNegativeButton("Cancel", OkAction);
            Dialog myDialog = myAlert.Create();

            return myDialog;
        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            System.Console.WriteLine("OK Button Cliked");
        }

        private void CancelAction(object sender, DialogClickEventArgs e)
        {
            System.Console.WriteLine("Cancel Button Cliked");
        }
    }
}