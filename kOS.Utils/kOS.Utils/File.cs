/*
    Copyright (c) 2020 tony48
    This file is part of kOS.Utils.

    kOS.Utils is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    kOS.Utils is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with kOS.Utils.  If not, see <https://www.gnu.org/licenses/>.
*/

using kOS.AddOns;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using UnityEngine;
using kOS.Suffixed;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Proxies;

namespace kOS.Utils
{
    [kOSAddon("FILE")]
    [Safe.Utilities.KOSNomenclature("FILEAddon")]
    public class FileAddon : Suffixed.Addon
    {
        public FileAddon(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("READALLLINES", new OneArgsSuffix<ListValue<StringValue>, StringValue>(ReadLines));
            AddSuffix("READALLTEXT", new OneArgsSuffix<StringValue, StringValue>(ReadText));
        }

        private ListValue<StringValue> ReadLines(StringValue path)
        {
            //Value<StringValue>.CreateList(File.ReadAllLines(KSPUtil.ApplicationRootPath + "Ships/Script/" + path).ToList());
            ListValue<StringValue> lines = new ListValue<StringValue>();
            string[] fileLines = File.ReadAllLines(KSPUtil.ApplicationRootPath + "Ships/Script/" + path);
            for (int i = 0; i < fileLines.Length; i++)
            {
                lines.Add(fileLines[i]);
            }

            return lines;
        }

        private StringValue ReadText(StringValue path)
        {
            return File.ReadAllText(KSPUtil.ApplicationRootPath + "Ships/Script/" + path);
        }

        public override BooleanValue Available()
        {
            return true;
        }
    }
}