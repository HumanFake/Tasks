using JetBrains.Annotations;

namespace Model
{
    public class Car: IProduct
    {
        public string Id { get; }

        public Motor Motor { get; }
        public Body Body { get; }
        public Accessory Accessory { get; }

        public Car([NotNull] Motor motor, [NotNull]  Body body, [NotNull]  Accessory accessory, [NotNull] string id)
        {
            Id = id;
            Motor = motor;
            Body = body;
            Accessory = accessory;
        }
    }
}