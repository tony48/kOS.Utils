/* Copyright© (c) 2020 tony48.
  Copyright© (c) 2020 Zoeille.
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

/*
using System;
using KRPC.Service;
using KRPC.Service.Attributes;
using UnityEngine;
using KRPC.SpaceCenter.Services;
using Double = System.Double;
using System.Threading;

namespace Predictor
{
    [KRPCService(Name = "PR", GameScene = GameScene.Flight)]
    public static class Service
    {
        [KRPCProcedure]
        public static KRPC.Utils.Tuple<Double, Double> ImpactPos(KRPC.SpaceCenter.Services.Vessel vessel)
        {
            Class1 pr = new Class1(vessel.InternalVessel);
            CelestialBody body = vessel.InternalVessel.orbit.referenceBody;
            Vector3? impactVect = GetImpactPosition(pr, vessel.InternalVessel);

            if (vessel.Situation.Equals("Flying") || vessel.Situation.Equals(VesselSituation.SubOrbital))
            {
                while (impactVect != null)
                {
                    var worldImpactPos = (Vector3d)impactVect + body.position;
                    var lat = body.GetLatitude(worldImpactPos);
                    var lng = DegreeFix(body.GetLongitude(worldImpactPos), -180);
                    return new KRPC.Utils.Tuple<Double, Double>(lat, lng);
                }
                throw new Exception("No impact pos");
            }
            throw new Exception("Cannot calculate, the vessel is in not in flight. ("+ vessel.Situation+")");
        }

        public static Vector3? GetImpactPosition(Class1 pr, Vessel vessel)
        {


            pr.ComputeTrajectory(vessel, DescentProfile.fetch);
            foreach (Patch patch in pr.Patches)
            {
                if (patch.ImpactPosition != null)
                    return patch.ImpactPosition;
            }
            return null;
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
    }
}
*/