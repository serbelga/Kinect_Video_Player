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
        public VideoPlayer(String directory)
        {
            InitializeComponent();
            Uri uri = new Uri(directory);
            mediaElement.Source = uri;
            mediaTimeLine.Source = uri;
            // TODO
            //mediaElement.Play();
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
            timeLineSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void MediaTimeChanged(object sender, EventArgs e)
        {
            timeLineSlider.Value = mediaElement.Position.TotalMilliseconds;
        }

        // Jump to different parts of the media (seek to). 
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int) timeLineSlider.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            
            // TODO
            if(mediaElement.Clock.Controller != null)
            {
                mediaElement.Clock.Controller.Seek(ts, TimeSeekOrigin.BeginTime);
            }
            
        }


    }
}
