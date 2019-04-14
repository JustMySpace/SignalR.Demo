using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
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

namespace SignalR.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        //public string UserName { get; set; }

        public IHubProxy HubProxy { get; set; }

        const string ServerURI = "http://193.112.56.169:83/signalr/";
        public HubConnection Connection { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StatusText.Content = "Connecting to server...";
            ConnectAsync();
        }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("chatHub");
            HubProxy.On<string, string>("ReceiveMessage", (user, message) =>
               this.Dispatcher.Invoke(() =>
                   RichTextBoxConsole.AppendText(String.Format("{0}: {1}\r", user, message))
               ));
            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                StatusText.Content = "Unable to connect to server: Start server before connecting clients.";
                //No connection: Don't enable Send button or show chat UI
                return;
            }
            RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + "\r");
        }

        void Connection_Closed()
        {
            //Hide chat UI; show login UI
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() => StatusText.Content = "You have been disconnected.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HubProxy.Invoke("SendMessage", UserName.Text, Message);
           
        }
    }
}
