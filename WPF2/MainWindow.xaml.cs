using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ImageText> imagelist = new ObservableCollection<ImageText>();
        public MainWindow()
        {
            InitializeComponent();
            PatiKot.ItemsSource = imagelist;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simple image browser!", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryInfo root = new DirectoryInfo(folder.SelectedPath);
                string[] extensions = new[] { ".jpg", ".png", ".bmp", ".jpeg" };

                FileInfo[] files =
                    root.GetFiles()
                    .Where(f => extensions.Contains(f.Extension.ToLower()))
                    .ToArray();
                imagelist.Clear();
                if (files != null)
                {
                    foreach (FileInfo fi in files)
                        imagelist.Add(new ImageText { image = new BitmapImage(new Uri(fi.FullName)), path = fi.Name, fullpath = fi.FullName, date = fi.CreationTime });

                }

            }
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OtworzOkno(((sender as Button).DataContext as ImageText).path, ((sender as Button).DataContext as ImageText).fullpath, ((sender as Button).DataContext as ImageText).date);
        }
        private void OtworzOkno(string path, string fullpath, DateTime date)
        {
            BitmapImage bi = new BitmapImage(new Uri(fullpath));

            double mnoznik = 1;
            if ((bi.Height + 100) > SystemParameters.PrimaryScreenHeight || (bi.Width + 300) > SystemParameters.PrimaryScreenWidth)
            {
                if ((bi.Height + 100) > SystemParameters.PrimaryScreenHeight)
                    mnoznik = (SystemParameters.PrimaryScreenHeight - 100) / bi.Height;
                if ((bi.Width + 300) > SystemParameters.PrimaryScreenWidth)
                    if ((SystemParameters.PrimaryScreenWidth - 350) / bi.Width < mnoznik)
                        mnoznik = (SystemParameters.PrimaryScreenWidth - 350) / bi.Width;

            }
            double localwidth = mnoznik * bi.Width;
            double localheight = mnoznik * bi.Height;
            PhotoWindow pw = new PhotoWindow(bi, path, fullpath, localwidth, localheight, bi.Width, bi.Height, date);
            pw.Height = pw.MaxHeight = pw.MinHeight = localheight + 60;
            pw.Width = pw.MaxWidth = pw.MinWidth = localwidth + 340;
            pw.Title = path;
            pw.ShowDialog();
        }
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Pictures (*.png;*.jpeg;*.bmp;*.jpg)|*.png;*.jpeg;*.bmp;*.jpg";
            if (of.ShowDialog() == true)
            {
                //of.SafeFileName
                FileInfo fi = new FileInfo(of.FileName);
                OtworzOkno(of.SafeFileName, of.FileName, fi.CreationTime);
            }
        }
        public class ImageText
        {
            public BitmapImage image { get; set; }
            public string path { get; set; }
            public string fullpath { get; set; }
            public DateTime date { get; set; }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private object znacznik = null;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string drive in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = drive;
                item.Tag = drive;
                item.FontWeight = FontWeights.Normal;

                item.Items.Add(znacznik);
                item.Expanded += new RoutedEventHandler(ExpandedFolder);

                foldersItem.Items.Add(item);
            }
        }
        void ExpandedFolder(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == znacznik)
            {
                item.Items.Clear();
                try
                {
                    foreach (string drive in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = drive.Substring(drive.LastIndexOf("\\") + 1);
                        subitem.Tag = drive;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(znacznik);
                        subitem.Expanded += new RoutedEventHandler(ExpandedFolder);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
        }

        private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            //foldersItem.Items.Remove(item);
            if (item.Items.Count == 1 && item.Items[0] == znacznik)
            {
                item.Items.Clear();
                try
                {
                    foreach (string drive in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = drive.Substring(drive.LastIndexOf("\\") + 1);
                        subitem.Tag = drive;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(znacznik);
                        subitem.Expanded += new RoutedEventHandler(ExpandedFolder);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
            //foldersItem.Items.Add(item);

        }
    }
}
