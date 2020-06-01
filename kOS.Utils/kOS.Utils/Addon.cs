using kOS.AddOns;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using UnityEngine;
using System;
using kOS.Suffixed;

namespace kOS.Utils
{
    [kOSAddon("SCREEN")]
    [Safe.Utilities.KOSNomenclature("SCREENAddon")]
    public class Addon : Suffixed.Addon
    {
        public Addon(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("MeanAnomalyAtUT", new TwoArgsSuffix<ScalarValue, Orbitable, ScalarValue>(MeanAnomalyAtUT));
            AddSuffix("RadiusAtTrueAnomaly", new TwoArgsSuffix<ScalarValue, Orbitable, ScalarValue>(RadiusAtTrueAnomaly));
            AddSuffix("TrueAnomalyAtRadius", new TwoArgsSuffix<ScalarValue, Orbitable, ScalarValue>(TrueAnomalyAtRadius));
            AddSuffix("TrueAnomalyAtUT", new TwoArgsSuffix<ScalarValue, Orbitable, ScalarValue>(TrueAnomalyAtUT));
            AddSuffix("UTAtTrueAnomaly", new TwoArgsSuffix<ScalarValue, Orbitable, ScalarValue>(UTAtTrueAnomaly));
            AddSuffix("EccentricAnomalyAtUT", new TwoArgsSuffix<ScalarValue, Orbitable, ScalarValue>(EccentricAnomalyAtUT));

            
        }

        // ReSharper disable once InconsistentNaming
        private ScalarValue MeanAnomalyAtUT(Orbitable obt, ScalarValue ut)
        {
            var percent = obt.Orbit.getObtAtUT(ut) / shared.Vessel.orbit.period;
            return RadToDeg(percent * (2 * Math.PI));
        }
        
        private ScalarValue RadiusAtTrueAnomaly(Orbitable obt, ScalarValue trueAnomaly)
        {
            return obt.Orbit.RadiusAtTrueAnomaly(DegToRad(trueAnomaly));
        }

        private ScalarValue TrueAnomalyAtRadius(Orbitable obt, ScalarValue radius)
        {
            return RadToDeg(obt.Orbit.TrueAnomalyAtRadius(radius));
        }
        
        private ScalarValue TrueAnomalyAtUT(Orbitable obt, ScalarValue ut)
        {
            return RadToDeg(obt.Orbit.TrueAnomalyAtUT(ut));
        }
        
        private ScalarValue UTAtTrueAnomaly(Orbitable obt, ScalarValue trueAnomaly)
        {
            return obt.Orbit.GetUTforTrueAnomaly(DegToRad(trueAnomaly), 0);
        }
        
        private ScalarValue EccentricAnomalyAtUT(Orbitable obt, ScalarValue ut)
        {
            return RadToDeg(obt.Orbit.EccentricAnomalyAtUT(ut));
        }

        public override BooleanValue Available()
        {
            return true;
        }

        public double RadToDeg(double angle)
        {
            return angle * 180 / Math.PI;
        }

        public double DegToRad(double angle)
        {
            return angle * Math.PI / 180;
        }
    }
}