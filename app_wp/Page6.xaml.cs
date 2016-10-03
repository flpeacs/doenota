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

namespace NotaFiscal
{
    public partial class Page6 : PhoneApplicationPage
    {
        DB3DataContext db;

        public Page6()
        {
            InitializeComponent();
                        
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            db = new DB3DataContext(DB3DataContext.DB3ConnectionString);

            try
            {
                CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyReleased += CameraButtons_ShutterKeyPressed;
            }
            catch (Exception ex) { }

            try
            {
                DB3 dados = new DB3();

                dados = (from tarefa in db.DB3Items select tarefa).First();

                if (dados.Type == 1)
                {
                    check.IsChecked = true;
                }

                dados = (from tarefa in db.DB3Items where tarefa.Id == 1 select tarefa).First();

                if (dados.Type == 1)
                {
                    check2.IsChecked = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro 002: erro no banco de dados");
                NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
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
           
            try
            {
                DB3 dados = new DB3();

                dados = (from tarefa in db.DB3Items select tarefa).First();

                if (check.IsChecked == true)
                    dados.Type = 1;
                else
                    dados.Type = 0;
                
                  db.SubmitChanges();
                
                  dados = (from tarefa in db.DB3Items where tarefa.Id == 1 select tarefa).First();

                  if (check2.IsChecked == true)
                      dados.Type = 1;
                  else
                      dados.Type = 0;

                  db.SubmitChanges();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro 002: erro no banco de dados");
                NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
            }            
        }

        private void Institution_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page3.xaml", UriKind.RelativeOrAbsolute));
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