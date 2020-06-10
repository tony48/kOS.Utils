/*
  Copyright© (c) 2014-2017 Youen Toupin, (aka neuoy).
  Copyright© (c) 2014-2018 A.Korsunsky, (aka fat-lobyte).
  Copyright© (c) 2017-2018 S.Gray, (aka PiezPiedPy).
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

using UnityEngine;
using System;

namespace Predictor
{
    public sealed class DescentProfile: MonoBehaviour
    {
        public class Node
        {
            public string Name { get; private set; }
            public string Description { get; private set; }
            public string Horizon_txt { get; private set; }
            public string Angle_txt { get; private set; }
            private bool horizon;       // If true, angle is relative to horizon, otherwise it's relative to velocity (i.e. angle of attack)
            private double angle_rad;   // In radians
            private double angle_deg;   // In degrees
            private float sliderPos;

            public bool Horizon
            {
                get => horizon;
                set
                {
                    horizon = value;
                    Horizon_txt = value ? "Horiz" : "AoA";
                }
            }

            public double AngleRad
            {
                get => angle_rad;
                set
                {
                    if (Math.Abs(value) < 0.00001)
                        angle_rad = 0d;
                    else
                        angle_rad = value;

                    angle_deg = angle_rad * (180.0 / Math.PI);
                    if (angle_deg <= -100d || angle_deg >= 100d)
                        Angle_txt = angle_deg.ToString("F1") + "°";
                    else if (angle_deg <= -10d || angle_deg >= 10d)
                        Angle_txt = angle_deg.ToString("F2") + "°";
                    else
                        Angle_txt = angle_deg.ToString("F3") + "°";
                }
            }

            public double AngleDeg
            {
                get => angle_deg;
                set
                {
                    if (Math.Abs(value) < 0.00001)
                        angle_deg = 0d;
                    else
                        angle_deg = value;

                    angle_rad = angle_deg * (Math.PI / 180.0);
                    if (angle_deg <= -100d || angle_deg >= 100d)
                        Angle_txt = angle_deg.ToString("F1") + "°";
                    else if (angle_deg <= -10d || angle_deg >= 10d)
                        Angle_txt = angle_deg.ToString("F2") + "°";
                    else
                        Angle_txt = angle_deg.ToString("F3") + "°";
                }
            }

            public float SliderPos
            {
                get => sliderPos;
                set
                {
                    sliderPos = value;
                    AngleRad = value * value * value * Math.PI; // This helps to have high precision near 0° while still allowing big angles
                }
            }

            //  constructor
            public Node(string name, string description)
            {
                Name = name;
                Description = description;
            }

            public void RefreshSliderPos()
            {
                float position = (float)Math.Pow(Math.Abs(AngleRad) / Math.PI, 1d / 3d);
                if (AngleRad < 0d)
                    sliderPos = -position;
                else
                    sliderPos = position;
            }

            public double GetAngleOfAttack(Vector3d position, Vector3d velocity)
            {
                if (!Horizon)
                    return AngleRad;

                return Math.Acos(Vector3d.Dot(position, velocity) / (position.magnitude * velocity.magnitude)) - Math.PI * 0.5 + AngleRad;
            }
        }

        public Node entry;
        public Node highAltitude;
        public Node lowAltitude;
        public Node finalApproach;

        public bool ProgradeEntry { get; set; }

        public bool RetrogradeEntry { get; set; }

        private Vessel attachedVessel;

        // permit global access
        public static DescentProfile fetch { get; private set; } = null;

        //  constructors
        public DescentProfile()
        {
            fetch = this;
            Allocate();
        }

        // Awake is called only once when the script instance is being loaded. Used in place of the constructor for initialization.
        public void Awake()
        {
            if (Settings.DefaultDescentIsRetro)
                Reset();
            else
                Reset(0d);
        }

        public void OnDestroy()
        {
            fetch = null;
            entry = null;
            highAltitude = null;
            lowAltitude = null;
            finalApproach = null;
        }

        private void Allocate()
        {
            entry = new Node("Entry", "");
            highAltitude = new Node("High", "");
            lowAltitude = new Node("Low", "");
            finalApproach = new Node("Ground", "");
        }

        public void Reset(double AoA = Math.PI)
        {
            //Debug.Log(string.Format("Resetting vessel descent profile to {0} degrees", AoA));
            entry.AngleRad = AoA;
            entry.Horizon = false;
            highAltitude.AngleRad = AoA;
            highAltitude.Horizon = false;
            lowAltitude.AngleRad = AoA;
            lowAltitude.Horizon = false;
            finalApproach.AngleRad = AoA;
            finalApproach.Horizon = false;

            ProgradeEntry = AoA == 0d;
            RetrogradeEntry = AoA == Math.PI;

            RefreshSliders();
        }

        private void RefreshSliders()
        {
            entry.RefreshSliderPos();
            highAltitude.RefreshSliderPos();
            lowAltitude.RefreshSliderPos();
            finalApproach.RefreshSliderPos();
        }

        public void Update()
        {
            
        }

        /*public void CheckGUI()
        {
            double? AoA = entry.Horizon ? (double?)null : entry.AngleRad;

            if (highAltitude.AngleRad != AoA || highAltitude.Horizon)
                AoA = null;
            if (lowAltitude.AngleRad != AoA || lowAltitude.Horizon)
                AoA = null;
            if (finalApproach.AngleRad != AoA || finalApproach.Horizon)
                AoA = null;

            if (!AoA.HasValue)
            {
                ProgradeEntry = false;
                RetrogradeEntry = false;
            }
            else
            {
                if (Math.Abs(AoA.Value) < 0.00001)
                    ProgradeEntry = true;
                if (Math.Abs((Math.Abs(AoA.Value) - Math.PI)) < 0.00001)
                    RetrogradeEntry = true;
            }
        }*/

        /// <summary>
        /// Computes the angle of attack to follow the current profile if the aircraft is at the specified position
        /// (in world frame, but relative to the body) with the specified velocity
        /// (relative to the air, so it takes the body rotation into account)
        /// </summary>
        public double GetAngleOfAttack(CelestialBody body, Vector3d position, Vector3d velocity)
        {
            double altitude = position.magnitude - body.Radius;
            double altitudeRatio = body.atmosphere ? altitude / body.atmosphereDepth : 0;

            Node a, b;
            double aCoeff;

            if (altitudeRatio > 0.5)  // Atmospheric entry
            {
                a = entry;
                b = highAltitude;
                aCoeff = Math.Min((altitudeRatio - 0.5) * 2.0, 1.0);
            }
            else if (altitudeRatio > 0.25)  // High Altitude
            {
                a = highAltitude;
                b = lowAltitude;
                aCoeff = altitudeRatio * 4.0 - 1.0;
            }
            else if (altitudeRatio > 0.05)  // Low Altitude
            {
                a = lowAltitude;
                b = finalApproach;
                aCoeff = altitudeRatio * 5.0 - 0.25;

                aCoeff = 1.0 - aCoeff;
                aCoeff = 1.0 - aCoeff * aCoeff;
            }
            else    // Final Approach or Non-Atmospheric Body
            {
                return finalApproach.GetAngleOfAttack(position, velocity);
            }

            double aAoA = a.GetAngleOfAttack(position, velocity);
            double bAoA = b.GetAngleOfAttack(position, velocity);

            return aAoA * aCoeff + bAoA * (1.0 - aCoeff);
        }
    }
}