using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Microsoft.Phone.Net.NetworkInformation;
using System.Collections.ObjectModel;
using Microsoft.Devices;
using System.IO.IsolatedStorage;
using NotaFiscal.Resources;
using Microsoft.Phone.Info;
using System.Windows.Threading;
using System.Threading;

namespace NotaFiscal
{
    public partial class Page5 : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;

        public Page5()
        {
            InitializeComponent();

            ThreadPool.QueueUserWorkItem(Upload);

            if (App.Current.Host.Content.ScaleFactor == 150)
            {
                photoButton.Margin = new Thickness(200, 751, 200, -53);
                notinha.Margin = new Thickness(249, 615, 0, 0);
                notinhaDialog.Margin = new Thickness(2, 475, 0, 0);
                panelBottom.Height = 58;
                viewFinderCanvas.Height = 698;
            }                    
        }

        DispatcherTimer timer = new DispatcherTimer();

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Count.Text = "";
                try
                {
                    using (DB2DataContext db = new DB2DataContext(DB2DataContext.DB2ConnectionString))
                    {
                        int count = (from tarefa in db.DB2Items select tarefa).Count();

                        if (count == 0)
                            Count.Text = (string)AppResources.HomePageAllSent;
                        else if (count == 1)
                            Count.Text = (string)AppResources.HomePage1NotSent;
                        else
                            Count.Text = count + (string)AppResources.HomePageNotSent;
                    }
                }
                catch (Exception ex)
                {
                    Count.Text = "";
                }

            });

            try {     
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 5);
                timer.Start();
            } catch(Exception ex){}
                        
            try
            {
                CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyReleased += CameraButtons_ShutterKeyPressed;
            }
            catch (Exception ex) { } 
                        
        }

        private void timer_Tick(object sender, EventArgs e)
        {            
            Dispatcher.BeginInvoke(() =>
            {
                Count.Text = "";
                try
                {
                    using (DB2DataContext db = new DB2DataContext(DB2DataContext.DB2ConnectionString))
                    {
                        int count = (from tarefa in db.DB2Items select tarefa).Count();

                        if (count == 0)
                            Count.Text = (string)AppResources.HomePageAllSent;
                        else if (count == 1)
                            Count.Text = (string)AppResources.HomePage1NotSent;
                        else
                            Count.Text = count + (string)AppResources.HomePageNotSent;
                    }
                }
                catch (Exception ex)
                {
                    Count.Text = "";
                }

            });
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                timer.Stop();
                CameraButtons.ShutterKeyPressed -= CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyHalfPressed -= CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyReleased -= CameraButtons_ShutterKeyPressed;
            }
            catch (Exception ex) { }
        }

        void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }
        
        private void Photo_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Config_Clicked(object sender, RoutedEventArgs e)
        {
                NavigationService.Navigate(new Uri("/Page6.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Ask_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page7.xaml", UriKind.RelativeOrAbsolute));
        }

        private void PhotoChooser(object sender, RoutedEventArgs e)
        {
            try
            {                
                photoChooserTask = new PhotoChooserTask();
                photoChooserTask.Completed += photoTask_Completed;

                photoChooserTask.Show();
            } catch(Exception ex){
                MessageBox.Show("Erro 001: erro na galeria");
                NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private async void Upload(object o) {
            Upload up = new Upload();
            await up.UploadService();
        }


        private void photoTask_Completed(object sender, PhotoResult e)
        {
            try
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    BitmapImage bmp = new BitmapImage();
                    bmp.SetSource(e.ChosenPhoto);
                    Upload up = new Upload();
                    bool verd = up.uploadServer(bmp);

                    if (!verd)
                    {
                        MessageBox.Show("Erro 002: erro no banco de dados");
                        NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
                    }
                    
                }
            } catch(Exception ex){            
            }
        }
                
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
            Application.Current.Terminate();
        }
    }
}