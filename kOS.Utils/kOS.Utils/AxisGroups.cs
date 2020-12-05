/*using kOS.AddOns;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;
using UnityEngine;

namespace kOS.Utils
{
    [kOSAddon("AXIS")]
    [KOSNomenclature("AXISAddon")]
    public class AxisGroups : Addon
    {
        public AxisGroups(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("THROTTLE", new SetSuffix<ScalarValue>(() => FlightGlobals.ActiveVessel.ctrlState.mainThrottle, value => FlightGlobals.ActiveVessel.ctrlState.mainThrottle = value));
            
        }

        public override BooleanValue Available()
        {
            return true;
        }
        
        
    }
}*/