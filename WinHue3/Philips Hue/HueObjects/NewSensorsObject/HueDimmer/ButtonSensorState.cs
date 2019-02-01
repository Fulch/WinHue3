﻿using System.ComponentModel;
using System.Runtime.Serialization;
using WinHue3.Philips_Hue.HueObjects.Common;
using WinHue3.Utils;

namespace WinHue3.Philips_Hue.HueObjects.NewSensorsObject.HueDimmer
{
    /// <summary>
    /// Hue Tap Sensor State.
    /// </summary>
    [DataContract]
    public class ButtonSensorState : ValidatableBindableBase, ISensorStateBase
    {
        private int? _buttonevent;

        /// <summary>
        /// Button event number.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public int? buttonevent
        {
            get => _buttonevent;
            set => SetProperty(ref _buttonevent,value);
        }

        private string _lastupdated;

        [DataMember, ReadOnly(true)]
        public string lastupdated
        {
            get => _lastupdated;
            set => SetProperty(ref _lastupdated, value);
        }

    }
}
