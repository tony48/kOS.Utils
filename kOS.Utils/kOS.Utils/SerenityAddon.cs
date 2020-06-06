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
using System.Diagnostics.CodeAnalysis;
using kOS.AddOns;
using kOS.Safe.Encapsulation;
using kOS.Safe.Exceptions;
using UnityEngine;
using kOS.Suffixed;
using kOS.Suffixed.Part;
using Expansions.Serenity;
using kOS.Safe.Encapsulation.Suffixes;


namespace kOS.Utils
{
    [kOSAddon("BG")]
    [Safe.Utilities.KOSNomenclature("BGAddon")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class SerenityAddon : Suffixed.Addon
    {
        public SerenityAddon(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("SETTARGETPOS", new TwoArgsSuffix<PartValue, ScalarValue>(SetTargetAngle));
            AddSuffix("GETCURRENTPOS", new OneArgsSuffix<ScalarValue, PartValue>(GetCurrentAngle));
            AddSuffix("SETINVERTED", new TwoArgsSuffix<PartValue, BooleanValue>(SetInverted));
            AddSuffix("GETINVERTED", new OneArgsSuffix<BooleanValue, PartValue>(GetInverted));
            AddSuffix("SETLOCKED", new TwoArgsSuffix<PartValue, BooleanValue>(SetLocked));
            AddSuffix("GETLOCKED", new OneArgsSuffix<BooleanValue, PartValue>(GetLocked));
            AddSuffix("SETENGAGED", new TwoArgsSuffix<PartValue, BooleanValue>(SetEngaged));
            AddSuffix("GETENGAGED", new OneArgsSuffix<BooleanValue, PartValue>(GetEngaged));
        }

        private void SetTargetAngle(PartValue part, ScalarValue angle)
        {
            if (!part.Part.isRobotic())
                throw new KOSException("You can only call this method on robotics parts.");
            BaseServo servo = part.Part.FindModuleImplementing<BaseServo>();
            if (part.Part.isRoboticRotationServo())
            {
                ((ModuleRoboticRotationServo) servo).targetAngle = angle;
            }
            else if (part.Part.isRoboticHinge())
            {
                ((ModuleRoboticServoHinge) servo).targetAngle = angle;
            }
            else if (part.Part.isRoboticPiston())
            {
                ((ModuleRoboticServoPiston) servo).targetExtension = angle;
            }
        }

        private ScalarValue GetCurrentAngle(PartValue part)
        {
            if (!part.Part.isRobotic())
                throw new KOSException("You can only call this method on robotics parts.");
            BaseServo servo = part.Part.FindModuleImplementing<BaseServo>();
            if (part.Part.isRoboticRotationServo())
            {
                return ((ModuleRoboticRotationServo) servo).currentAngle;
            }
            if (part.Part.isRoboticHinge())
            {
                return ((ModuleRoboticServoHinge) servo).currentAngle;
            }
            if (part.Part.isRoboticPiston())
            {
                return ((ModuleRoboticServoPiston) servo).currentExtension;
            }
            throw new KOSException("Part must be an hinge, a servo or a piston.");
        }

        private void SetInverted(PartValue part, BooleanValue bo)
        {
            if (!part.Part.isRobotic())
                throw new KOSException("You can only call this method on robotics parts.");
            BaseServo servo = part.Part.FindModuleImplementing<BaseServo>();
            if (part.Part.isRoboticRotationServo())
            {
                ((ModuleRoboticRotationServo) servo).inverted = bo;
            }
            else if (part.Part.isRoboticRotor())
            {
                ((ModuleRoboticServoRotor) servo).inverted = bo;
            }
        }

        private BooleanValue GetInverted(PartValue part)
        {
            if (!part.Part.isRobotic())
                throw new KOSException("You can only call this method on robotics parts.");
            BaseServo servo = part.Part.FindModuleImplementing<BaseServo>();
            if (part.Part.isRoboticRotationServo())
            {
                return ((ModuleRoboticRotationServo) servo).inverted;
            }
            if (part.Part.isRoboticRotor())
            {
                return ((ModuleRoboticServoRotor) servo).inverted;
            }
            throw new KOSException("Part must be a rotor or a servo.");
        }

        private void SetLocked(PartValue part, BooleanValue locked)
        {
            if (!part.Part.isRobotic())
                throw new KOSException("You can only call this method on robotics parts.");
            BaseServo servo = part.Part.FindModuleImplementing<BaseServo>();
            servo.servoIsLocked = locked;
        }

        private BooleanValue GetLocked(PartValue part)
        {
            if (!part.Part.isRobotic())
                throw new KOSException("You can only call this method on robotics parts.");
            BaseServo servo = part.Part.FindModuleImplementing<BaseServo>();
            return servo.servoIsLocked;
        }

        private void SetEngaged(PartValue part, BooleanValue bo)
        {
            if (!part.Part.isRobotic())
                throw new KOSException("You can only call this method on robotics parts.");
            BaseServo servo = part.Part.FindModuleImplementing<BaseServo>();
            servo.servoMotorIsEngaged = bo;
        }
        
        private BooleanValue GetEngaged(PartValue part)
        {
            if (!part.Part.isRobotic())
                throw new KOSException("You can only call this method on robotics parts.");
            BaseServo servo = part.Part.FindModuleImplementing<BaseServo>();
            return servo.servoMotorIsEngaged;
        }

        public override BooleanValue Available()
        {
            return true;
        }
    }
}