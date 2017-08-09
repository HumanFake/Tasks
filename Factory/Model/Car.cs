using JetBrains.Annotations;

namespace Model
{
    public class Car: IProduct
    {
        public string Id { get; }

        public Motor Motor { get; }
        public Body Body { get; }

        public Car(Motor motor, Body body, [NotNull] string id)
        {
            Id = id;
            Motor = motor;
            Body = body;
        }
    }
}