﻿using System;
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
    [Activity(Label = "News Main Page")]
    public class MainPage : Activity
    {
        Fragment[] _fragmentsArray;
        string userLogged;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.ActionBar); //Must delete android:theme="@style/AppTheme" from Manifest file
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mainPageLayout);
            userLogged = Intent.GetStringExtra("userName");
            _fragmentsArray = new Fragment[]
            {
                new FragMainNews(),
                new FragFavNews(),
                new FragUserInfo()
            };

            AddTabToActionBar("News"); //First Tab
            AddTabToActionBar("Fav"); //Second Tab
            AddTabToActionBar("User Info"); //Second Tab
        }

        void AddTabToActionBar(string tabTitle)
        {
            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText(tabTitle);
            tab.TabSelected += TabOnTabSelected;
            //tab.SetIcon(Android.Resource.Drawable.IcInputAdd);
            ActionBar.AddTab(tab);
        }

        void TabOnTabSelected(object sender, ActionBar.TabEventArgs tabEventArgs)
        {
            ActionBar.Tab tab = (ActionBar.Tab)sender;
            Fragment frag = _fragmentsArray[tab.Position];
            tabEventArgs.FragmentTransaction.Replace(Resource.Id.frameLayout1, frag);
        }
    }
}