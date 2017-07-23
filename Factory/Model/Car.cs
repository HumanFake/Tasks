using JetBrains.Annotations;

namespace Model
{
    public class Car
    {
        public string Id { get; }

        public Motor Motor { get; }

        public Car(Motor motor,[NotNull] string id)
        {
            Id = id;
            Motor = motor;
        }
    }
}