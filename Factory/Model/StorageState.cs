namespace Model
{
    public class StorageState
    {
        public int InStock { get; }
        public int ForAllTime { get; }

        public StorageState(int inStock, int forAllTime)
        {
            InStock = inStock;
            ForAllTime = forAllTime;
        }
    }
}