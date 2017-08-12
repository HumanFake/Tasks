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

        public void StartFabric()
        {
            _fabric.Start();
        }

        public void StopFabric()
        {
            _fabric.Stop();
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
            _fabric.SetAccessorySupplyTime(time);
        }

        public void SetDealersReleaseTime(uint time)
        {

        }
    }
}