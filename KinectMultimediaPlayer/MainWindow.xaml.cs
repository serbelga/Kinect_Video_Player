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

        private TemplatedGestureDetector circleGestureRecognizer;

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
        /// Window Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
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
            videoPlayer = new VideoPlayer(button.Tag as String);
            Main.Content = videoPlayer;
            this.BackButton.Visibility = Visibility.Visible;
            this.Toolbar.Visibility = Visibility.Collapsed;
        }
       
        /// <summary>
        /// Up and down volume
        /// </summary>
        /// <param name="gesture"></param>
        private void OnGestureDetectedSwipe(String gesture)
        {
            if (videoPlayer != null)
            {
                if (gesture.Contains("Left")) {
                    videoPlayer.mediaElement.Volume -= 0.1;
                    videoPlayer.Volume.Content = "Volume: " + videoPlayer.mediaElement.Volume.ToString();
                }
                else if (gesture.Contains("Right"))
                {
                    videoPlayer.mediaElement.Volume += 0.1;
                    videoPlayer.Volume.Content = "Volume: " + videoPlayer.mediaElement.Volume.ToString();
                }
            }
        }

        /// <summary>
        /// Video back when detects circle
        /// </summary>
        /// <param name="gesture"></param>
        private void OnGestureDetectedCircle(String gesture)
        {

            TimeSpan ts = videoPlayer.mediaElement.Position;
            if (!(ts.Seconds < 10 && ts.Minutes == 0))
            {
                ts = new TimeSpan(ts.Days, ts.Hours, ts.Minutes, ts.Seconds - 10);
                videoPlayer.mediaElement.Position = ts;
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
                    using (Stream recordStream = File.Open(Path.Combine(Environment.CurrentDirectory, @"Datos\circleKB.save"), FileMode.Open))
                    {
                        circleGestureRecognizer = new TemplatedGestureDetector("Circle", recordStream);
                        circleGestureRecognizer.OnGestureDetected += OnGestureDetectedCircle;
                    }
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
            this.BackButton.Visibility = Visibility.Collapsed;
            this.Toolbar.Visibility = Visibility.Visible;
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
                foreach (Skeleton skeleton in skeletons)
                {
                    CatchGestures(skeleton);
                }
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void CatchGestures(Skeleton skeleton)
        {
            
            foreach (Joint joint in skeleton.Joints)
            {
                
                if (joint.JointType == JointType.HandRight && joint.TrackingState == JointTrackingState.Tracked)
                {
                    swipeGestureDetector.Add(joint.Position, sensor);
                }
                if (joint.JointType == JointType.HandLeft && joint.TrackingState == JointTrackingState.Tracked)
                {
                    circleGestureRecognizer.Add(joint.Position, sensor);
                }
            }
        }
    }
}
