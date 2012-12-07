﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace MediaCenter
{
    /// <summary>
    /// Interaction logic for MediaWindow.xaml
    /// </summary>
    public partial class MediaWindow : Window
    {
        Boolean _edit = false;
        //In case we clicked on Add Media (Which means we are displaying the new media Window)
        public MediaWindow()
        {
            InitializeComponent();
            MediaLabel.Content = "Add New Media";
            Submit.Content = "Add";
        }

        //This is the constructor called in case of a Media Edit, meaning we need to fill the contents
        public MediaWindow(Media media)
        {
            InitializeComponent();
            MediaLabel.Content = "Edit Media";
            Submit.Content = "Save";
            Media editMedia = new Media();
            
            MediaIDLabel.Visibility = System.Windows.Visibility.Visible;
            MediaID.Text = editMedia.GetID().ToString();
            MediaID.Visibility = System.Windows.Visibility.Visible;
            
            MediaName.Text      = editMedia.GetName();
            MediaPath.Text      = editMedia.GetPath();
            MediaSize.Text      = editMedia.GetSize().ToString();
            MediaRating.Value   = editMedia.GetRating();
            
            if (editMedia.GetType().Name.Equals("Video"))
            {
                MediaVideoQualityLabel.IsEnabled = true;
                MediaVideoQuality.IsEnabled = true;
                MediaVideoQuality.IsChecked = ((Video)(editMedia)).IsHD();
            }
            else if (editMedia.GetType().Name.Equals("Audio"))
            {

                MediaAudioType.Text = ((Audio)(editMedia)).GetAudioType().ToString();
            }
            _edit = true;
        }

        //Event Handler: Click on Browse button
        private void MediaPathButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Media files (image, video, audio)|*.jpg;*.gif;*.png;*.aac;*.wma;*.m4a;*.ogg;*.flac;*.wav;*.mp3;*.avi;*.mpg;*.mov|All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == true)
            {
                
                FileInfo info = new FileInfo(ofd.FileName);
                Boolean valid = false;
                List<String> validVideoExt = new List<String> {".avi", ".mpg", ".mov"};
                List<String> validAudioExt = new List<String> {".aac", ".wma", ".m4a", ".ogg", ".flac", ".wav", ".mp3"};
                List<String> validImageExt = new List<String> {".jpg", ".gif", ".png"};
                
                //Is it a video file?
                if (validVideoExt.Exists(delegate(String ext) { return (ext == info.Extension); }))
                {
                    
                    MediaVideoQualityLabel.IsEnabled    = true;
                    MediaVideoQuality.IsEnabled         = true;
                    MediaVideoQuality.Visibility        = System.Windows.Visibility.Visible;
                    MediaVideoQualityLabel.Visibility   = System.Windows.Visibility.Visible;
                    MediaVideoQuality.IsChecked         = false;
                    valid = true;
                }
                //Is it an audio file?
                if (validAudioExt.Exists(delegate(String ext) { return (ext == info.Extension); }))
                {
                    MediaAudioType.Text                 = info.Extension;
                    MediaAudioTypeLabel.Visibility      = System.Windows.Visibility.Visible;
                    MediaAudioType.Visibility           = System.Windows.Visibility.Visible;
                    valid = true;
                }
                //Is it an image file?
                if (validImageExt.Exists(delegate(String ext) { return (ext == info.Extension); }))
                {
                    valid = true;
                }
                
                //If we have a valid file, we can fill both path and size.
                if (valid)
                {
                    MediaPath.Text = ofd.FileName;
                    MediaSize.Text = info.Length.ToFileSize();
                }
                
            }
            ofd = null;
        }

        //Event Handler: Change on MediaRating
        private void MediaRating_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaRating.Value = Math.Ceiling(MediaRating.Value);
            MediaRatingValue.Content = MediaRating.Value;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e) 
        {
            MCDatabase MCDB = new MCDatabase();
            Media FinalMedia = new Media();
            //ICI ON REMPLIT LE MEDIA AVEC LES VALEURS DU FORM

            if (_edit)
            {
                MCDB.UpdateMedia(FinalMedia.GetID(), FinalMedia);
            }
            else
            {
                MCDB.AddMedia(FinalMedia);
            }
        }
    }
}
