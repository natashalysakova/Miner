using System;
using System.Windows.Forms;

namespace Miner
{
    public partial class CustomLevel : Form
    {
        public CustomLevel()
        {
            InitializeComponent();
        }

        internal Level GetNewLevelData()
        {
            return new Level("User", (int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value);
        }

        private void CustomLevel_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = Properties.Settings.Default.Width;
            numericUpDown2.Value = Properties.Settings.Default.Height;
            numericUpDown3.Value = Properties.Settings.Default.Mines;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            
        }
    }
}
