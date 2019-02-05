﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WinHue3.Functions.Application_Settings.Settings;
using WinHue3.Philips_Hue.BridgeObject;
using WinHue3.Philips_Hue.HueObjects.SceneObject;
using WinHue3.Utils;

namespace WinHue3.Functions.Sensors.HueTap
{
    /// <summary>
    /// Interaction logic for Form_HueTapConfig.xaml
    /// </summary>
    public partial class Form_HueTapConfig : Window
    {
        HueTapConfigViewModel tcvm;
        public Form_HueTapConfig()
        {

            InitializeComponent();
            tcvm = DataContext as HueTapConfigViewModel;  
        }

        public async Task Initialize(string sensorid)
        {
            tcvm.HueTapModel.Id = sensorid;

            List<Scene> hr = await HueObjectHelper.GetBridgeScenesAsyncTask(BridgeManager.SelectedBridge);

            if (hr != null)
            {

                if (WinHueSettings.settings.ShowHiddenScenes)
                {
                    tcvm.HueTapModel.ListScenes = hr;
                }
                else
                {
                    List<Scene> temp = hr;
                    temp = temp.Where(x => !x.name.StartsWith("HIDDEN")).ToList();
                    tcvm.HueTapModel.ListScenes = temp;
                }

            }
            else
            {
                BridgeManager.SelectedBridge.ShowErrorMessages();
            }
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

    }
}
