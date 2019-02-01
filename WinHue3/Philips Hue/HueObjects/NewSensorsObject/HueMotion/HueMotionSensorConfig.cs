﻿using System.ComponentModel;
using System.Runtime.Serialization;
using WinHue3.Philips_Hue.BridgeObject.BridgeObjects;
using WinHue3.Philips_Hue.HueObjects.Common;
using WinHue3.Utils;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WinHue3.Philips_Hue.HueObjects.NewSensorsObject.HueMotion
{
    [DataContract, ExpandableObject]
    public class HueMotionSensorConfig : ValidatableBindableBase, ISensorConfigBase
    {
        private byte? _battery;
        private bool? _reachable;
        private bool? _on;
        private string _alert;
        private int? _sensitivitymax;
        private int? _sensitivity;

        [DataMember]
        public int? sensitivity
        {
            get => _sensitivity;
            set => SetProperty(ref _sensitivity,value);
        }

        [DataMember]
        public int? sensitivitymax
        {
            get => _sensitivitymax;
            set => SetProperty(ref _sensitivitymax,value);
        }

        /// <summary>
        /// Alert.
        /// </summary>
        [DataMember, ItemsSource(typeof(AlertItemsSource))]
        public string alert
        {
            get => _alert;
            set => SetProperty(ref _alert,value);
        }

        /// <summary>
        /// On off state.
        /// </summary>
        [DataMember]
        public bool? on
        {
            get => _on;
            set => SetProperty(ref _on,value);
        }

        /// <summary>
        /// Sensor reachability.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public bool? reachable
        {
            get => _reachable;
            set => SetProperty(ref _reachable,value);
        }

        /// <summary>
        /// Battery state.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public byte? battery
        {
            get => _battery;
            set => SetProperty(ref _battery,value);
        }

    }
}
