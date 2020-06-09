/*
    Copyright (c) 2020 tony48
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
using kOS.Safe.Utilities;
using kOS.Suffixed;
using kOS.Suffixed.Part;
using System.IO;
using Expansions.Serenity;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Exceptions;

namespace kOS.Utils
{
    [kOSAddon("BG")]
    [KOSNomenclature("BGAddon")]
    public class SerenityAddonv2 : Suffixed.Addon
    {
        public SerenityAddonv2(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("GETROTOR", new OneArgsSuffix<RotorValue, PartValue>(GetRotor));
            AddSuffix("GETSERVO", new OneArgsSuffix<ServoValue, PartValue>(GetServo));
            AddSuffix("GETHINGE", new OneArgsSuffix<HingeValue, PartValue>(GetHinge));
        }

        private RotorValue GetRotor(PartValue part)
        {
            return new RotorValue(part.Part.FindModuleImplementing<ModuleRoboticServoRotor>(), shared);
        }

        private ServoValue GetServo(PartValue part)
        {
            return new ServoValue(part.Part.FindModuleImplementing<ModuleRoboticRotationServo>(), shared);
        }

        private HingeValue GetHinge(PartValue part)
        {
            return new HingeValue(part.Part.FindModuleImplementing<ModuleRoboticServoHinge>(), shared);
        }

        private PistonValue GetPiston(PartValue part)
        {
            return new PistonValue(part.Part.FindModuleImplementing<ModuleRoboticServoPiston>(), shared);
        }

        public override BooleanValue Available()
        {
            if (Directory.Exists(KSPUtil.ApplicationRootPath + "GameData/SquadExpansion/Serenity"))
                return true;
            return false;
            }
    }
    
    /*
    [KOSNomenclature("RoboticPart")]
    public class RoboticPart : Structure
    {
        
    }
    */
    
    #region Piston

    [KOSNomenclature("PistonValue")]
    public class PistonValue : Structure
    {
        private ModuleRoboticServoPiston servo;
        private SharedObjects shared;
        private PartValue part;

        public PistonValue(ModuleRoboticServoPiston init, SharedObjects shared)
        {
            servo = init;
            this.shared = shared;
            part = GetPart();
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("EXTEND", new NoArgsVoidSuffix(servo.ExtendPiston));
            AddSuffix("RETRACT", new NoArgsVoidSuffix(servo.RetractPiston));
            AddSuffix("CURRENTEXTENSION", new Suffix<ScalarValue>(() => servo.currentExtension));
            AddSuffix("TARGETEXTENSION", new SetSuffix<ScalarValue>(() => servo.targetExtension, value => servo.targetExtension = value));
            AddSuffix("SPEED", new SetSuffix<ScalarValue>(() => servo.traverseVelocity, value => servo.traverseVelocity = value));
            AddSuffix("ENGAGE", new NoArgsVoidSuffix(servo.EngageMotor));
            AddSuffix("DISENGAGE", new NoArgsVoidSuffix(servo.DisengageMotor));
            AddSuffix("LOCKED", new SetSuffix<BooleanValue>(() => servo.servoIsLocked, LockServo));
            AddSuffix("PART", new Suffix<PartValue>(() => part));
        }
        
        private void LockServo(BooleanValue b)
        {
            if (b)
            {
                bool isLocked = servo.EngageServoLock();
                if (isLocked)
                    return;
                else
                    throw new KOSException("Servo lock wasn't engaged");
            }
            else
            {
                servo.DisengageServoLock();
            }
        }

        private PartValue GetPart()
        {
            var p = servo.part;

            return p != null ? VesselTarget.CreateOrGetExisting(shared)[p] : null;
        }
    }
    #endregion
    
    #region Hinge

    [KOSNomenclature("HingeValue")]
    public class HingeValue : Structure
    {
        private ModuleRoboticServoHinge servo;
        private SharedObjects shared;
        private PartValue part;
        
        public HingeValue(ModuleRoboticServoHinge init, SharedObjects shared)
        {
            servo = init;
            this.shared = shared;
            part = GetPart();
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("TARGETANGLE", new SetSuffix<ScalarValue>(() => servo.targetAngle, value => servo.targetAngle = value));
            AddSuffix("CURRENTANGLE", new Suffix<ScalarValue>(() => servo.currentAngle));
            AddSuffix("SPEED", new SetSuffix<ScalarValue>(() => servo.traverseVelocity, value => servo.traverseVelocity = value));
            AddSuffix("ENGAGE", new NoArgsVoidSuffix(servo.EngageMotor));
            AddSuffix("DISENGAGE", new NoArgsVoidSuffix(servo.DisengageMotor));
            AddSuffix("LOCKED", new SetSuffix<BooleanValue>(() => servo.servoIsLocked, LockServo));
            AddSuffix("PART", new Suffix<PartValue>(() => part));
        }
        
        private void LockServo(BooleanValue b)
        {
            if (b)
            {
                bool isLocked = servo.EngageServoLock();
                if (isLocked)
                    return;
                else
                    throw new KOSException("Servo lock wasn't engaged");
            }
            else
            {
                servo.DisengageServoLock();
            }
        }
        
        private PartValue GetPart()
        {
            var p = servo.part;

            return p != null ? VesselTarget.CreateOrGetExisting(shared)[p] : null;
        }
    }
    #endregion

    #region Rotor
    [KOSNomenclature("RotorValue")]
    public class RotorValue : Structure
    {
        private ModuleRoboticServoRotor rotor;
        private PartValue part;
        private SharedObjects shared;
        public RotorValue(ModuleRoboticServoRotor init, SharedObjects shared)
        {
            rotor = init;
            this.shared = shared;
            part = GetPart();
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            //AddSuffix("INVERTED", new SetSuffix<BooleanValue>(() => rotor.inverted, value => rotor.inverted = value));
            AddSuffix("TOGGLEDIRECTION", new NoArgsVoidSuffix(rotor.ToggleMotorDirection));
            AddSuffix("COUNTERCLOCKWISE", new Suffix<BooleanValue>(() => rotor.rotateCounterClockwise));
            AddSuffix("PART", new Suffix<PartValue>(() => part));
            AddSuffix("ENGAGE", new NoArgsVoidSuffix(rotor.EngageMotor));
            AddSuffix("DISENGAGE", new NoArgsVoidSuffix(rotor.DisengageMotor));
            AddSuffix("LOCKED", new SetSuffix<BooleanValue>(() => rotor.servoIsLocked, LockServo));
            AddSuffix("RPMLIMIT", new SetSuffix<ScalarValue>(() => rotor.rpmLimit, value => rotor.rpmLimit = value));
            AddSuffix("MAXTORQUE", new SetSuffix<ScalarValue>(() => rotor.maxTorque, value => rotor.maxTorque = value));
            AddSuffix("CURRENTRPM", new Suffix<ScalarValue>(() => rotor.currentRPM));
        }
        
        private void LockServo(BooleanValue b)
        {
            if (b)
            {
                bool isLocked = rotor.EngageServoLock();
                if (isLocked)
                    return;
                else
                    throw new KOSException("Servo lock wasn't engaged");
            }
            else
            {
                rotor.DisengageServoLock();
            }
        }
        
        private PartValue GetPart()
        {
            var p = rotor.part;

            return p != null ? VesselTarget.CreateOrGetExisting(shared)[p] : null;
        }
        
    }
    
    #endregion

    #region Servo
    // Servo
    [KOSNomenclature("ServoValue")]
    public class ServoValue : Structure
    {
        private ModuleRoboticRotationServo servo;
        private PartValue part;
        private SharedObjects shared;

        public ServoValue(ModuleRoboticRotationServo init, SharedObjects shared)
        {
            servo = init;
            this.shared = shared;
            part = GetPart();
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("TARGETANGLE", new SetSuffix<ScalarValue>(() => servo.targetAngle, value => servo.targetAngle = value));
            AddSuffix("CURRENTANGLE", new Suffix<ScalarValue>(() => servo.currentAngle));
            AddSuffix("INVERTED", new SetSuffix<BooleanValue>(() => servo.inverted, value => servo.inverted = value));
            AddSuffix("SPEED", new SetSuffix<ScalarValue>(() => servo.traverseVelocity, value => servo.traverseVelocity = value));
            AddSuffix("ENGAGE", new NoArgsVoidSuffix(servo.EngageMotor));
            AddSuffix("DISENGAGE", new NoArgsVoidSuffix(servo.DisengageMotor));
            AddSuffix("LOCKED", new SetSuffix<BooleanValue>(() => servo.servoIsLocked, LockServo));
            AddSuffix("PART", new Suffix<PartValue>(() => part));

        }

        private void LockServo(BooleanValue b)
        {
            if (b)
            {
                bool isLocked = servo.EngageServoLock();
                if (isLocked)
                    return;
                else
                    throw new KOSException("Servo lock wasn't engaged");
            }
            else
            {
                servo.DisengageServoLock();
            }
        }
        
        private PartValue GetPart()
        {
            var p = servo.part;

            return p != null ? VesselTarget.CreateOrGetExisting(shared)[p] : null;
        }
    }
    #endregion
}