namespace Model
{
    public class Accessor: IProduct
    {
        public string Id { get; }

        public Accessor(string id)
        {
            Id = id;
        }
    }
}