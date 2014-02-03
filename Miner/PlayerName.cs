using System;
using System.Windows.Forms;

namespace Miner
{
    public partial class PlayerName : Form
    {
        public PlayerName()
        {
            InitializeComponent();
        }

        private void PlayerName_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.PlayerName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.PlayerName = textBox1.Text;
            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;
        }

        public string GetPlayerName()
        {
            return textBox1.Text;
        }
    }
}
