using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NotaFiscal.Resources;
using Microsoft.Xna.Framework.Media;
using Microsoft.Devices;
using System.IO;
using System.IO.IsolatedStorage;
using Windows.Phone.Media.Capture;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using Windows.Storage;
using System.Windows.Media.Imaging;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Phone.Net.NetworkInformation;
using System.Collections.ObjectModel;
using Microsoft.Phone.Scheduler;

namespace NotaFiscal
{
    public partial class MainPage : PhoneApplicationPage
    {
        PhotoCaptureDevice captureDevice;
        CameraCaptureSequence seq;
        static bool isReady = false;
        static bool isBusy = false;
        static bool isCapturing = false;
        MediaLibrary library = new MediaLibrary();
        bool next = true;
                
        public MainPage()
        {
            InitializeComponent();
                        
            try
            {
                if (App.Current.Host.Content.ScaleFactor == 150)
                {
                    photoButton.Margin = new Thickness(200, 751, 200, -53);
                    panelBottom.Height = 58;
                    viewfinderCanvas.Height = 698;
                }
            }
            catch (Exception e) { }
        }
        
        private async void InitializeCamera()
        {
            // Check to see if the camera is available on the device.
            if (PhotoCaptureDevice.AvailableSensorLocations.Contains(CameraSensorLocation.Back) || PhotoCaptureDevice.AvailableSensorLocations.Contains(CameraSensorLocation.Front))
            {
                // Initialize the camera, when available.
                if (PhotoCaptureDevice.AvailableSensorLocations.Contains(CameraSensorLocation.Back))
                {
                    // Use the back camera.
                    System.Collections.Generic.IReadOnlyList<Windows.Foundation.Size> SupportedResolutions =
                        PhotoCaptureDevice.GetAvailableCaptureResolutions(CameraSensorLocation.Back);
                    Windows.Foundation.Size res = SupportedResolutions[0];
                    this.captureDevice = await PhotoCaptureDevice.OpenAsync(CameraSensorLocation.Back, res);
                   viewfinderBrush.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 };
                   try
                {
                    captureDevice.SetProperty(KnownCameraGeneralProperties.EncodeWithOrientation, 90);
                }catch(Exception ex){}
                }
                else
                if (PhotoCaptureDevice.AvailableSensorLocations.Contains(CameraSensorLocation.Front))
                {
                    // Otherwise, use the front camera.
                    System.Collections.Generic.IReadOnlyList<Windows.Foundation.Size> SupportedResolutions =
                        PhotoCaptureDevice.GetAvailableCaptureResolutions(CameraSensorLocation.Front);
                    Windows.Foundation.Size res = SupportedResolutions[0];
                    this.captureDevice = await PhotoCaptureDevice.OpenAsync(CameraSensorLocation.Front, res);
                    viewfinderBrush.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = -90 };
                    try
                    {
                        captureDevice.SetProperty(KnownCameraGeneralProperties.EncodeWithOrientation, -90);
                    }
                    catch (Exception ex) { }
                }
                else {
                    MessageBox.Show("Erro 004: nenhuma câmera encontrada");
                    NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
                }

                //Set the VideoBrush source to the camera.
                viewfinderBrush.SetSource(this.captureDevice);

                viewfinderCanvas.Visibility = Visibility.Visible;
                loading.Visibility = Visibility.Collapsed;

                // The event is fired when the shutter button receives a full press.
                try
                {
                    CameraButtons.ShutterKeyPressed += OnButtonFullPress;
                }catch(Exception ex){}

                try
                {
                    captureDevice.SetProperty(KnownCameraPhotoProperties.FocusIlluminationMode, FlashState.Off);
                }catch(Exception ex){}

                try
                {
                    captureDevice.SetProperty(KnownCameraPhotoProperties.FlashMode, FlashState.Auto);
                }
                catch (Exception ex) { }

                try
                {
                    captureDevice.SetProperty(KnownCameraGeneralProperties.PlayShutterSoundOnCapture, true);
                }
                catch (Exception ex) { }
                
                captureDevice.PreviewFrameAvailable += pAv;
                isReady = true;
            }
        }

        int cont = 0;

        private async void pAv(ICameraCaptureDevice sender, object args)
        {
            try
            {
                cont++;

                if (!isBusy && cont % 100 == 0)
                {
                    cont = 0;
                    isBusy = true;
                    await captureDevice.FocusAsync();
                    isBusy = false;
                }
            }
            catch (Exception ex) { }
        }
        
        private void OnButtonFullPress(object sender, EventArgs e)
        {
            Capture();
        }
        
        public async void Capture()
        {
            if (isCapturing || !isReady)
                return;
            isCapturing = true;

            captureDevice.PreviewFrameAvailable -= pAv;

            while (isBusy) ;

            configButton.IsEnabled = false;
            configButton.Opacity = 0.2;
            photoButton.IsEnabled = false;
            photoButton.Opacity = 0.2;
            homeButton.IsEnabled = false;
            homeButton.Opacity = 0.2;
            questionButton.IsEnabled = false;
            questionButton.Opacity = 0.2;
            questioImage.Opacity = 0.2;

            try
              {     
                    seq = captureDevice.CreateCaptureSequence(1);
                    MemoryStream captureStream1 = new MemoryStream();
                    seq.Frames[0].CaptureStream = captureStream1.AsOutputStream();
                    await captureDevice.PrepareCaptureSequenceAsync(seq);
                    Application.Current.Resources.Remove("photo");
                                    
                    Dispatcher.BeginInvoke(async() =>
                    {
                    
                    await seq.StartCaptureAsync();

                    BitmapImage bi3 = new BitmapImage();
                    bi3.SetSource(captureStream1);

                    if (next)
                    {
                        isReady = false;
                        Application.Current.Resources.Add("photo", bi3);
                        this.captureDevice.Dispose();
                        configButton.IsEnabled = true;
                        configButton.Opacity = 1.0;
                        photoButton.IsEnabled = true;
                        photoButton.Opacity = 1.0;
                        homeButton.IsEnabled = true;
                        homeButton.Opacity = 1.0;
                        questionButton.IsEnabled = true;
                        questionButton.Opacity = 1.0;
                        questioImage.Opacity = 1.0;
                        NavigationService.Navigate(new Uri("/Page2.xaml", UriKind.RelativeOrAbsolute));
                    }
                    else {
                            Dispatcher.BeginInvoke(() =>
                            {
                                Upload up = new Upload();
                                bool verd = up.uploadServer(bi3);

                                if (!verd)
                                {
                                    MessageBox.Show("Erro 002: erro no banco de dados");
                                    NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
                                }
                            });
                            



                            loading.Visibility = Visibility.Collapsed;
                        viewfinderCanvas.Visibility = Visibility.Visible;
                        configButton.IsEnabled = true;
                        configButton.Opacity = 1.0;
                        photoButton.IsEnabled = true;
                        photoButton.Opacity = 1.0;
                        homeButton.IsEnabled = true;
                        homeButton.Opacity = 1.0;
                        questionButton.IsEnabled = true;
                        questionButton.Opacity = 1.0;
                        questioImage.Opacity = 1.0;
                    }
                    isReady = true;
                    isCapturing = false;
                    
                    });

                    Dispatcher.BeginInvoke(() =>
                   {
                       if (next)
                       {
                           notinhaDialog.Text = (string)AppResources.PhotoPageProcessing;
                       }
                       else {
                           notinhaDialog.Text = (string)AppResources.PhotoPageProcessing2;
                       }
                       viewfinderCanvas.Visibility = Visibility.Collapsed;
                       loading.Visibility = Visibility.Visible;
                   });       
                    
               }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro 003: erro de captura");
                    NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
                }
        }

        //Code for initialization, capture completed, image availability events; also setting the source for the viewfinder.
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                DB3DataContext db = db = new DB3DataContext(DB3DataContext.DB3ConnectionString);
                DB3 dados = new DB3();
                                
                dados = (from tarefa in db.DB3Items where tarefa.Id == 1 select tarefa).First();
                
                if (dados.Type == 1)
                    next = true;
                else
                    next = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro 002: erro no banco de dados");
                NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
            }


            notinhaDialog.Text = (string)AppResources.PhotoPageProcessing;
            viewfinderCanvas.Visibility = Visibility.Collapsed;
            loading.Visibility = Visibility.Visible;
            isReady = false;
            isCapturing = false;
            isBusy = false;
            InitializeCamera();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                CameraButtons.ShutterKeyPressed -= OnButtonFullPress;
            }
            catch (Exception ex) { }
        }
        
        private void Config_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                captureDevice.PreviewFrameAvailable -= pAv;

                while (isBusy) ;

                this.captureDevice.Dispose();
                NavigationService.Navigate(new Uri("/Page6.xaml", UriKind.RelativeOrAbsolute));
            } catch(Exception ex){
            }
        }

        private void Photo_Clicked(object sender, RoutedEventArgs e)
        {
            Capture();
        }

        private void Ask_Clicked(object sender, RoutedEventArgs e)
        {
            try{
            captureDevice.PreviewFrameAvailable -= pAv;

            while (isBusy) ;

            this.captureDevice.Dispose();
            NavigationService.Navigate(new Uri("/Page7.xaml", UriKind.RelativeOrAbsolute));
            } catch(Exception ex){            
            }
        }

        private void Home_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                captureDevice.PreviewFrameAvailable -= pAv;

                while (isBusy) ;

                this.captureDevice.Dispose();
                NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                captureDevice.PreviewFrameAvailable -= pAv;

                while (isBusy) ;

                this.captureDevice.Dispose();
            }
            catch (Exception ex)
            {
            }
            NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}