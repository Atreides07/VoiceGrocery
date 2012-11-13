using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VoiceGrocery.WP7.BLL;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace VoiceGrocery.Win8Store
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : VoiceGrocery.Win8Store.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();

            DispatcherTimer dispathcerTimer = new DispatcherTimer();
            dispathcerTimer.Interval = TimeSpan.FromSeconds(5);
            dispathcerTimer.Tick += dispathcerTimer_Tick;
            dispathcerTimer.Start();
        }

        async void dispathcerTimer_Tick(object sender, object e)
        {
            var client = new ServiceReference1.GroceryServiceSoapClient();
            var result = await client.GetVersionAsync();

            if (result.Body.GetVersionResult.Version > logicLayer.GetVersion())
            {
                logicLayer.UpdateShopList(result.Body.GetVersionResult);
                RefreshView();
            }
            
        }

        private void RefreshView()
        {
            listView.ItemsSource = logicLayer.GetShopItems().ToList();
        }


        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {

        }

        LogicLayer logicLayer = new LogicLayer();

       


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}
