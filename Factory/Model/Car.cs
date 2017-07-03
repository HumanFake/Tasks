using JetBrains.Annotations;

namespace Model
{
    public class Car
    {
        public string Id { get; }

        public Car([NotNull] string id)
        {
            Id = id;
        }
    }
}