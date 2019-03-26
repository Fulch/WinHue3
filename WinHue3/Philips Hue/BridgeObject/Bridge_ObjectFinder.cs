﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WinHue3.ExtensionMethods;
using WinHue3.Philips_Hue.BridgeObject.BridgeMessages;
using WinHue3.Philips_Hue.Communication;
using WinHue3.Philips_Hue.HueObjects;

namespace WinHue3.Philips_Hue.BridgeObject
{
    public partial class Bridge
    {
        public async Task<bool> StartNewObjectsSearchAsyncTask(Type objecttype)
        {            
            string typename = objecttype.Name.ToLower() + "s";
            
            CommResult comres = await Comm.SendRequestAsyncTask(new Uri(BridgeUrl + $"/{typename}"), WebRequestType.Post);

            if (comres.Status == WebExceptionStatus.Success)
            {
                LastCommandMessages.AddMessage(Serializer.DeserializeToObject<List<IMessage>>(comres.Data));
                return true;
            }
            ProcessCommandFailure(BridgeUrl + $"/{typename}", comres.Status);
            return false;
        }

        public async Task<bool> TouchLink()
        {
            CommResult comres = await Comm.SendRequestAsyncTask(new Uri(BridgeUrl + "/config"), WebRequestType.Put,"{\"touchlink\":true}");
            if (comres.Status == WebExceptionStatus.Success)
            {
                LastCommandMessages.AddMessage(Serializer.DeserializeToObject<List<IMessage>>(comres.Data));
                return true;
            }
            ProcessCommandFailure(BridgeUrl + "/config", comres.Status);
            return false;
        }

        public async Task<bool> FindNewLightsAsync(string serialslist = null)
        {
            LightSearchSerial lsl = new LightSearchSerial();

            if (serialslist != null)
            {
                string[] serials = serialslist.Split(',');

                foreach (string s in serials)
                {
                    lsl.deviceid.Add(s);
                }

            }

            CommResult comres = await Comm.SendRequestAsyncTask(new Uri(BridgeUrl + $"/lights"), WebRequestType.Post, lsl.deviceid.Count == 0 ? "" : Serializer.SerializeJsonObject(lsl));

            if (comres.Status == WebExceptionStatus.Success)
            {
                LastCommandMessages.AddMessage(Serializer.DeserializeToObject<List<IMessage>>(comres.Data));
                return true;
            }
            ProcessCommandFailure(BridgeUrl + $"/lights", comres.Status);
            return false;
        }
    }
}
