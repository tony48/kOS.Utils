/*
    Copyright (c) 2020 tony48, 2015-2016 djungelorm
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
using System;
using kOS.Suffixed;

namespace kOS.Utils
{
    [kOSAddon("OBT")]
    [Safe.Utilities.KOSNomenclature("OBTAddon")]
    public class Orbital : Suffixed.Addon
    {
        public Orbital(SharedObjects shared) : base(shared)
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
            AddSuffix("TrueAnomalyAtAN", new TwoArgsSuffix<ScalarValue, Orbitable, Orbitable>(TrueAnomalyAtAN));
            AddSuffix("TrueAnomalyAtDN", new TwoArgsSuffix<ScalarValue, Orbitable, Orbitable>(TrueAnomalyAtDN));
            AddSuffix("RelativeInclination", new TwoArgsSuffix<ScalarValue, Orbitable, Orbitable>(RelativeInclination));
        }
        
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

        private ScalarValue TrueAnomalyAtAN(Orbitable obt, Orbitable tgt)
        {
            if (ReferenceEquals(tgt, null))
                throw new ArgumentNullException(nameof(tgt));
            return ClampAngle180(FinePrint.Utilities.OrbitUtilities.AngleOfAscendingNode(obt.Orbit, tgt.Orbit));
        }

        private ScalarValue TrueAnomalyAtDN(Orbitable obt, Orbitable tgt)
        {
            if (ReferenceEquals(tgt, null))
                throw new ArgumentNullException(nameof(tgt));
            return ClampAngle180(FinePrint.Utilities.OrbitUtilities.AngleOfDescendingNode(obt.Orbit, tgt.Orbit));
        }

        private ScalarValue RelativeInclination(Orbitable obt, Orbitable tgt)
        {
            if (ReferenceEquals(tgt, null))
                throw new ArgumentNullException();
            return FinePrint.Utilities.OrbitUtilities.GetRelativeInclination(obt.Orbit, tgt.Orbit);
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

        public double ClampAngle360(double angle)
        {
            angle = angle % 360d;
            if (angle < 0d)
                angle += 360d;
            return angle;
        }

        public double ClampAngle180(double angle)
        {
            angle = ClampAngle360(angle);
            if (angle > 180)
                angle -= 360;
            return angle;
        }
    }
}