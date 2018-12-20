﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using Newtonsoft.Json;
using WinHue3.Philips_Hue.Communication;


namespace WinHue3.Utils
{
    public static class UpdateManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string UPDATE_URL = "https://raw.githubusercontent.com/Hyrules/WinHue3/master/update.md";
        //private const string UPDATE_URL = "https://raw.githubusercontent.com/Hyrules/WinHue3/dev/update.md";
        private static bool _updateAvailable;
        private static Update _update;
        private static readonly WebClient wc;

        public static bool UpdateAvailable
        {
            get => _updateAvailable;
            set => _updateAvailable = value;
        }

        static UpdateManager()
        {
            wc = new WebClient();
            wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
            UpdateAvailable = false;
        }

        public static bool CheckForWinHueUpdate()
        {
            log.Info("Checking for WinHue 3 update...");
            CommResult data = Comm.SendRequest(new Uri(UPDATE_URL), WebRequestType.Get);
            if (data.Status == WebExceptionStatus.Success)
            {
                _update = JsonConvert.DeserializeObject<Update>(data.Data);
            }
            if (_update == null) return false;
            try
            {
                UpdateAvailable = Assembly.GetExecutingAssembly().GetName().Version < new Version(_update.Version);
            }
            catch (Exception)
            {
                log.Error("Unable to parse new version. Please try again later.");
                UpdateAvailable = false;
            }
            
            return UpdateAvailable;
        }

        public static bool CheckBridgeNeedUpdate(string actualversion)
        {
            try
            {
                return new Version(_update.BridgeVersion) > new Version(actualversion);
            }
            catch (Exception)
            {
                return false;
            }

            
        }

        public static void DownloadUpdate()
        {
            if (UpdateAvailable)
            {


                log.Info($"Downloading update for WinHue 3 {_update.Version}");
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string savepath = Path.Combine(path, "WinHue\\Temp");
                string filepath = Path.Combine(savepath, _update.Filename);
                try
                {
                    if (!Directory.Exists(savepath))
                    {
                        Directory.CreateDirectory(savepath);
                    }
                    else
                    {
                        if (!File.Exists(filepath))
                        {
                            DirectoryInfo di = new DirectoryInfo(savepath);

                            foreach (FileInfo fi in di.GetFiles())
                            {

                                fi.Delete();
                            }
                        }
                        else
                        {
                            log.Info("File already downloaded not downloading again.");
                            DoUpdate(filepath);
                        }
                    }
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    
                    wc.DownloadFileAsync(new Uri(_update.Url), filepath,filepath);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.Error("Unable to create temp save path for update");
                }
            }
            else
            {
                log.Warn("No Update available to download.");
            }
        }

        private static void DoUpdate(string path)
        {
            if (MessageBox.Show(GlobalStrings.NewUpdateDownloaded, GlobalStrings.Warning, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            Application.Current.Shutdown();
        }

        private static void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null) return;
            DoUpdate(e.UserState.ToString());
        }

        
    }

    public class Update : ValidatableBindableBase
    {
        private string _version;
        private string _URL;
        private string _bridgeversion;

        public string Url
        {
            get => _URL;
            set => SetProperty(ref _URL,value);
        }

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version,value);
        }

        public string BridgeVersion
        {
            get => _bridgeversion;
            set => SetProperty(ref _bridgeversion, value);
        }

        public string Filename => !string.IsNullOrEmpty(_URL) ? Path.GetFileName(new Uri(_URL).LocalPath) : string.Empty;
    }
}
