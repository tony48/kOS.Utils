/*
    Copyright (c) 2020 tony48, 2014 CYBUTEK
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

using kOS.Suffixed;
using System;
using kOS.AddOns;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using UnityEngine;

namespace kOS.Utils
{
    [kOSAddon("IMPACT")]
    [Safe.Utilities.KOSNomenclature("IMPACTAddon")]
    public class Landing : Suffixed.Addon
    {
        public Landing(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("GETIMPACTPOS", new OneArgsSuffix<GeoCoordinates, VesselTarget>(PredictLandingPos));
        }
        
        // Original code from KER
        private GeoCoordinates PredictLandingPos(VesselTarget vessel)
        {
            Vessel internalVessel = vessel.Vessel;
            double latitude = 0.0;
            double longitude = 0.0;
            //Vector3d vector3d = new Vector3d();
            CelestialBody referenceBody = internalVessel.GetOrbit().referenceBody;
            bool impactHappening = false;
            if (internalVessel.situation == Vessel.Situations.LANDED)
                return null;
            double eccentricity = internalVessel.GetOrbit().eccentricity;
            double semiMajorAxis = internalVessel.GetOrbit().semiMajorAxis;
            double r = referenceBody.Radius * 0.9999;
            Vector3d positionFromTrueAnomaly1 = internalVessel.GetOrbit().getRelativePositionFromTrueAnomaly(internalVessel.GetOrbit().trueAnomaly);
            double currentirflong = 180.0 * Math.Atan2(positionFromTrueAnomaly1.x, positionFromTrueAnomaly1.y) / Math.PI;
            double startAngle = internalVessel.GetOrbit().trueAnomaly * 180.0 / Math.PI;
            if (startAngle > 0.0)
                startAngle -= 360.0;
            double endAngle = startAngle + 360.0;
            if (internalVessel.GetOrbit().PeR <= referenceBody.Radius)
            {
                double cosTheta = semiMajorAxis / r / eccentricity - semiMajorAxis * eccentricity / r - 1.0 / eccentricity;
                if (cosTheta < -1.0)
                    cosTheta = -1.0;
                else if (cosTheta > 1.0)
                    cosTheta = 1.0;
                endAngle = -180.0 * Math.Acos(cosTheta) / Math.PI;
                endAngle += Math.Abs(endAngle - startAngle) / 10.0;
            }
            double interval = Math.Abs(endAngle - startAngle) / 36.0;
            int side = 1;
            int it = 0;
            bool ok;
            do
            {
                ok = false;
                ++it;
                for (double impactTheta = startAngle + interval * (double) side; side == 1 ? impactTheta <= endAngle : impactTheta >= endAngle; impactTheta += interval * (double) side)
                {
                    double tA = Math.PI * impactTheta / 180.0;
                    double dtforTrueAnomaly = internalVessel.GetOrbit().GetDTforTrueAnomaly(tA, 0.0);
                    Vector3d positionFromTrueAnomaly2 = internalVessel.GetOrbit().getRelativePositionFromTrueAnomaly(tA);
                    double deltairflong = 180.0 * Math.Atan2(positionFromTrueAnomaly2.x, positionFromTrueAnomaly2.y) / Math.PI - currentirflong;
                    double bodyrot = 360.0 * dtforTrueAnomaly / referenceBody.rotationPeriod;
                    longitude = NormAngle(internalVessel.longitude - deltairflong - bodyrot);
                    latitude = 180.0 * Math.Asin(positionFromTrueAnomaly2.z / positionFromTrueAnomaly2.magnitude) / Math.PI;
                    Vector3d radialVector = QuaternionD.AngleAxis(longitude, Vector3d.down) * QuaternionD.AngleAxis(latitude, Vector3d.forward) * Vector3d.right;
                    double terrainAltitude = referenceBody.pqsController.GetSurfaceHeight(radialVector) - referenceBody.pqsController.radius;
                    double shipalt = positionFromTrueAnomaly2.magnitude - referenceBody.Radius;
                    if (terrainAltitude < 0.0 && referenceBody.ocean)
                        terrainAltitude = 0.0;
                    double delta = shipalt - terrainAltitude;
                    if (side * delta < 0.0)
                    {
                        impactHappening = true;
                        side *= -1;
                        startAngle = impactTheta;
                        endAngle = impactTheta + side * interval;
                        interval = Math.Abs(endAngle - startAngle) / 20.0;
                        endAngle += interval * side;
                        ok = true;
                        break;
                    }
                    if (delta == 0.0)
                    {
                        impactHappening = true;
                        interval = 0.0;
                        ok = true;
                        break;
                    }
                }
            }
            while (ok && (interval > 1E-05 & impactHappening && it < 36));
            return new GeoCoordinates(shared, latitude, longitude);
        }

        private double NormAngle(double ang)
        {
            if (ang > 180.0)
                ang -= 360.0 * Math.Ceiling((ang - 180.0) / 360.0);
            if (ang <= -180.0)
                ang -= 360.0 * Math.Floor((ang + 180.0) / 360.0);
            return ang;
        }

        public override BooleanValue Available()
        {
            return true;
        }
    }
}