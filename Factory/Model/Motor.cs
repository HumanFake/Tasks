using JetBrains.Annotations;

namespace Model
{
    public class Motor : IProduct
    {
        public string Id { get; }

        public Motor([NotNull] string id)
        {
            Id = id;
        }
    }
}