using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using VoiceGrocery.WP7.BLL;
using WPExtensions.Audio;
using System.Windows.Threading;
using System;

namespace VoiceGrocery.WP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DispatcherTimer dispathcerTimer = new DispatcherTimer();
            dispathcerTimer.Interval = TimeSpan.FromSeconds(5);
            dispathcerTimer.Tick += dispathcerTimer_Tick;
            dispathcerTimer.Start();
        }

        void dispathcerTimer_Tick(object sender, System.EventArgs e)
        {
            var client = new ServiceReference1.GroceryServiceSoapClient();
            client.GetVersionCompleted += client_GetVersionCompleted;
            client.GetVersionAsync();
        }

        void client_GetVersionCompleted(object sender, ServiceReference1.GetVersionCompletedEventArgs e)
        {
            if (e.Result.Version > logicLayer.GetVersion())
            {
                logicLayer.UpdateShopList(e.Result);
                RefreshView();
            }
        }

        private string defaultHeader = "Список покупок";

        LogicLayer logicLayer=new LogicLayer();
        

        private void Send(byte[] wav)
        {
            var client = new HttpWebClient();
            client.Post("http://voice.akhmed.ru/recognize.ashx", wav, (result) => Dispatcher.BeginInvoke(() => ParseString(result)));
            //client.Post("http://localhost:23877/recognize.ashx", wav, (result) => Dispatcher.BeginInvoke(() => MessageBox.Show(result)));
        }

        private void ParseString(string result)
        {
            logicLayer.Parse(result);

            RefreshView();
            
        }

        private void RefreshView()
        {
            shoplist.ItemsSource = logicLayer.GetShopItems().ToList();
        }

        private bool isRecording = false;
        private readonly MicrophoneWrapper microphone = new MicrophoneWrapper();
        

        private void ApplicationBarRecordIconButton_Click(object sender, System.EventArgs e)
        {
            var btn = sender as Button;
            if (!isRecording)
            {
                microphone.Record();
                PageTitle.Text= "Слушаю...";
            }
            else if (isRecording)
            {
                microphone.Stop();
                var wav = microphone.GetWavContent();
                Send(wav);
                PageTitle.Text = defaultHeader;
            }
            isRecording = !isRecording;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}