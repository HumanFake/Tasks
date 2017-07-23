using JetBrains.Annotations;

namespace Model
{
    public class Motor
    {
        public string Id { get; }

        public Motor([NotNull] string id)
        {
            Id = id;
        }
    }
}