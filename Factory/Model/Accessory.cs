namespace Model
{
    public class Accessory: IProduct
    {
        public string Id { get; }

        public Accessory(string id)
        {
            Id = id;
        }
    }
}