using System;
using System.Windows;

namespace KinectMultimediaPlayer
{
    /// <summary>
    /// Interaction for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="directory">Path to the video file</param>
        public VideoPlayer(String directory)
        {
            InitializeComponent();
            Uri uri = new Uri(directory);
            mediaElement.Source = uri;
            mediaElement.Play();
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
        }

        private void Element_MediaEnded(object sender, RoutedEventArgs e)
        {

        }

        private void Element_MediaOpened(object sender, RoutedEventArgs e)
        {

        }
    }
}
