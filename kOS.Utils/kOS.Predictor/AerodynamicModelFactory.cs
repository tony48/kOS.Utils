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
using System.Reflection;
using System;

namespace Predictor
{
    public static class AerodynamicModelFactory
        {
            public static VesselAerodynamicModel GetModel(Vessel ship, CelestialBody body)
            {
                foreach (var loadedAssembly in AssemblyLoader.loadedAssemblies)
                {
                    try
                    {
                        switch (loadedAssembly.name)
                        {
                            case "FerramAerospaceResearch":
                                var FARAPIType = loadedAssembly.assembly.GetType("FerramAerospaceResearch.FARAPI");

                                var FARAPI_CalculateVesselAeroForces = FARAPIType.GetMethodEx("CalculateVesselAeroForces", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(Vessel), typeof(Vector3).MakeByRefType(), typeof(Vector3).MakeByRefType(), typeof(Vector3), typeof(double) });

                                return new FARModel(ship, body, FARAPI_CalculateVesselAeroForces);

                            //case "MyModAssembly":
                            // implement here your atmo mod detection
                            // return new MyModModel(ship, body, any other parameter);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Trajectories: failed to interface with assembly " + loadedAssembly.name);
                        Debug.Log("Using stock model instead");
                        Debug.Log(e.ToString());
                    }
                }

                // Using stock model if no other aerodynamic is detected or if any error occured
                return new StockModel(ship, body);
            }
            public static MethodInfo GetMethodEx(this Type type, string methodName, BindingFlags flags)
            {
                try
                {
                    var res = type.GetMethod(methodName, flags);
                    if (res == null)
                        throw new Exception("method not found");
                    return res;
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to GetMethod " + methodName + " on type " + type.FullName + " with flags " + flags + ":\n" + e.Message + "\n" + e.StackTrace);
                }
            }

            public static MethodInfo GetMethodEx(this Type type, string methodName, Type[] types)
            {
                try
                {
                    var res = type.GetMethod(methodName, types);
                    if (res == null)
                        throw new Exception("method not found");
                    return res;
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to GetMethod " + methodName + " on type " + type.FullName + " with types " + types.ToString() + ":\n" + e.Message + "\n" + e.StackTrace);
                }
            }

            public static MethodInfo GetMethodEx(this Type type, string methodName, BindingFlags flags, Type[] types)
            {
                try
                {
                    var res = type.GetMethod(methodName, flags, null, types, null);
                    if (res == null)
                        throw new Exception("method not found");
                    return res;
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to GetMethod " + methodName + " on type " + type.FullName + " with types " + types.ToString() + ":\n" + e.Message + "\n" + e.StackTrace);
                }
            }
        }
    
    
}