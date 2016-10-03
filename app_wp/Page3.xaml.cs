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
using System.IO;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using NotaFiscal;
using System.Windows.Media;
using Microsoft.Devices;
using NotaFiscal.Resources;

namespace NotaFiscal
{
    public partial class Page3 : PhoneApplicationPage
    {
        int antigo, atual;
        
        private RadioButton GetCheckedRadioButton(UIElementCollection children, String groupName)
        {
            return children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true && rb.GroupName == groupName);
        }

        public Page3()
        {
            InitializeComponent();

            try
            {
                using (DBDataContext db = new DBDataContext(DBDataContext.DBConnectionString))
                {
                    List<DB> dados = new List<DB>();
                    DB ale = new DB();
                    ale.Id = -1;
                    ale.Institution_Name = AppResources.InstitutionsPageRandom;
                    dados.Add(ale);

                    dados.AddRange((from tarefa in db.DBItems select tarefa).ToList());

                    bool sel = false;

                    for (int i = 0; i < dados.Count; i++)
                    {

                        RadioButton radio = new RadioButton();
                        radio.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 189, 162, 193));
                        radio.Foreground = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                        radio.FontFamily = new FontFamily("Assets/Fonts/roboto_fonts/Roboto-Thin.ttf#Roboto");
                        radio.Tag = dados.ElementAt(i).Id;
                        radio.FontSize = 27;

                        string message = "";

                        String[] listStrings = dados.ElementAt(i).Institution_Name.Split(' ');

                        for (int j = 0; j < listStrings.Length; j++) {
                            int cont = 0;
                            for (; j < listStrings.Length; j++) {
                                if (listStrings[j].Length > 30 && cont == 0)
                                {
                                    message += " " + listStrings[j];
                                    break;
                                }

                                if (cont + listStrings[j].Length + 1 <= 30)
                                {
                                    cont += listStrings[j].Length + 1;
                                    message += " " + listStrings[j];
                                }
                                else {
                                    j--;
                                    break;
                                }
                          
                            }

                            if(j < listStrings.Length - 1)
                                message += "\n";
                        }

                        radio.Content = message;
                        
                        radio.GroupName = "group1";
                        radio.Checked += radio_Checked;

                        if (dados.ElementAt(i).Selected == true)
                        {
                            antigo = dados.ElementAt(i).Id;
                            radio.IsChecked = true;
                            sel = true;
                        }

                        RadioButtonContainer.Children.Add(radio);
                    }

                    if (sel == false)
                    {
                        RadioButton radio = (RadioButton)RadioButtonContainer.Children.ElementAt(0);
                        radio.IsChecked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro 002: erro no banco de dados");
                NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
            }
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
        }

        void radio_Checked(object sender, RoutedEventArgs e)
        {
            atual = (int) ((RadioButton)sender).Tag;
        }

        private void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e) {
            Institution_Choice();

            try
            {
                CameraButtons.ShutterKeyPressed -= CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyHalfPressed -= CameraButtons_ShutterKeyPressed;
                CameraButtons.ShutterKeyReleased -= CameraButtons_ShutterKeyPressed;
            }
            catch (Exception ex) { }
        }

        private void Institution_Choice()
        {
            if (atual == antigo)
                return;

            try
            {
                using (DBDataContext db = new DBDataContext(DBDataContext.DBConnectionString))
                {
                    try
                    {
                        DB update = (from tarefa in db.DBItems where tarefa.Id == antigo select tarefa).First();
                        update.Selected = false;
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                    }

                    if (atual != -1)
                    {
                        DB update2 = (from tarefa in db.DBItems where tarefa.Id == atual select tarefa).First();
                        update2.Selected = true;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro 002: erro no banco de dados");
                NavigationService.Navigate(new Uri("/Page5.xaml", UriKind.RelativeOrAbsolute));
            }
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