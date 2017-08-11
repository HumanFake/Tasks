using System;
using System.Windows.Forms;
using Model;
using Model.Observers;

namespace Factory
{
    public partial class MainForm : Form, IStorageObserver
    {
        private const uint MinimalSuplyTime = 200;

        private readonly Fabric _fabric;
        private readonly FabricController _fabricController;

        private bool _enabled = false;

        public MainForm()
        {
            InitializeComponent();
            _fabric = new Fabric(this);
            _fabricController = new FabricController(_fabric);

            bodySupplyTimeText.Text = MinimalSuplyTime.ToString();
            motorSupplyTimeText.Text = MinimalSuplyTime.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _enabled = true;
            _fabric.Start();
        }

        public void OnStorageChange()
        {
            UpdateFabricState();
        }

        private void UpdateFabricState()
        {
            SetTextBox(currentBodyCountText, _fabric.GetBodyStorage().Capacity.ToString());
            SetTextBox(currentMotorCountText, _fabric.GetMotorStorage().Capacity.ToString());

            SetTextBox(totalBodyCountText, _fabric.GetBodyStorage().ProductsInStorageForAllTime.ToString());
            SetTextBox(totalMotorCountText, _fabric.GetMotorStorage().ProductsInStorageForAllTime.ToString());
        }

        private void SetTextBox(Control target, string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => target.Text = text));
                return;
            }
            target.Text = text;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _fabric.Stop();
        }

        private void bodyCreatTimeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            var supplyTime = TryGetSupplyTime(bodyCreatTimeTrackBar);
            if (supplyTime == null)
            {
                return;
            }

            bodySupplyTimeText.Text = supplyTime.ToString();
            _fabricController.SetMotorSupplyTime(Convert.ToUInt32(supplyTime));
        }

        private void motorCreatTimeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            var supplyTime = TryGetSupplyTime(motorCreatTimeTrackBar);
            if (supplyTime == null)
            {
                return;
            }

            motorSupplyTimeText.Text = supplyTime.ToString();
            _fabricController.SetBodySupplyTime(Convert.ToUInt32(supplyTime));
        }

        private uint? TryGetSupplyTime(TrackBar trackBar)
        {
            if (false == _enabled)
            {
                return null;
            }
            var supplyTimeRate = trackBar.Value;
            var supplyTime = Convert.ToUInt32(supplyTimeRate * MinimalSuplyTime);

            return supplyTime;
        }
    }
}
