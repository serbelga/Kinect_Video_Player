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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KinectMultimediaPlayer
{
    /// <summary>
    /// Lógica de interacción para VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {
        public VideoPlayer(String directory)
        {
            InitializeComponent();
            Uri uri = new Uri(directory);
            mediaElement.Source = uri;
            mediaElement.Play();
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
