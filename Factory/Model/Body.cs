namespace Model
{
    public sealed class Body : IProduct
    {
        public string Id { get; }

        public Body(string id)
        {
            Id = id;
        }
    }
}
