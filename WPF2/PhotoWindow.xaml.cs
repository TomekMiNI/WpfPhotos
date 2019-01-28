using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace WPF2
{
    /// <summary>
    /// Interaction logic for PhotoWindow.xaml
    /// </summary>
    public partial class PhotoWindow : Window
    {
        BitmapImage oribi;
        string oripath;
        public PhotoWindow(BitmapImage bi, string shortpath, string path, double width, double height, double oriwidth, double oriheight, DateTime date)
        {
            InitializeComponent();
            oribi = bi;
            oripath = path;
            obrazek.Width = width;
            obrazek.Height = height;
            obrazek.Source = oribi;
            imagewidth.Text = oriwidth.ToString() + " px";
            imageheight.Text = oriheight.ToString() + " px";
            imagename.Text = shortpath;
            imagedate.Text = date.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter="BMP files (*.bmp) | *.bmp | JPEG files (*.jpeg) | *.jpeg | PNG files (*.png) | *.png";
            sf.FileName = this.Title;
            if (sf.ShowDialog()== true)
            {

                BitmapEncoder encoder = new BmpBitmapEncoder();
                
                switch (sf.FilterIndex)
                {
                    case 1:
                        {
                            encoder = new JpegBitmapEncoder();
                            break;
                        }
                    case 2:
                        {
                            encoder = new PngBitmapEncoder();
                            break;
                        }
                }
                using (var fileStream = new FileStream(sf.FileName, FileMode.Create))
                {
                    encoder.Frames.Add(BitmapFrame.Create(new Uri(oripath)));
                    encoder.Save(fileStream);
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
