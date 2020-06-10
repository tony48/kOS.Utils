/*
  Copyright© (c) 2020 tony48.
  This file is part of Predictor.
  Predictor is available under the terms of GPL-3.0-or-later.
  See the LICENSE.md file for more details.
  Predictor is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
  Predictor is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
  You should have received a copy of the GNU General Public License
  along with Predictor.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using kOS.AddOns;
using kOS.AddOns.TrajectoriesAddon;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Exceptions;
using kOS.Safe.Utilities;
using kOS.Suffixed;
using UnityEngine;
using Predictor;

namespace kOS.Predictor
{
    [kOSAddon("PR")]
    [KOSNomenclature("PRAddon")]
    public class Addon : Suffixed.Addon
    {
        //private List<KeyValuePair<Guid, Class1>> predictors = new List<KeyValuePair<Guid, Class1>>();
        //private bool isEngaged;

        public Addon(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("IMPACTPOS", new OneArgsSuffix<GeoCoordinates, VesselTarget>(ImpactPos));
            AddSuffix("HASIMPACT", new OneArgsSuffix<BooleanValue, VesselTarget>(HasImpact));
            //AddSuffix("ENGAGE", new OneArgsSuffix<VesselTarget>(Engage));
            //AddSuffix("DISENGAGE", new OneArgsSuffix<VesselTarget>(Disengage));
        }

        private BooleanValue HasImpact(VesselTarget vessel)
        {
            return GetImpactPosition(vessel).HasValue;
        }

        private GeoCoordinates ImpactPos(VesselTarget vessel)
        {
            //Class1 pr = new Class1(vessel.Vessel);
            //if(!isEngaged)
            //    throw new KOSException("please use engage");
            CelestialBody body = vessel.Vessel.orbit.referenceBody;
            Vector3? impactVect = GetImpactPosition(vessel);
            if (impactVect != null)
            {
                var worldImpactPos = (Vector3d)impactVect + body.position;
                var lat = body.GetLatitude(worldImpactPos);
                var lng = DegreeFix(body.GetLongitude(worldImpactPos), -180);
                return new GeoCoordinates(vessel.Shared, lat, lng);
            }
            throw new KOSException("Impact position is null");
        }
        
        public static double DegreeFix(double inAngle, double rangeStart)
        {
            double rangeEnd = rangeStart + 360.0;
            double outAngle = inAngle;
            while (outAngle > rangeEnd)
                outAngle -= 360.0;
            while (outAngle < rangeStart)
                outAngle += 360.0;
            return outAngle;
        }

        //private void Engage(VesselTarget vessel)
        //{
        //    predictors.Add(new KeyValuePair<Guid, Class1>(vessel.Vessel.id, new Class1(vessel.Vessel)));
         //   isEngaged = true;
        //}

        //private void Disengage(VesselTarget vessel)
        //{
        //    predictors.Find(kv => kv.Key == vessel.Vessel.id).Value.StopThread();
        //    predictors.Remove(new KeyValuePair<Guid, Class1>(vessel.Vessel.id, new Class1(vessel.Vessel)));
        //    isEngaged = false;
        //}
        
        public Vector3? GetImpactPosition(VesselTarget vessel)
        {
            Class1 pr = new Class1(vessel.Vessel);
            pr.ComputeTrajectory(vessel.Vessel, DescentProfile.fetch);
            foreach (Patch patch in pr.Patches)
            {
                if (patch.ImpactPosition != null)
                    return patch.ImpactPosition;
            }
            return null;
        }

        public override BooleanValue Available()
        {
            return true;
        }
    }
}
