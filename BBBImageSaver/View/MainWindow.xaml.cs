using BBBImageSaver.Domain;
using Microsoft.Win32;
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
using Unity;
namespace BBBImageSaver
{
    public partial class MainWindow : Window, Domain.IFormDataSender
    {
        private UnityContainer container = new UnityContainer();
        public MainWindow()
        {
            //composition root
            var mainWindow = this;

            container.RegisterInstance<MainWindow>(mainWindow);
            container.RegisterType<IFormDataSender, MainWindow>();
            container.RegisterType<IConverter, PDFConverter>();
            container.RegisterType<IImageDownloader, WebPngDownloader>();
            container.RegisterType<Processor>();
            container.RegisterType<IFormDataReciever, Processor>();

            var sender = container.Resolve<IFormDataSender>();
            var reciever = container.Resolve<IFormDataReciever>();
            sender.OnSent += reciever.OnRecieved;
            var proc = container.Resolve<Processor>();
            container.Resolve<IImageDownloader>().OnEndDownload += OnItemDownloaded;
            proc.OnCountOfPagesGot += OnCountOfPagesGot;

            InitializeComponent();
        }

        public event Action<string, string> OnSent;

        private void OnCountOfPagesGot(uint c)
        {
            //count.Content = c.ToString();
        }
        private uint itemCounter = 0;
        public void OnItemDownloaded()
        {
            //progressBar.Value = itemCounter / 100.0;
            itemCounter++;
        }
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void OnOpenButtonClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog v = new SaveFileDialog();
            v.Filter = "pdf file (*.pdf)|*.pdf";
            if(v.ShowDialog().HasValue)
            {
                savePath.Text = v.FileName;
            }
        }

        private void OnStartButtonClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(savePath.Text))
            {
                MessageBox.Show("введите путь сохранения");
                return;
            }
            if (string.IsNullOrEmpty(url.Text))
            {
                MessageBox.Show("введите url");
                return;
            }
            OnSent?.Invoke(url.Text, savePath.Text);
        }
    }
}
