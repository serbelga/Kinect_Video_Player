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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect;

namespace KinectMultimediaPlayer
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensorChooser sensorChooser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
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
                    // KinectSensor entra en estado invalid mientras se habilitan/deshabilitan
                    // streams.
                    // Ej.: Se desconecta sensor de forma abrupta.
                    error = true;
                }
            }
            if (args.NewSensor != null)
            {
                try
                {
                    //ToDo: Habilite el stream de profundidad con formato Resolution640x480Fps30
                    //ToDo: Habilite el stream de esqueletos
                    //Hint: Utilice args.NewSensor
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

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            
            mediaElement.Play();
        }

        private void PauseOnClick(object sender, RoutedEventArgs e)
        {

            mediaElement.Pause();
        }

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
