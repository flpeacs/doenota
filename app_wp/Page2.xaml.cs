using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.IO;
using Windows.Storage;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Net.NetworkInformation;
using System.Net.Sockets;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Devices;
using System.Threading;

namespace NotaFiscal
{
    public partial class Page2 : PhoneApplicationPage
    {
        BitmapImage bi3 = new BitmapImage();

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyReleased += CameraButtons_ShutterKeyPressed;
            }
            catch (Exception ex) { }

            base.OnNavigatedTo(e);

            while (Application.Current.Resources["photo"] == null) ;      
               
            bi3 = (BitmapImage)Application.Current.Resources["photo"] ;
            goodPhoto.Source = bi3;
        }

        public Page2()
        {
            InitializeComponent();

            if (App.Current.Host.Content.ScaleFactor == 150)
            {
                photoButton.Margin = new Thickness(200, 751, 200, -53);
                panelBottom.Height = 58;
                goodPhoto.Height = 698;
            }

            
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                CameraButtons.ShutterKeyPressed -= CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyHalfPressed -= CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyReleased -= CameraButtons_ShutterKeyPressed;
            }
            catch (Exception ex) { }
        }

        private void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }
        

        private void Send_Clicked(object sender, RoutedEventArgs e)
        {

            Dispatcher.BeginInvoke(() =>
            {
                Upload up = new Upload();
                bool verd = up.uploadServer((BitmapImage)Application.Current.Resources["photo"]);
                Application.Current.Resources.Remove("photo");

                if (!verd)
                {
                    MessageBox.Show("Erro 002: erro no banco de dados");
                        NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
                } 
            
            });
                
            NavigationService.Navigate(new Uri("/Page4.xaml", UriKind.RelativeOrAbsolute));
        }
        
        private void Config_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page6.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Ask_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page7.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Home_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));            
        }
    }
}