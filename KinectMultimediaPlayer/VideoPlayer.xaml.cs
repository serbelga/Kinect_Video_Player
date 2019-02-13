using Microsoft.Kinect.Toolkit.Controls;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectMultimediaPlayer
{
    /// <summary>
    /// Lógica de interacción para Page1.xaml
    /// </summary>
    public partial class VideoPlayer : Page
    {
        private Uri uri;
        public VideoPlayer(String directory)
        {
            InitializeComponent();
            uri = new Uri(directory);
            mediaElement.Source = uri;

            
            
            mediaElement.Play();
        }

        /*
        private void setTimeLine(Uri uri)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();

            
            mediaPlayer.ScrubbingEnabled = true;
            mediaPlayer.Open(uri);
            mediaPlayer.Position = TimeSpan.FromSeconds(50);
            mediaPlayer.MediaOpened += new EventHandler(mediaplayer_OpenMedia);
        }

        private void mediaplayer_OpenMedia(object sender, EventArgs e)
        {
            //----------------< mediaplayer_OpenMedia() >---------------- 
            //*create mediaplayer in memory and jump to position 
            //< draw video_image > 
            MediaPlayer mediaPlayer = sender as MediaPlayer;
            
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawVideo(mediaPlayer, new Rect(0, 0, 160, 100));
            drawingContext.Close();

            double dpiX = 1 / 200;
            double dpiY = 1 / 200;
            RenderTargetBitmap bmp = new RenderTargetBitmap(160, 100, dpiX, dpiY, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            //</ draw video_image > 

            //< set Image > 
            Image newImage = new Image();
            newImage.Source = bmp;
            newImage.Stretch = Stretch.Uniform;
            newImage.Width = 100;
            //</ set Image > 

            //< add > 
            KinectTileButton button = new KinectTileButton();
            button.Label = "test";

            button.Width = 200;
            button.Height = 200;
            button.FontSize = 18.0;
            button.Content = newImage;
            //button.Tag = videos[i].FullName.ToString();

            scrollContent.Children.Insert(0, button);


            button.Click += (o, re) =>
            {
                mediaElement.Position = TimeSpan.FromSeconds(50);
            };
            //</ add > 
            //----------------< mediaplayer_OpenMedia() >---------------- 
        }*/

        private void setTimeLine(Uri uri)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();


            mediaPlayer.ScrubbingEnabled = true;
            mediaPlayer.Open(uri);
            mediaPlayer.Play();
            mediaPlayer.Position = TimeSpan.FromSeconds(50);
            mediaPlayer.MediaOpened += new EventHandler(mediaplayer_OpenMedia);
        }

        private void mediaplayer_OpenMedia(object sender, EventArgs e)
        {
            //----------------< mediaplayer_OpenMedia() >---------------- 
            //*create mediaplayer in memory and jump to position 
            //< draw video_image > 
            MediaPlayer mediaPlayer = sender as MediaPlayer;
            var position = mediaPlayer.Position;
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawVideo(mediaPlayer, new Rect(0, 0, 160, 100));
            drawingContext.Close();

            double dpiX = 1 / 200;
            double dpiY = 1 / 200;
            RenderTargetBitmap bmp = new RenderTargetBitmap(160, 100, dpiX, dpiY, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            //</ draw video_image > 

            //< set Image > 
            Image newImage = new Image();
            newImage.Source = bmp;
            newImage.Stretch = Stretch.Uniform;
            newImage.Width = 100;
            //</ set Image > 

            //< add > 
            KinectTileButton button = new KinectTileButton();
            button.Label = "test";

            button.Width = 200;
            button.Height = 200;
            button.FontSize = 18.0;
            button.Content = newImage;
            //button.Tag = videos[i].FullName.ToString();

            scrollContent.Children.Insert(0, button);


            button.Click += (o, re) =>
            {
                mediaElement.Position = position;
            };
            //</ add > 
            //----------------< mediaplayer_OpenMedia() >---------------- 
        }

        /// <summary>
        /// Play video button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayOnClick(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        /// <summary>
        /// Pause video button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseOnClick(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
            
        }

        /// <summary>
        /// Stop video button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopOnClick(object sender, RoutedEventArgs e)
        {

            mediaElement.Stop();
            mediaElement.Play();
        }

        private void Element_MediaEnded(object sender, RoutedEventArgs e)
        {
            //TODO : Hide video controls + Show Restart and Back
        }

        private void Element_MediaOpened(object sender, EventArgs e)
        {
            //timeLineSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            setTimeLine(uri);
        }

        private void MediaTimeChanged(object sender, EventArgs e)
        {
            //timeLineSlider.Value = mediaElement.Position.TotalMilliseconds;
        }

        // Jump to different parts of the media (seek to). 
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            //int SliderValue = (int) timeLineSlider.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);

           // mediaElement.Position = ts;
            
        }


    }
}
