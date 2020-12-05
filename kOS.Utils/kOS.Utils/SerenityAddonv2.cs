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
using System.Reflection;
using UnityEngine;

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
            AddSuffix("GETPISTON", new OneArgsSuffix<PistonValue, PartValue>(GetPiston));
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
            AddSuffix("TARGETEXTENSION", new SetSuffix<ScalarValue>(() => (ScalarValue)servo.Fields["targetExtension"].GetValue(servo), value => SetTargetExt(value)));
            AddSuffix("SPEED", new SetSuffix<ScalarValue>(() => servo.traverseVelocity, value => servo.traverseVelocity = value));
            AddSuffix("ENGAGE", new NoArgsVoidSuffix(servo.EngageMotor));
            AddSuffix("DISENGAGE", new NoArgsVoidSuffix(servo.DisengageMotor));
            AddSuffix("LOCKED", new SetSuffix<BooleanValue>(() => servo.servoIsLocked, LockServo));
            AddSuffix("PART", new Suffix<PartValue>(() => part));
        }

        private void SetTargetExt(ScalarValue ext)
        {
            BaseAxisField field = (BaseAxisField)typeof(ModuleRoboticServoPiston)
                .GetField("targetExtensionAxisField", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(servo);
            field.SetValue((float) ext, field.module);
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
            AddSuffix("CURRENTANGLE", new Suffix<ScalarValue>(GetCurrentAngle));
            AddSuffix("SPEED", new SetSuffix<ScalarValue>(() => servo.traverseVelocity, value => servo.traverseVelocity = value));
            AddSuffix("ENGAGE", new NoArgsVoidSuffix(servo.EngageMotor));
            AddSuffix("DISENGAGE", new NoArgsVoidSuffix(servo.DisengageMotor));
            AddSuffix("LOCKED", new SetSuffix<BooleanValue>(() => servo.servoIsLocked, LockServo));
            AddSuffix("PART", new Suffix<PartValue>(() => part));
        }

        private ScalarValue GetCurrentAngle()
        {
            float angle = servo.modelInitialAngle + (float) typeof(ModuleRoboticServoHinge)
                .GetMethod("currentTransformAngle", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(servo, null);
            return angle;
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
            AddSuffix("RPMLIMIT", new SetSuffix<ScalarValue>(() => rotor.rpmLimit, value => SetRPM((float)value)));
            AddSuffix("MAXTORQUE", new SetSuffix<ScalarValue>(() => rotor.maxTorque, value => rotor.maxTorque = value));
            AddSuffix("CURRENTRPM", new Suffix<ScalarValue>(GetRotorRPM));
            AddSuffix("TORQUELIMIT", new SetSuffix<ScalarValue>(() => (ScalarValue)rotor.Fields["servoMotorLimit"].GetValue(rotor), SetTorque));
        }

        private ScalarValue GetRotorRPM()
        {
            return (float)typeof(ModuleRoboticServoRotor)
                .GetField("transformRateOfMotion", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(rotor);
        }

        private void SetTorque(ScalarValue torque)
        {
            rotor.Fields["servoMotorLimit"].SetValue((float)torque, rotor);
            //typeof(ModuleRoboticServoRotor).GetField("servoMotorLimit", )
        }

        private void SetRPM(float RPM)
        {
            BaseAxisField field = (BaseAxisField)typeof(ModuleRoboticServoRotor).GetField("rpmLimitAxisField", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(rotor);
            field.SetValue(RPM, field.module);
            //typeof(ModuleRoboticServoRotor)
            //.GetMethod("UpdateAxisFieldHardLimit", BindingFlags.Instance | BindingFlags.NonPublic)
            //.Invoke(rotor, new object[] {"rpmLimitAxisField", new Vector2(0f, RPM)});
            //rotor.Fields["rpmLimit"].SetValue(RPM, rotor);
            //object[] parametersArray = new object[] { UI_Scene.Flight };
            //BaseField a = typeof(BaseServo).GetField("partActionMenuOpen", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(rotor).SetValue(rotor, true);
            //rotor.GetType().GetMethod("UpdatePAWUI", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(rotor, parametersArray);
            //typeof(BaseServo).GetField("partActionMenuOpen", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(rotor, false);
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
            AddSuffix("CURRENTANGLE", new Suffix<ScalarValue>(GetCurrentAngle));
            AddSuffix("INVERTED", new SetSuffix<BooleanValue>(() => servo.inverted, value => servo.inverted = value));
            AddSuffix("SPEED", new SetSuffix<ScalarValue>(() => servo.traverseVelocity, value => servo.traverseVelocity = value));
            AddSuffix("ENGAGE", new NoArgsVoidSuffix(servo.EngageMotor));
            AddSuffix("DISENGAGE", new NoArgsVoidSuffix(servo.DisengageMotor));
            AddSuffix("LOCKED", new SetSuffix<BooleanValue>(() => servo.servoIsLocked, LockServo));
            AddSuffix("PART", new Suffix<PartValue>(() => part));

        }

        private ScalarValue GetCurrentAngle()
        {
            return (float)typeof(ModuleRoboticRotationServo)
                .GetMethod("currentTransformAngle", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(servo, null);
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