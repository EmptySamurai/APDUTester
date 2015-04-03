using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;

namespace APDUTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        IReadersController controller = new APDUReadersController();
        public APDURequest Request { get; set; }

        public MainWindow()
        {
            Request = new APDURequest();
            this.DataContext = Request;
            InitializeComponent();
            SetReaders(controller.GetReaders());

        }

        void SetReaders(string[] readers)
        {
            this.cbReaders.ItemsSource = readers;
            this.cbReaders.SelectedIndex = 0;
        }

        private void btRefreshReaders_Click(object sender, RoutedEventArgs e)
        {
            btRefreshReaders.IsEnabled = false;
            new Thread(() =>
            {
                var readers = controller.GetReaders();
                this.cbReaders.Dispatcher.Invoke(() => SetReaders(readers));
                btRefreshReaders.Dispatcher.Invoke(() => btRefreshReaders.IsEnabled = true);
            }).Start();

        }

        private string ToHex(byte b)
        {
            return Convert.ToString(b, 16);
        }

        private void btSend_Click(object sender, RoutedEventArgs e)
        {
            btSend.IsEnabled = false;
            this.tblResponse.Text = "";

            if (this.cbReaders.SelectedItem == null)
            {
                tblResponse.Text = "Error\n" + "Reader is not selected";
                btSend.IsEnabled = true;
                return;
            }

            if (!IsValid(this.grRequestForm))
            {
                tblResponse.Text = "Error\n" + "Fill the request form";
                btSend.IsEnabled = true;
                return;
            }

            var reader = this.cbReaders.SelectedItem.ToString();
            new Thread(() =>
            {
                string responseText;
                try
                {

                    var response = controller.SendAPDU(reader, Request);
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine(String.Format("Data: {0}", ToHexString(response.Data)));
                    builder.AppendLine(String.Format("SW1: {0}; SW2: {1}", ToHex(response.SW1), ToHex(response.SW2)));
                    builder.AppendLine();
                    builder.AppendLine("Reader Info:");
                    builder.AppendLine(ReaderInfo(reader));
                    responseText = builder.ToString();
                }
                catch (Exception ex)
                {
                    responseText = "Error\n" + ex.Message;
                }
                tblResponse.Dispatcher.Invoke(() => tblResponse.Text = responseText);
                btSend.Dispatcher.Invoke(() => btSend.IsEnabled = true);
            }).Start();
        }

        private string ToHexString(byte[] value)
        {
            if (value == null)
            {
                return "";
            }
            string result = "";
            foreach (byte b in value)
            {
                result += ToHex(b) + " ";
            }
            return result;
        }

        private bool IsValid(DependencyObject obj)
        {
            return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(IsValid);
        }

        private string ReaderInfo(string reader)
        {
            string message;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("Name: {0}", reader));
            builder.AppendLine(String.Format("Port: {0}", controller.GetReaderPort(reader)));
            message = builder.ToString();
            return message;
        }
    }
}
