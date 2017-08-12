using Model;

namespace Factory
{
    public class FabricController
    {
        private readonly Fabric _fabric;

        public FabricController(Fabric fabric)
        {
            _fabric = fabric;
        }

        public void SetMotorSupplyTime(uint time)
        {
            _fabric.SetMotorSupplyTime(time);
        }

        public void SetBodySupplyTime(uint time)
        {
            _fabric.SetBodySupplyTime(time);
        }

        public void SetAccessorySupplyTime(uint time)
        {
            _fabric.SetAccesserySupplyTime(time);
        }
    }
}