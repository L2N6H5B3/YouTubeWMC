using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouTubeWMC {
    public partial class YouTubeTV : Form {

        // Create YouTube TV URL
        private static string youtubeTvUrl = "https://youtube.com/tv";
        // Create User Agent
        private static string userAgent = "Mozilla/5.0 (SMART-TV; Linux; Tizen 4.0.0.2) AppleWebkit/605.1.15 (KHTML, like Gecko) SamsungBrowser/9.2 TV Safari/605.1.15";

        public YouTubeTV() {
            // Initialise
            InitializeComponent();

            // Configure Form Sizing
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.chromiumWebBrowser1.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            // Create CefSharp Settings
            var settings = new CefSettings();
            // Set Background Colour
            settings.BackgroundColor = 0xff000000;
            // Set User Agent
            settings.UserAgent = userAgent;
            // Set Browser Cache Path
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\YouTubeWMC\\WebCache";
            // Initialise CefSharp
            Cef.Initialize(settings);
            // Set YouTube TV Url
            chromiumWebBrowser1.Load(youtubeTvUrl);
        }
    }
}
