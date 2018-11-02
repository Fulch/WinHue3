﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WinHue3.Philips_Hue.Communication;

namespace WinHue3.Philips_Hue.HueObjects.LightObject
{
    public class LightControlCt
    {
        [DataMember(EmitDefaultValue = false, IsRequired = false), Category("Light Control CT"), Description("Min"), ReadOnly(true)]
        public ushort min { get; set; }
        [DataMember(EmitDefaultValue = false, IsRequired = false), Category("Light Control CT"), Description("Max"), ReadOnly(true)]
        public ushort max { get; set; }

        /// <summary>
        /// To string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Serializer.SerializeToJson(this);
        }
    }
}
