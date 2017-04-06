using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Audio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LED_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public AsusVgaAura.AsusVgaAuraWrapper a = new AsusVgaAura.AsusVgaAuraWrapper();
        public int MILLI_DELAY = 0;
        public bool killFade = true;

        public MainPage()
        {
            this.InitializeComponent();

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            killFade = true;
            string selected = this.comboBox.SelectedValue.ToString();
            if (selected == "Off")
            {
                a.DisableLights();
            }
            else if (selected == "Red")
            {
                a.SetColor(255, 0, 0);
            }
            else if (selected == "Green")
            {
                a.SetColor(0, 255, 0);
            }
            else if (selected == "Blue")
            {
                a.SetColor(0, 0, 255);
            }
        }

        private void SpecFadeBtn_Click(object sender, RoutedEventArgs e)
        {
            killFade = true;
            a.SetCrossFade();
        }

        private void ColorPickBtn_Click(object sender, RoutedEventArgs e)
        {
            killFade = true;
        }

        async System.Threading.Tasks.Task PutTaskDelay()
        {
            await System.Threading.Tasks.Task.Delay(MILLI_DELAY);
        }

        private async void RedFade_Click(object sender, RoutedEventArgs e)
        {
            killFade = false;
            byte red = 0;
            while (!killFade)
            {
                while (red < 255)
                {
                    a.SetColor(red += 1, 0, 0);
                    Log("Going Up. red = " + red);
                    await PutTaskDelay();
                    if (killFade) break;
                }
                while (red > 0)
                {
                    a.SetColor(red -= 1, 0, 0);
                    Log("Going Down. red = " + red);
                    await PutTaskDelay();
                    if (killFade) break;
                }
            }
        }

        private static void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now + ": " + message);
        }

        private static void OnTimerElapsed(object state)
        {
            Log("Timer Elapsed.");
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            MILLI_DELAY = (int)RedFadeTimer.Value;
        }

        private async void Audio_Click(object sender, RoutedEventArgs e)
        {
            AudioNodeListener listener = new AudioNodeListener();
            killFade = false;
            while (!killFade)
            {
                byte r = (byte)listener.DopplerVelocity.X;
                byte g = (byte)listener.DopplerVelocity.Y;
                byte b = (byte)listener.DopplerVelocity.Z;
                Log(listener.Position.X + "");
                Log(listener.Orientation.X + "");
                Log("R: " + r + ", G: " + g + ", B: " + b);
                await PutTaskDelay();
            }
        }
    }
}
