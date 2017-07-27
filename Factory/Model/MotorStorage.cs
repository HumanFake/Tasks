using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace Model
{
    public class MotorStorage
    {
        private readonly uint _maxCapacity;
        private readonly List<Motor> _motors = new List<Motor>();

        public MotorStorage(uint maxCapacity)
        {
            _maxCapacity = maxCapacity;
        }

        internal void AddMotor([NotNull] Motor motor)
        {
            lock (_motors)
            {
                Monitor.PulseAll(_motors);
                while (_motors.Count >= _maxCapacity)
                {
                    Monitor.Wait(_motors);
                }
                
                Console.WriteLine(motor.Id + $" : add to storage. Current capacity: {_motors.Count + 1}");
                _motors.Add(motor);
            }
        }

        public Motor PopMotor()
        {
            lock (_motors)
            {
                Monitor.PulseAll(_motors);
                while (_motors.Count == 0)
                {
                    Monitor.Wait(_motors);
                }

                var result = _motors.First();
                _motors.Remove(result);

                return result;
            }
        }

        private int Capacity => _motors.Count;
    }
}