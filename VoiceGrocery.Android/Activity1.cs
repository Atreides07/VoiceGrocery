using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using VoiceGrocery.Android.ru.akhmed.voicegrocery;
using System.Threading;

namespace VoiceGrocery.Android
{
    [Activity(Label = "Список покупок", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : ListActivity
    {
        List<string> list = new List<string>();
        GroceryService client;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.ListItem, list);

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.Main);

            //listView = FindViewById<ListView>(Resource.Id.listView1);

            Timer timer = new Timer(TimerTick, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5));

            client = new GroceryService();
            client.GetVersionCompleted += client_GetVersionCompleted;

        }

        private void TimerTick(object state)
        {
            client.GetVersionAsync();
        }

        void client_GetVersionCompleted(object sender, ru.akhmed.voicegrocery.GetVersionCompletedEventArgs e)
        {
            try
            {
                list.Clear();

                var result = e.Result.ShopItems;


                foreach (var item in result)
                {
                    var checkBox = item.IsBought ? "( X ) " : "(   ) ";
                    list.Add(checkBox + item.Name);
                }


                this.RunOnUiThread(() =>
                {
                    this.ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.ListItem, list);
                    ((BaseAdapter)this.ListAdapter).NotifyDataSetChanged();
                });
            }
            catch (Exception)
            {
            }
            //);

        }
    }
}

