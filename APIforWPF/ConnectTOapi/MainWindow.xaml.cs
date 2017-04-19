﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Web.Script.Serialization;
namespace ConnectTOapi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void listOfFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxFileName.Text = listOfFiles.SelectedItem.ToString();
        }

        private void GetAllFiles_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            client.GetAsync(@"http://localhost:43416/api/directory")
                .ContinueWith(response =>
                {
                    if (response.Exception != null)
                    {
                        MessageBox.Show(response.Exception.Message);
                    }
                    else
                    {
                        //System.Threading.Thread.Sleep(2000);
                        HttpResponseMessage message = response.Result;
                        string responseText = message.Content.ReadAsStringAsync().Result;

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        ObservableCollection<string> files = jss.Deserialize<ObservableCollection<string>>(responseText);

                        Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            (Action)(() => {
                                listOfFiles.DataContext = files;

                                Binding binding = new Binding();
                                listOfFiles.SetBinding(ItemsControl.ItemsSourceProperty, binding);

                                (listOfFiles.ItemsSource as ObservableCollection<string>).RemoveAt(0);
                            }));
                    }
                });

        }

        private void buttonGetByName_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            string url = string.Format("http://localhost:43416/api/directory?name={0}", Uri.EscapeDataString(textBoxFileName.Text));


            client.GetAsync(url)
                .ContinueWith(response =>
                {
                    if (response.Exception != null)
                    {
                        MessageBox.Show(response.Exception.Message);
                    }
                    else
                    {
                        //System.Threading.Thread.Sleep(2000);
                        HttpResponseMessage message = response.Result;
                        string responseText = message.Content.ReadAsStringAsync().Result;

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string body = jss.Deserialize<string>(responseText);

                        Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            (Action)(() => { textBoxFileContent.Text = body; }));
                    }
                });


        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            string url = string.Format("http://localhost:43416/api/directory?name={0}", Uri.EscapeDataString(textBoxFileName.Text));

            client.DeleteAsync(url);
        }
    }
}
