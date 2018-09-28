﻿using System;
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

namespace fotovisarn
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
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
			foreach(var arg in Environment.GetCommandLineArgs().Skip(1))
			{
				this.ChangeImage(new Uri(arg));
			}
		}
		/// <summary>
		/// TODO: Find out if this can be done with dependency properties or bindings or whatnots
		/// </summary>
		/// <param name="uri">Uri pointing to a local file to be shown in the previewer.</param>
		void ChangeImage(Uri uri)
		{
			if (uri.IsFile && MainWindow.allowedExtensions.Contains(System.IO.Path.GetExtension(uri.LocalPath)) && System.IO.File.Exists(uri.LocalPath))
			{
				this.mainImage.Source = new BitmapImage(uri);
				this.Title = "fotovisarn visar " + uri.LocalPath;
			}
		}
		public static string[] allowedExtensions = { ".jpeg", ".jpg", ".jpe", ".jfif", ".jfi", ".jif", ".png", ".bmp", ".dib", ".gif", ".tiff", ".tif", ".jxr", ".hdp", ".wdp", ".ico" };
		void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{

			if (!(this.mainImage.Source is null))
			{
				if (e.Key == Key.Left)
				{
					Uri uri = (this.mainImage.Source as BitmapImage).UriSource;
					if (uri.IsFile)
					{
						var previousFile = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(uri.LocalPath)).AsQueryable<string>().Where<string>(file => MainWindow.allowedExtensions.Contains<string>(System.IO.Path.GetExtension(file))).Reverse<string>().SkipWhile<string>(file => file != uri.LocalPath).Except<string>(new string[] { uri.LocalPath }).FirstOrDefault<string>();
						if (!(previousFile is null))
						{
							this.ChangeImage(new Uri(previousFile));
						}
					}
				}
				else if (e.Key == Key.Right)
				{
					Uri uri = (this.mainImage.Source as BitmapImage).UriSource;
					if (uri.IsFile)
					{
						var previousFile = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(uri.LocalPath)).AsQueryable<string>().Where<string>(file => MainWindow.allowedExtensions.Contains<string>(System.IO.Path.GetExtension(file))).SkipWhile<string>(file => file != uri.LocalPath).Except<string>(new string[] { uri.LocalPath }).FirstOrDefault<string>();
						if (!(previousFile is null))
						{
							this.ChangeImage(new Uri(previousFile));
						}
					}
				}
			}
		}
		private void MenuItem_Öppna_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
			{
				Title = "Öppna",
				Filter =
				"Alla bildfiler|*.jpeg;*.jpg;*.jpe;*.jfif;*.jfi;*.jif;*.png;*.bmp;*.dib;*.gif;*.tiff;*.tif;*.jxr;*.hdp;*.wdp;*.ico" +
				"|Alla filer|*.*" +
				"|JPEG (Joint Photographic Experts Group)|*.jpeg;*.jpg;*.jpe;*.jfif;*.jfi;*.jif" +
				"|PNG (Portable Network Graphics)|*.png" +
				"|BMP (Bitmap/Device Independent Bitmap)|*.bmp;*.dib" +
				"|GIF (Graphics Interchange Format)|*.gif" +
				"|TIFF (Tagged Image File Format)|*.tiff;*.tif" +
				"|JPEG XR (Joint Photographic Experts Group Extended Range)|*.jxr;*.hdp;*.wdp" +
				"|ICO (Icon)|*.ico"
			};
			Nullable<bool> result = dialog.ShowDialog();
			if (result.HasValue && result.Value)
			{
				this.ChangeImage(new Uri(dialog.FileName));
			}
		}

		private void MenuItem_Avsluta_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.MainWindow.Close();
		}

		//protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		//{
		//	MessageBoxResult result = MessageBox.Show("Vill du avsluta?", "fotovisarn", MessageBoxButton.OKCancel, MessageBoxImage.Question);
		//	if (result == MessageBoxResult.Cancel)
		//	{
		//		e.Cancel = true;
		//	}
		//	base.OnClosing(e);
		//}
	}
}
