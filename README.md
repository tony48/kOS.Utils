# kOS.Utils
## Orbital functions

To access orbital functions, use ADDONS:OBT.
Example : ADDONS:OBT:MEANANOMALYATUT(ship, 2810320).

- MeanAnomalyAtUT([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) obt, Scalar UT)
  
  The mean anomaly for the orbitable obt at the given time.
  
- RadiusAtTrueAnomaly([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) obt, Scalar TrueAnomaly)

  The orbital radius at the point in the orbit given by the true anomaly for the orbitable obt.

- TrueAnomalyAtRadius([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) obt, Scalar radius)

  The true anomaly at the given orbital radius for the orbitable obt.

- TrueAnomalyAtUT([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) obt, Scalar UT)

  The true anomaly at the given time for the orbitable obt.
  
- UTAtTrueAnomaly([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) obt, Scalar TrueAnomaly)

  The universal time, in seconds, corresponding to the given true anomaly, for the orbitable obt.
  
- EccentricAnomalyAtUT([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) obt, Scalar UT)

  The eccentric anomaly at the given universal time, for the orbitable obt.
  
- TrueAnomalyAtAN([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) ship, [Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) target)

  The true anomaly of the ascending node with the given target orbit.
  
- TrueAnomalyAtDN([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) ship, [Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) target)

  The true anomaly of the descending node with the given target orbit.

- RelativeInclination([Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) ship, [Orbitable](https://ksp-kos.github.io/KOS/structures/orbits/orbitable.html) target)

  Relative inclination of an orbit and another, in degrees.

## Input

To access input functions, use ADDONS:INPUT.
Example : ADDONS:INPUT:ENGAGE().

- Engage()

  Lock the input. You MUST do this before calling GetKey.

- Disengage()

  Unlock the input.
  
- GetKey(String key)

  Returns true if the key is pressed. Values for key can be found [here](https://gist.github.com/C0DEF52/b1168e6ed3d1f567fc919f2942037bab).
  
## Screen

To access screenshot functions, use ADDONS:SCREEN.
Example : ADDONS:SCREEN:TAKE().

- Take()

  Takes a screenshot and saves it in KSP/Screenshots_kOS.
