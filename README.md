# kOS.Utils

## Table of contents
- [Sockets]
- [Orbital functions]
- [Input]
- [Screen]
- [Impact Prediction] (see Predictor if you are looking for predictions on atmopheric bodies)
- [Predictor]
- [Breaking Ground]

## Sockets

### Addon

To access socket functions, use ADDONS:SOCK.
Example : ADDONS:SOCK:CONNECT("127.0.0.1", 11000).

- Connect(string ipAddress, scalar port)

  Returns a kOSSocket object and connect it to a server socket at the given IP address and port.
  
### kOSSocket

- Send(string message)

  Send a message to the socket server. THE SERVER MUST DECODE THE MESSAGE AS UTF-8.
  
- Queue

  Returns the message queue. All the received messages are in the queue. See [here](https://ksp-kos.github.io/KOS/structures/collections/queue.html) for the queue documentation. THE SERVER MUST SEND ALL THE MESSAGES IN UTF-8.
  
- Close()

  Close the socket.

### Example script

```
set socket to addons:sock:connect("127.0.0.1", 11000). // create a socket and connect it to 127.0.0.1:11000
socket:send("This is a test").  // Send "This is a test"
wait until not socket:queue:empty. // Wait until we have receiced something
print socket:queue:pop(). // print the received message and remove it from the queue
```

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


## Impact Prediction

To access impact prediction functions, use ADDONS:IMPACT.

THESE FUNCTIONS DO NOT TAKE INTO ACCOUNT THE ATMOSPHERIC FORCES. If you want this, use trajectories or go to the Predictor section.

- GetImpactPos(Vessel vessel)

  Returns the impact location for the vessel in [GeoCoordinates](https://ksp-kos.github.io/KOS/math/geocoordinates.html#structure:GEOCOORDINATES).

## Predictor

Functions to predict the impact position on an atmospheric body.

To access predictor functions, use ADDONS:PR.

- ImpactPos(Vessel vessel)

  Returns the impact location for the vessel in [GeoCoordinates](https://ksp-kos.github.io/KOS/math/geocoordinates.html#structure:GEOCOORDINATES).
  
- HasImpact(Vessel vessel)

  Returns true if the vessel has an impact position.
  
## BreakingGround 

Functions to control Breaking Ground robotic parts.

To access predictor functions, use ADDONS:BG.

- GetRotor(Part part)

  Returns the Rotor object found on the part.
  
- GetHinge(Part part)

  Returns the Hinge object found on the part.
  
- GetPiston(Part part)

  Returns the Piston object found on the part.

- GetServo(Part part)

  Returns the Servo object found on the part.
  
#### Rotor

- ToggleDirection()

  Toggle the rotor direction.
  
- Counterclockwise

  True if the rotor rotates counterclockwise.

- RPMLimit

  The rotor rpm limit.
  
- MaxTorque

  The rotor max torque.
  
- CurrentRPM

  The rotor current RPM.
  
- Engage()

  Engage the servo.

- Disengage()

  Disengage the servo.
  
- Locked

  Get or set the lock of the servo.
  
- Part

  The part containing the servo.

#### Servo

- Inverted

  True if the servo is inverted.

- TargetAngle

  Get or set the target angle.

- CurrentAngle

  Get the current angle,
  
- Speed

  Get or set the speed.
  
- Engage()

  Engage the servo.

- Disengage()

  Disengage the servo.
  
- Locked

  Get or set the lock of the servo.
  
- Part

  The part containing the servo.

#### Hinge

- TargetAngle

  Get or set the target angle.

- CurrentAngle

  Get the current angle,
  
- Speed

  Get or set the speed.
  
- Engage()

  Engage the servo.

- Disengage()

  Disengage the servo.
  
- Locked

  Get or set the lock of the servo.
  
- Part

  The part containing the servo.

#### Piston

- Extend()
  
  Extend the piston.
  
- Retract()

  Retract the piston.
  
- TargetExtension

  Get or set the target extension.
  
- CurrentExtension

  The piston current extension.

- Speed

  Get or set the speed.
  
- Engage()

  Engage the servo.

- Disengage()

  Disengage the servo.
  
- Locked

  Get or set the lock of the servo.
  
- Part

  The part containing the servo.
