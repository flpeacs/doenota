using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using NotaFiscal.Resources;

namespace NotaFiscal
{    
   
    public partial class Page4 : PhoneApplicationPage
    {
        private void DonateAgain_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        public Page4()
        {
            InitializeComponent();
                        
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyReleased += CameraButtons_ShutterKeyPressed;
            }
            catch (Exception ex) { }

            try
            {
                using (DBDataContext dbAux = new DBDataContext(DBDataContext.DBConnectionString))
                {
                    int count = (from tarefa in dbAux.DBItems where tarefa.Selected == true select tarefa).Count();

                    if (count > 0)
                        institution.Text = (from tarefa in dbAux.DBItems where tarefa.Selected == true select tarefa).First().Institution_Name;
                    else
                    {
                        institution.Text = AppResources.InstitutionsPageRandom;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro 002: erro no banco de dados");
                NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
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