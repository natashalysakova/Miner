using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Miner
{
    public partial class HighScores : Form
    {
        private bool isOpenNewRecord;
        private string time;

        public HighScores(bool mode = false)
        {
            InitializeComponent();
            isOpenNewRecord = mode;
        }

        private void HighScores_Load(object sender, EventArgs e)
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (!Directory.Exists(userFolder + "\\Local\\Miner"))
            {
                Directory.CreateDirectory(userFolder + "\\Local\\Miner");
            }

            string gamePath = userFolder + "\\Local\\Miner";


            StreamReader sr = new StreamReader(new FileStream(gamePath + "\\HighScore.score", FileMode.OpenOrCreate, FileAccess.Read));

            List<string> records = new List<string>();

            int i = 1;
            while (!sr.EndOfStream)
            {
                records.Add(sr.ReadLine());
                listBox1.Items.Add(i + ". " + records.Last());
                i++;
            }

            if (isOpenNewRecord)
            {
                PlayerName p = new PlayerName();
                p.ShowDialog();
                records.Add(time + " " + p.GetPlayerName());

                listBox1.Items.Clear();
                i = 1;

                records.Sort();
                foreach (string record in records)
                {
                    listBox1.Items.Add(i + ". " + record);
                    i++;
                }
            }

            sr.Dispose();

            StreamWriter swWriter = new StreamWriter(new FileStream(gamePath + "\\HighScore.score", FileMode.Truncate, FileAccess.Write));

            foreach (string record in records)
            {
                swWriter.WriteLine(record);
            }

            swWriter.Dispose();

        }

        public void SetTime(string time)
        {
            this.time = time;
        }
    }
}
