using System;
using System.Windows;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using System.IO;
using Kinect.Toolbox;

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

        private SwipeGestureDetector swipeGestureDetector;

        private KinectSensor sensor;

        private VideoPlayer videoPlayer;


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
            //VideoPlayer videoPlayer = new VideoPlayer(button.Tag as String);
            //videoPlayer.Show();
            videoPlayer = new VideoPlayer(button.Tag as String);
            Main.Content = videoPlayer;
            //this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
        }

        private void OnGestureDetectedSwipe(String gesture)
        {
            if (videoPlayer != null)
            {
                if (gesture.Contains("Left")) {
                    videoPlayer.mediaElement.Volume -= 0.1;
                    videoPlayer.Volume.Content = videoPlayer.mediaElement.Volume.ToString();
                }
                else if (gesture.Contains("Right"))
                {
                    videoPlayer.mediaElement.Volume += 0.1;
                    videoPlayer.Volume.Content = videoPlayer.mediaElement.Volume.ToString();
                }
            }
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
                    args.NewSensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
                    swipeGestureDetector = new SwipeGestureDetector();
                    swipeGestureDetector.OnGestureDetected += OnGestureDetectedSwipe;
                    sensor = args.NewSensor;
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

        private void BackOnClick(object sender, RoutedEventArgs e)
        {
            this.Main.Content = null;
        }

        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            this.Main.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }
            if (skeletons.Length != 0)
            {
                foreach (Skeleton skel in skeletons)
                {
                    CatchSwipeGesture(skel);
                }
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void CatchSwipeGesture(Skeleton skeleton)
        {
            
            foreach (Joint joint in skeleton.Joints)
            {
                
                if (joint.JointType == JointType.HandRight && joint.TrackingState == JointTrackingState.Tracked)
                {
                    swipeGestureDetector.Add(joint.Position, sensor);
                }
            }
        }
    }
}
