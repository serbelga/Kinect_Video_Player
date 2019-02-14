using Microsoft.Kinect.Toolkit.Controls;
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
        private List<double> positions;
        public VideoPlayer(String directory)
        {
            InitializeComponent();
            uri = new Uri(directory);
            mediaElement.Source = uri;

            
            
            mediaElement.Play();
            

        }

        private void setTimeLine(Uri uri)
        {
            positions = new List<double>();
            var frameLength = mediaElement.NaturalDuration.TimeSpan.TotalSeconds / 10;
            for (var i = 0; i < 10; i++)
            {
                positions.Add(i * frameLength);
            }
            BuildListView();
        }

        private void BuildListView()
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.ScrubbingEnabled = true;
            mediaPlayer.Open(uri);
            mediaPlayer.Pause();
            mediaPlayer.MediaOpened += new EventHandler(delegate (Object o, EventArgs e)
            {
                MediaPlayer mediaPlayer1 = o as MediaPlayer;
                for (int i = 0; i < positions.Count; i++)
                {
                    var ts = new TimeSpan(0, 0, Convert.ToInt32(positions.ElementAt(i)));
                    mediaPlayer.Play();
                    mediaPlayer1.Position = ts;
                    DrawingVisual drawingVisual = new DrawingVisual();
                    DrawingContext drawingContext = drawingVisual.RenderOpen();
                    drawingContext.DrawVideo(mediaPlayer1, new Rect(0, 0, 160, 100));
                    drawingContext.Close();

                    double dpiX = 1 / 200;
                    double dpiY = 1 / 200;
                    RenderTargetBitmap bmp = new RenderTargetBitmap(160, 100, dpiX, dpiY, PixelFormats.Pbgra32);
                    bmp.Render(drawingVisual);
                    
                    Image newImage = new Image();
                    newImage.Source = bmp;
                    newImage.Stretch = Stretch.Uniform;
                    newImage.Width = 200;
                    newImage.Height = 120;

                    KinectTileButton button = new KinectTileButton();
                    button.Label = ts.ToString();

                    button.Width = Double.NaN;
                    button.Height = Double.NaN;
                    button.FontSize = 18.0;
                    button.Width = 200;
                    button.Height = 120;
                    button.Background = Brushes.Black;
                    button.FontSize = 12.0;
                    button.Content = newImage;
                    button.Click += (o1, re) =>
                    {
                        mediaElement.Position = ts;
                    };
                    scrollContent.Children.Add(button);
                    mediaPlayer.Pause();
                }
                mediaPlayer.Close();
            });
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
