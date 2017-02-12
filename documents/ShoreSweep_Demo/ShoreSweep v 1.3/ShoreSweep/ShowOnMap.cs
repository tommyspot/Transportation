using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;

// This only works in .NET Framework 4.0 or later and Windows 7 or later.

// If the application is not allowed to use location services,
// the watcher's StatusChanged event never fires.

// Note that the CivicAddressResolver class's ResolveAddress method has not
// yet been implemented so you can't convert this into a street address.

// Add a reference to System.Device.
using System.Device.Location;

// Add a reference to System.Web.
using System.Web;

using Microsoft.Win32;
using System.Collections;
using System.IO;

namespace ShoreSweep
{
    public partial class ShowOnMap : Form
    {
        String lat, longi;
        String [] strArrayList;
        int iIndexTotalImages = 0;
        int iIndexPreviousNext = 0;
        public ShowOnMap()
        {
        }

        public ShowOnMap(String lati, String longit, String [] str)
        {
            InitializeComponent();
            lat = lati;
            longi = longit;
            strArrayList = str;
        }
        // The coordinate watcher.
        private GeoCoordinateWatcher Watcher = null;

        // The watcher's status has change. See if it is ready.
        private void Watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                // Display the latitude and longitude.
                if (Watcher.Position.Location.IsUnknown)
                {
                    lblStatus.Text = "Cannot find location data";
                }
                else
                {
                    GeoCoordinate location =
                        Watcher.Position.Location;
                    
                    string[] splitURL = null;
                    if (null != strArrayList)
                    {
                        foreach (String str in strArrayList)
                        {
                            if(null!=str)
                            splitURL = str.Split(',');

                            using (WebClient webClient = new WebClient())
                            {
                                byte[] data = webClient.DownloadData(splitURL[0]);

                                using (MemoryStream mem = new MemoryStream(data))
                                {
                                    using (var yourImage = Image.FromStream(mem))
                                    {
                                        // If you want it as Jpeg
                                        yourImage.Save("image"+iIndexTotalImages.ToString() +".jpg", ImageFormat.Jpeg);
                                        Thread.Sleep(1000);
                                        pictureBox1.Load("image" + iIndexTotalImages.ToString() + ".jpg");
                                        iIndexTotalImages++;
                                        //  pictureBox1.Image = ImageUtil.ResizeImage(ImageUtil.LoadPicture("image.jpg"), pictureBox1, true);
                                    }
                                }
                            }
                        }
                        int label3text = strArrayList.Count() - 1;
                        label3.Text = label3text.ToString();
                        if(iIndexTotalImages!=0)
                        { 
                            button1.Enabled = true;
                            button2.Enabled = true;
                        }
                        DisplayMap();
                    }
                }
            }
        }

        // Display a map for this location.
        private void DisplayMap()
        {
            // Emulate Internet Explorer 11.
            SetWebBrowserVersion(11001);

            // Display the URL in the WebBrowser control.
            string curDir = Directory.GetCurrentDirectory();
            wbrMap.Url = new Uri(String.Format("file:///{0}/my_html.html", curDir));

            // Hide the label and display the map.
            lblStatus.Hide();
            wbrMap.Show();
        }

        // Return a Google map URL.
        // The URL format is:
        //      http://maps.google.com/maps?q=QUERY&t=TYPE&z=ZOOM
        //  Where:
        //      QUERY is the query. If it begins with "loc:" then its latitude+longitude.
        //      TYPE is map type:
        //          m = Map
        //          k = Satellite
        //          h = Hybrid
        //          p = Terrain
        //          e = Google Earth
        //      ZOOM is the zoom level, usually 1 - 20.

        private string GoogleMapUrl(string query, string map_type, int zoom)
        {
            // Start with the base map URL.
            string url = "http://maps.google.com/maps?";

            // Add the query.
            // If the query starts with "loc:", don't encode.
            if (query.StartsWith("loc:"))
                url += "q=" + query;
            else
                url += "q=" + HttpUtility.UrlEncode(query, Encoding.UTF8);

            // Add the type.
            map_type = GoogleMapTypeCode(map_type);
            if (map_type != null) url += "&t=" + map_type;

            // Add the zoom level.
            if (zoom > 0) url += "&z=" + zoom.ToString();

            return url;
        }

        // Return a Google map type code.
        private string GoogleMapTypeCode(string map_type)
        {
            // Insert the proper type.
            switch (map_type)
            {
                case "Map":
                    return "m";
                case "Satellite":
                    return "k";
                case "Hybrid":
                    return "h";
                case "Terrain":
                    return "p";
                case "Google Earth":
                    return "e";
                default:
                    return null;
            }
        }

        // Make the WebBrowser control emulate the indicated IE version.
        // See:
        //      https://msdn.microsoft.com/library/ee330730.aspx#browser_emulation
        private void SetWebBrowserVersion(int ie_version)
        {
            // For testing:
            //DeleteRegistryValue(key64bit, app_name);
            //DeleteRegistryValue(key32bit, app_name);

            const string key64bit =
                @"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION";
            const string key32bit =
                @"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION";
            string app_name = System.AppDomain.CurrentDomain.FriendlyName;

            // You can do both if you like.
            //SetRegistryDword(key64bit, app_name, ie_version);
            SetRegistryDword(key32bit, app_name, ie_version);
        }

        // Set a registry DWORD value.
        private void SetRegistryDword(string key_name, string value_name, int value)
        {
            // Open the key.
            RegistryKey key =
                Registry.CurrentUser.OpenSubKey(key_name, true);
            if (key == null)
                key = Registry.CurrentUser.CreateSubKey(key_name,
                    RegistryKeyPermissionCheck.ReadWriteSubTree);

            // Set the desired value.
            key.SetValue(value_name, value, RegistryValueKind.DWord);

            key.Close();
        }


        // Create and start the watcher.

        private void ShowOnMap_Load(object sender, EventArgs e)
        {
            // Create the watcher.
            Watcher = new GeoCoordinateWatcher();

            // Catch the StatusChanged event.
            Watcher.StatusChanged += Watcher_StatusChanged;

            // Start the watcher.
            Watcher.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (iIndexPreviousNext <= strArrayList.Count()-1)
            {
                
                iIndexPreviousNext--;
                if (iIndexPreviousNext < 0)
                {
                    iIndexPreviousNext = 0;
                }
                pictureBox1.Load("image" + iIndexPreviousNext.ToString() + ".jpg");
                
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (iIndexPreviousNext < strArrayList.Count()-1)
            {
                iIndexPreviousNext++;
                pictureBox1.Load("image" + iIndexPreviousNext.ToString() + ".jpg");
            }
        }

        private void form_close(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            for (int i = iIndexTotalImages-1; i >= 0; i--)
            {
                File.SetAttributes("image" + i.ToString() + ".jpg", FileAttributes.Normal);
                File.Delete("image" + i.ToString() + ".jpg");
            }

            File.Delete("LatLong.txt");
            

        }

        // Delete a registry value.
        private void DeleteRegistryValue(string key_name, string value_name)
        {
            // Open the key.
            RegistryKey key =
                Registry.CurrentUser.OpenSubKey(key_name, true);
            if (key == null) return;

            // Delete the desired value.
            key.DeleteValue(value_name, false);

            key.Close();
        }
    }
}

