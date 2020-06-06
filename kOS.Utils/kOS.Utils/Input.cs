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
using kOS.Safe.Exceptions;
using UnityEngine;

namespace kOS.Utils
{
    [kOSAddon("INPUT")]
    [Safe.Utilities.KOSNomenclature("INPUTAddon")]
    public class InputAddon : Suffixed.Addon
    {
        private bool isEngaged = false;
        public InputAddon(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("GetKey", new OneArgsSuffix<BooleanValue, StringValue>(GetKey));
            AddSuffix("Engage", new NoArgsVoidSuffix(Engage));
            AddSuffix("Disengage", new NoArgsVoidSuffix(Disengage));
        }
        
        public void Engage()
        {
            InputLockManager.SetControlLock(ControlTypes.All, "kOSInput");
            isEngaged = true;
        }

        public void Disengage()
        {
            InputLockManager.RemoveControlLock("kOSInput");
            isEngaged = false;
        }
        
        // List of key strings here : https://gist.github.com/C0DEF52/b1168e6ed3d1f567fc919f2942037bab
        private BooleanValue GetKey(StringValue kc)
        {
            if (!isEngaged)
                throw new KOSException("Please lock the input by doing engage before using GetKey");
            return Input.GetKey(kc);
        }

        public override BooleanValue Available()
        {
            return true;
        }
    }
}