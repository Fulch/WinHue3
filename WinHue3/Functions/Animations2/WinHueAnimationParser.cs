﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using WinHue3.Philips_Hue.HueObjects.Common;
using WinHue3.Philips_Hue.HueObjects.LightObject;

namespace WinHue3.Functions.Animations2
{
    public static class WinHueAnimationParser
    {
        private static readonly Parser<char> SpaceDelimiter = Parse.WhiteSpace;
        private static readonly Parser<char> LineDelimiter = Parse.Char(';');

        private static readonly Parser<int> WaitCommand =
            from wait in Parse.String("WAIT")
            from sep in Parse.Char(':')
            from value in Parse.Digit.DelimitedBy(LineDelimiter).Text()
            select Convert.ToInt32(value);

        private static readonly Parser<Tuple<string, string>> setter =
            from typeword in (Parse.String("LIGHT").Or(Parse.String("GROUP"))).Text()
            from space in SpaceDelimiter
            from id in Parse.Digit.DelimitedBy(SpaceDelimiter).Text()
            select new Tuple<string, string>(typeword, id);

        private static readonly Parser<KeyValuePair<string, string>> property =
            from prop in (
                Parse.String("BRI")
                .Or(Parse.String("SAT")               
                .Or(Parse.String("CT")
                .Or(Parse.String("HUE")
                .Or(Parse.String("ON")))))).Text()
            from sep in Parse.Char(':')
            from val in Parse.Digit.Many().Text()
            select new KeyValuePair<string, string>(prop, val);

        private static readonly Parser<Dictionary<string, string>> properties =
            from prop in property.DelimitedBy(SpaceDelimiter)
            from lineend in LineDelimiter
            select new Dictionary<string, string>(prop.ToDictionary(x => x.Key,x => x.Value));

        private static readonly Parser<IHueObject> hueobject =
            from set in Parse.String("SET")
            from space1 in SpaceDelimiter
            from type in setter
            from space2 in SpaceDelimiter
            from to in Parse.String("TO")
            from space3 in SpaceDelimiter
            from props in properties
            select CreateHueObject(type,props);

        private static IHueObject CreateHueObject(Tuple<string,string> type ,Dictionary<string,string> properties)
        {
            IHueObject ho = HueObjectCreator.CreateHueObject(type.Item1);
            ho.Id = type.Item2;
            PropertyInfo[] pi = typeof(State).GetProperties();

            foreach(KeyValuePair<string,string> kvp in properties)
            {

            }
            return ho;
        }

        public static object ParseAnimation(string text)
        {
        
            return hueobject.Parse(text);
        }
    }
}