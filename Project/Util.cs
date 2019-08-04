using Android.App;
using Android.Content;
using Android.Preferences;
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

        public void setPref(Context context,string key, string value)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(key, value);
            // editor.Commit();    // applies changes synchronously on older APIs
            editor.Apply();        // applies changes asynchronously on newer APIs

        }

        public string getPref(Context context, string key)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            string mString = prefs.GetString(key, "empty");

            return mString;
        }
    }
}