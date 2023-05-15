using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp.Wpf;
using VncSharp;
using MessageBox = System.Windows.MessageBox;

namespace IntrexPreviewApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthenticateDelegate vncAuthenticateDelegate = new AuthenticateDelegate(GetVncPassword);

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Global.LoadSettings();
                ConnectCognexHmi();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConnectVnc()
        {
            

            try
            {
                vncView.GetPassword = vncAuthenticateDelegate;
                vncView.Connect(Global.plcIp, false, true);
                vncView.ConnectComplete += VncView_ConnectComplete;
                vncView.ConnectionLost += VncView_ConnectionLost;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error connecting to VNC: {0}\r\n{1}",Global.plcIp,ex.Message));
            }
        }

        private void ConnectCognexHmi()
        {
            cef1.Address = Global.cogIp;

        }

        private void VncView_ConnectionLost(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Lost connection to host: " + Global.plcIp);
            }
            catch (Exception)
            {

                MessageBox.Show("Lost connection to host and crashed event!");
            }
        }

        private void VncView_ConnectComplete(object sender, VncSharp.ConnectEventArgs e)
        {
            
        }

        public static string GetVncPassword()
        {
            return Global.vncLogin;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectVnc();
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dr = System.Windows.Forms.MessageBox.Show("Czy na pewno chcesz zamknąć aplikację?", "Zamykanie aplikacji", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
                Process.GetCurrentProcess().Kill();
            }
        }

        private void btnSet1_Click(object sender, RoutedEventArgs e)
        {
            col2.Width = new GridLength(0, GridUnitType.Star);
            col0.Width = new GridLength(1, GridUnitType.Star);
        }

        private void btnPlc_Click(object sender, RoutedEventArgs e)
        {

            col0.Width = new GridLength(0, GridUnitType.Star);
            col2.Width = new GridLength(1, GridUnitType.Star);
        }

        private void btnOverview_Click(object sender, RoutedEventArgs e)
        {

            col0.Width = new GridLength(1, GridUnitType.Star);
            col2.Width = new GridLength(3, GridUnitType.Star);
        }

        private void btnStartCamera_Click(object sender, RoutedEventArgs e)
        {
            vncView = new RemoteDesktop();
            ConnectVnc();

        }

        private void btnStartVnc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectVnc();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (vncView.IsConnected)
                vncView.Disconnect();

            cef1.Dispose();

            MainWindow mw = new MainWindow();
            mw.Show();
            this.Hide(); //<----------------------- nie podoba mi się to, wymyśl coś lepszego debilu :c
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vncView.IsConnected)
                vncView.Disconnect();

            base.OnClosing(e);
        }

        private void menuDisconnect_Click(object sender, RoutedEventArgs e)
        {
            vncView.Disconnect();
        }
    }


}
