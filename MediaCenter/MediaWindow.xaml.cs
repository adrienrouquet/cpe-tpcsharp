using System;
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
        private Boolean _edit = false;
        enum MediaType {False,Image,Audio,Video};
        private MediaType _mediaType = MediaType.False;

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
                _mediaType = MediaType.Video;
            }
            else if (editMedia.GetType().Name.Equals("Audio"))
            {
                MediaAudioType.Text = ((Audio)(editMedia)).GetAudioType().ToString();
                _mediaType = MediaType.Audio;
            }
            else if (editMedia.GetType().Name.Equals("Image"))
            {
                _mediaType = MediaType.Image;
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

                //Clearing previous fields
                ClearFields();

                //Is it a video file?
                if (validVideoExt.Exists(delegate(String ext) { return (ext == info.Extension); }))
                {
                    MediaVideoQualityLabel.IsEnabled    = true;
                    MediaVideoQuality.IsEnabled         = true;
                    MediaVideoQuality.Visibility        = System.Windows.Visibility.Visible;
                    MediaVideoQualityLabel.Visibility   = System.Windows.Visibility.Visible;
                    MediaVideoQuality.IsChecked         = false;
                    valid = true;
                    _mediaType = MediaType.Video;
                }
                //Is it an audio file?
                if (validAudioExt.Exists(delegate(String ext) { return (ext == info.Extension); }))
                {
                    MediaAudioType.Text                 = info.Extension;
                    MediaAudioTypeLabel.Visibility      = System.Windows.Visibility.Visible;
                    MediaAudioType.Visibility           = System.Windows.Visibility.Visible;
                    valid = true;
                    _mediaType = MediaType.Audio;
                }
                //Is it an image file?
                if (validImageExt.Exists(delegate(String ext) { return (ext == info.Extension); }))
                {
                    valid = true;
                    _mediaType = MediaType.Image;
                }
                
                //If we have a valid file, we can fill both path and size.
                if (valid)
                {
                    MediaPath.Text = ofd.FileName;
                    MediaSize.Text = info.Length.ToFileSize();
                    Submit.IsEnabled = true;
                }
                
            }
            ofd = null;
        }

        private void ClearFields()
        {
            _mediaType = MediaType.False;
            MediaVideoQualityLabel.IsEnabled = false;
            MediaVideoQuality.IsEnabled = false;
            MediaVideoQualityLabel.Visibility = System.Windows.Visibility.Hidden;
            MediaVideoQuality.Visibility = System.Windows.Visibility.Hidden;
            MediaAudioType.Text = "";
            MediaAudioType.Visibility = System.Windows.Visibility.Hidden;
            MediaAudioTypeLabel.Visibility = System.Windows.Visibility.Hidden;
            Submit.IsEnabled = false;
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

            Media FinalMedia = null;

            if (_mediaType == MediaType.Video)
            {
                FinalMedia = new Video((String) MediaName.Text, (String) MediaPath.Text, (String) MediaSize.Text, (Int32) MediaRating.Value, (Boolean) MediaVideoQuality.IsChecked);
            }
            else if (_mediaType == MediaType.Audio)
            {
                FinalMedia = new Audio((String) MediaName.Text, (String)MediaPath.Text, (String)MediaSize.Text, (Int32)MediaRating.Value, (String) MediaAudioType.Text);
            }
            else if (_mediaType == MediaType.Image)
            {
                FinalMedia = new Image((String)MediaName.Text, (String)MediaPath.Text, (String)MediaSize.Text, (Int32)MediaRating.Value);
            }

            
            //ICI ON REMPLIT LE MEDIA AVEC LES VALEURS DU FORM

            Debug debug = new Debug();
            debug.Show("TEST");
            debug.Show(FinalMedia.GetType().Name.ToString());
            debug.Show(FinalMedia.GetName());
            debug.Show(FinalMedia.GetPath());
            debug.Show(FinalMedia.GetRating().ToString());

            if (_edit)
            {
                FinalMedia.SetID(Int32.Parse(MediaID.Text));
                MCDB.UpdateMedia(FinalMedia);
            }
            else
            {
                MCDB.AddMedia(FinalMedia);
            }
        }
    }
}
