using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Miner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel1.GetControlFromPosition(0, 1), 3);

            random = new Random();
            game = new Game();

            button1.BackgroundImage = Properties.Resources.smiles_0017_Layer_1_copy_13;
        }

        private Game game;
        private Random random;
        Stopwatch time = new Stopwatch();
        private Level[] levels = new[]
        {
            new Level("Easy", 7, 8, 10), 
            new Level("Medium", 20, 10, 45), 
            new Level("Hard", 40, 15, 100),
            new Level("User", Properties.Settings.Default.Width, Properties.Settings.Default.Height,
                Properties.Settings.Default.Mines)
        };

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame(Properties.Settings.Default.LastUsedLevelName);
        }

        private void StartGame(string levelName)
        {
            game = new Game();

            Level l = levels[0];

            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i].Name == levelName)
                    l = levels[i];
            }


            game.NewGame(pictureBox1, l.Width, l.Height, l.Mines);



            button1.BackgroundImage = Properties.Resources.smiles_0017_Layer_1_copy_13;

            this.Width = game.Cellsize * l.Width + 48;
            this.Height = game.Cellsize * l.Height + 132;

            time.Reset();
            time.Start();
            timer1.Enabled = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            game.DrawField();
            if (game.IsOver)
            {
                timer1.Enabled = false;

                if (game.IsWin)
                {
                    time.Stop();

                    button1.BackgroundImage = game.WinSmiles[random.Next(0, game.WinSmiles.Length - 1)];

                    HighScores h = new HighScores(true);
                    h.SetTime(time.Elapsed.ToString());
                    h.ShowDialog();


                    


                }
                else
                {
                    button1.BackgroundImage = game.LoseSmiles[random.Next(0, game.LoseSmiles.Length - 1)];
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartGame(Properties.Settings.Default.LastUsedLevelName);
        }

        private int selectedMoveSmilee = -1;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                game.SetPressed(e.Location);
            }

            if (e.Button == MouseButtons.None)
            {
                game.ResetPressed();
                selectedMoveSmilee = -1;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                game.OpenCell(e.Location);
            }
            
            if (e.Button == MouseButtons.Right)
                game.SetFlag(e.Location);
            
            

            if (!game.IsOver)
            {
                button1.BackgroundImage = Properties.Resources.smiles_0017_Layer_1_copy_13;
            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                game.SetPressed(e.Location);
            }

            if (e.Button == MouseButtons.None)
            {
                game.ResetPressed();
                selectedMoveSmilee = -1;
            }

            if (!game.IsOver)
            {
                if (selectedMoveSmilee == -1)
                {
                    selectedMoveSmilee = random.Next(0, game.MoveSmiles.Length - 1);
                    button1.BackgroundImage = game.MoveSmiles[selectedMoveSmilee];
                }
            }
        }

        private void легкий7х8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastUsedLevelName = "Easy";
            Properties.Settings.Default.Save();

            StartGame(Properties.Settings.Default.LastUsedLevelName);

        }

        private void среднийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastUsedLevelName = "Medium";
            Properties.Settings.Default.Save();

            StartGame(Properties.Settings.Default.LastUsedLevelName);
        }

        private void сложный40х15ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastUsedLevelName = "Hard";
            Properties.Settings.Default.Save();

            StartGame(Properties.Settings.Default.LastUsedLevelName);
        }

        private void другойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomLevel f = new CustomLevel();


            if (f.ShowDialog() == DialogResult.OK)
            {
                levels[3] = f.GetNewLevelData();

                Properties.Settings.Default.Width = levels[3].Width;
                Properties.Settings.Default.Height = levels[3].Height;
                Properties.Settings.Default.Mines = levels[3].Mines;
                Properties.Settings.Default.LastUsedLevelName = "User";
                Properties.Settings.Default.Save();


                StartGame(Properties.Settings.Default.LastUsedLevelName);
            }

        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame(Properties.Settings.Default.LastUsedLevelName);
        }

        private void рекордыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HighScores h = new HighScores();
            h.ShowDialog();
        }
    }
}
