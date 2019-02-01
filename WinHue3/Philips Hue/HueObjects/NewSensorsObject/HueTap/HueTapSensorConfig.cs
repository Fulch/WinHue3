﻿using System.Runtime.Serialization;
using WinHue3.Philips_Hue.HueObjects.Common;
using WinHue3.Utils;

namespace WinHue3.Philips_Hue.HueObjects.NewSensorsObject.HueTap
{
    [DataContract]
    public class HueTapSensorConfig : ValidatableBindableBase, ISensorConfigBase
    {
        private bool? _on;

        /// <summary>
        /// On off state.
        /// </summary>
        [DataMember]
        public bool? on
        {
            get => _on;
            set => SetProperty(ref _on,value);
        }

    }
}
