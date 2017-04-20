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
using System.Net.Http.Headers;

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

            client.GetAsync(@"http://localhost:43416/api/room")
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
                            (Action)(() =>
                            {
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

            string url = string.Format("http://localhost:43416/api/room?name={0}", Uri.EscapeDataString(textBoxFileName.Text));


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
            string url = string.Format("http://localhost:43416/api/room?name={0}", Uri.EscapeDataString(textBoxFileName.Text));

            client.DeleteAsync(url);
        }

        //async Task<Uri> Createnew_Click(object sender, RoutedEventArgs e)
        //{
        //    HttpClient client = new HttpClient();
        //    string url = string.Format("http://localhost:43416/api/room?name={0}", Uri.EscapeDataString(textBoxFileName.Text));

        //    HttpResponseMessage response = await client.PostAsJsonAsync("api/room",url);
        //    response.EnsureSuccessStatusCode();

        //    return response.Headers.Location;

        //}
        private void Createnew_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    HttpClient client = new HttpClient();
            //    var response = await client.PostAsJsonAsync("http://localhost:43416/api/room?name={0}", textBoxFileName.Text);
            //    //response.EnsureSuccessStatusCode(); // Throw on error code.
            //    MessageBox.Show("Student Added Successfully", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            //    //listOfFiles.ItemsSource = await GetAllFiles();
            //    listOfFiles.ScrollIntoView(listOfFiles.ItemContainerGenerator.Items[listOfFiles.Items.Count - 1]);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Student not Added, May be due to Duplicate ID");
            //}


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:43416/");

            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            //var employee = new Employee();

            //employee.Id = int.Parse(txtId.Text);
            //employee.Name = txtName.Text;
            //employee.Address = txtAddress.Text;
            //employee.Designation = txtDesignation.Text;

            var response = client.PostAsJsonAsync("api/room", textBoxFileName.Text);

            //if (response.IsSuccessStatusCode)
            //{
            //    MessageBox.Show("Employee Added");
            //    txtId.Text = "";
            //    txtName.Text = "";
            //    txtAddress.Text = "";
            //    txtDesignation.Text = "";
            //    BindEmployeeList();
            //}
            //else
            //{
            //    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            //}

        }
      

    }
}
