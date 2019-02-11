using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
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
            mediaElement.Play();
        }

        private void Element_MediaEnded(object sender, RoutedEventArgs e)
        {
            //TO DO : Hide video controls + Show Restart and Back
        }

        private void Element_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void BackOnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            MessageBox.Show(args.NewSensor == null ? "No Kinect" : args.NewSensor.Status.ToString());
            bool error = false;
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    error = true;
                }
            }
            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();
                }
                catch (InvalidOperationException)
                {
                    error = true;
                }
            }
            if (!error)
            {
                kinectRegion.KinectSensor = args.NewSensor;
            }
        }
    }
}
