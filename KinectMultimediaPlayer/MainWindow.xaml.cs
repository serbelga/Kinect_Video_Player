using System;
using System.Windows;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using System.IO;

namespace KinectMultimediaPlayer
{
    /// <summary>
    /// Interaction for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensorChooser sensorChooser;

        /// <summary>
        /// List of FileInfo. Videos in Assets folder
        /// </summary>
        public FileInfo[] videos;
         
        /// <summary>
        /// Class constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DirectoryInfo directoryInfo = new DirectoryInfo(@"Assets");
            videos = directoryInfo.GetFiles("*.mp4");
            BuildListView();
        }

        /// <summary>
        /// Builds the ListView
        /// </summary>
        private void BuildListView()
        {
            for (int i = 0; i < videos.Length; i++)
            {
                KinectTileButton button = new KinectTileButton();
                button.Label = videos[i].Name;
                button.Width = Double.NaN;
                button.Height = Double.NaN;
                button.FontSize = 18.0;
                button.Tag = videos[i].FullName.ToString();

                scrollContent.Children.Insert(0, button);
                button.Click += ScrollButtonClick;
            }
        }

        /// <summary>
        /// List View button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollButtonClick(object sender, RoutedEventArgs e)

        {
            KinectTileButton button = (KinectTileButton) sender;
            VideoPlayer videoPlayer = new VideoPlayer(button.Tag as String);
            videoPlayer.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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
