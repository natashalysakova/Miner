using System;
using System.Drawing;
using System.Windows.Forms;
using Miner.Properties;

namespace Miner
{
    internal class Game
    {
        private const int Pressed = 0;
        private const int Empty = 0;
        private readonly Image[] _images;
        public int Cellsize = 30;
        public bool IsWin;
        private int[][] _field;

        private int _fieldHeight;
        private int _fieldWidth;
        private PictureBox _graphicField;
        private int _minesCount;
        private State[][] _state;
        private Point _pressedCell;

        public Game()
        {
            _images = new Image[15];
            _images[0] = Resources.empty;
            _images[1] = Resources.one;
            _images[2] = Resources.two;
            _images[3] = Resources.three;
            _images[4] = Resources.four;
            _images[5] = Resources.five;
            _images[6] = Resources.six;
            _images[7] = Resources.seven;
            _images[8] = Resources.eight;
            _images[9] = Resources.nine;
            _images[10] = Resources.butt;
            _images[11] = Resources.explosion;
            _images[12] = Resources.flag;
            _images[13] = Resources.mine;
            _images[14] = Resources.wrong;

            WinSmiles = new Image[]
            {
                Resources.smiles_0002_Layer_1_copy_11, Resources.smiles_0013_Layer_1,
                Resources.smiles_0013_Layer_1_copy_17, Resources.smiles_0014_Layer_1_copy_16,
                Resources.smiles_0015_Layer_1_copy_15, Resources.smiles_0016_Layer_1_copy_13
            };

            MoveSmiles = new Image[]
            {
                Resources.smiles_0006_Layer_1_copy_7, Resources.smiles_0007_Layer_1_copy_6,
                Resources.smiles_0009_Layer_1_copy_4,
                Resources.smiles_0010_Layer_1_copy_3, Resources.smiles_0011_Layer_1_copy_2,
                Resources.smiles_0000_Layer_1_copy_14
            };

            LoseSmiles = new Image[]
            {
                Resources.smiles_0001_Layer_1_copy_12,
                Resources.smiles_0003_Layer_1_copy_10, Resources.smiles_0004_Layer_1_copy_9,
                Resources.smiles_0005_Layer_1_copy_8,
                Resources.smiles_0008_Layer_1_copy_5,
                Resources.smiles_0012_Layer_1_copy
            };
        }

        public Image[] WinSmiles { get; private set; }
        public Image[] MoveSmiles { get; private set; }
        public Image[] LoseSmiles { get; private set; }

        public bool IsOver { get; private set; }

        public void NewGame(PictureBox field, int width, int height, int minesCount)
        {
            _graphicField = field;
            _fieldWidth = width;
            _fieldHeight = height;
            _minesCount = minesCount;
            _pressedCell = new Point(-1, -1);

            GenerateField();


            _state = new State[_fieldWidth][];
            for (int i = 0; i < _fieldWidth; i++)
            {
                _state[i] = new State[_fieldHeight];
                for (int j = 0; j < _fieldHeight; j++)
                {
                    _state[i][j] = State.Closed;
                }
            }
        }

        private void GenerateField()
        {
            var random = new Random();
            _field = new int[_fieldWidth][];
            for (int i = 0; i < _field.Length; i++)
            {
                _field[i] = new int[_fieldHeight];
            }

            for (int i = 0; i < _minesCount; i++)
            {
                int x = random.Next(0, _fieldWidth);
                int y = random.Next(0, _fieldHeight);
                if(_field[x][y] != (int)State.Mine)
                    _field[x][y] = (int) State.Mine;
                else
                {
                    i--;
                }
            }

            for (int i = 0; i < _fieldWidth; i++)
            {
                for (int j = 0; j < _fieldHeight; j++)
                {
                    if (_field[i][j] != (int) State.Mine)
                    {
                        _field[i][j] = CalcCount(i, j);
                    }
                }
            }
        }

        private int CalcCount(int i, int j)
        {
            int count = 0;


            for (int k = i - 1; k < i + 2; k++)
            {
                for (int l = j - 1; l < j + 2; l++)
                {
                    if (k != -1 && l != -1 && k != _fieldWidth && l != _fieldHeight)
                        if (_field[k][l] == (int) State.Mine)
                            count++;
                }
            }


            return count;
        }

        public void DrawField()
        {
            var fieldBitmap = new Bitmap(Cellsize*_fieldWidth, Cellsize*_fieldHeight);
            Graphics g = Graphics.FromImage(fieldBitmap);
            for (int i = 0; i < _fieldWidth; i++)
            {
                for (int j = 0; j < _fieldHeight; j++)
                {
                    if (_state[i][j] == State.Closed)
                        g.DrawImageUnscaledAndClipped(_images[(int) State.Closed],
                            new Rectangle(i*Cellsize, j*Cellsize, Cellsize, Cellsize));

                    if (_state[i][j] == State.Flag)
                        g.DrawImageUnscaledAndClipped(_images[(int) State.Flag],
                            new Rectangle(i*Cellsize, j*Cellsize, Cellsize, Cellsize));

                    if (_state[i][j] == State.Open)
                    {
                        int val = _field[i][j];
                        g.DrawImageUnscaledAndClipped(_images[val],
                            new Rectangle(i*Cellsize, j*Cellsize, Cellsize, Cellsize));
                    }

                    if (_state[i][j] == State.Explosion)
                        g.DrawImageUnscaledAndClipped(_images[(int) State.Explosion],
                            new Rectangle(i*Cellsize, j*Cellsize, Cellsize, Cellsize));


                    if (_state[i][j] == State.Wrong)
                        g.DrawImageUnscaledAndClipped(_images[(int) State.Wrong],
                            new Rectangle(i*Cellsize, j*Cellsize, Cellsize, Cellsize));

                    if (_pressedCell.X == i && _pressedCell.Y == j)
                        g.DrawImageUnscaledAndClipped(_images[Pressed],
                            new Rectangle(i*Cellsize, j*Cellsize, Cellsize, Cellsize));
                }
            }


            _graphicField.Image = fieldBitmap;
        }

        internal void SetPressed(Point point)
        {
            int x = point.X/Cellsize;
            int y = point.Y/Cellsize;
            if (x < _fieldWidth && y < _fieldHeight)
            {
                if (_state[x][y] == State.Closed)
                {
                    _pressedCell = new Point(x, y);
                }
            }
        }

        internal void ResetPressed()
        {
            _pressedCell = new Point(-1, -1);
        }

        internal void OpenCell(Point point)
        {
            int x = point.X/Cellsize;
            int y = point.Y/Cellsize;
            if (x < _fieldWidth && y < _fieldHeight)
            {
                if (_state[x][y] == State.Closed || _state[x][y] == State.Flag)
                {
                    _state[x][y] = State.Open;

                    if (_field[x][y] == Empty)
                    {
                        FindAllEmptyCell(x, y);

                        for (int i = 0; i < (_fieldHeight*_fieldWidth)/3; i++)
                        {
                            OpenHelpingCell();
                        }
                    }

                    if (_field[x][y] == (int) State.Mine)
                    {
                        _state[x][y] = State.Explosion;
                        IsOver = true;

                        IsWin = false;
                        OpenAllField();
                    }
                    else
                    {
                        IsOver = IsGameOver();

                        if (IsOver)
                            IsWin = true;
                    }
                }
            }
        }

        private bool IsGameOver()
        {
            bool isOver = true;

            for (int i = 0; i < _fieldWidth; i++)
            {
                for (int j = 0; j < _fieldHeight; j++)
                {
                    if (_state[i][j] == State.Closed)
                    {
                        isOver = false;
                        break;
                    }
                }
            }

            //TODO: fix on isOver

            return isOver;
        }

        private void OpenHelpingCell()
        {
            for (int i = 0; i < _fieldWidth; i++)
            {
                for (int j = 0; j < _fieldHeight; j++)
                {
                    if (_field[i][j] == Empty && _state[i][j] == State.Open)
                    {
                        OpenNeibours(i, j);
                    }
                }
            }
        }

        private void OpenNeibours(int i, int j)
        {
            for (int k = i - 1; k < i + 2; k++)
            {
                for (int l = j - 1; l < j + 2; l++)
                {
                    if (k > -1 && l > -1 && k < _fieldWidth && l < _fieldHeight)
                    {
                        _state[k][l] = State.Open;
                    }
                }
            }
        }

        private void FindAllEmptyCell(int x, int y)
        {
            if (_field[x][y] == Empty && _state[x][y] != State.Open)
            {
                _state[x][y] = State.Open;

                if (x != 0)
                {
                    FindAllEmptyCell(x - 1, y);
                }
                if (y != 0)
                {
                    FindAllEmptyCell(x, y - 1);
                }
                if (x != _fieldWidth - 1)
                {
                    FindAllEmptyCell(x + 1, y);
                }
                if (y != _fieldHeight - 1)
                {
                    FindAllEmptyCell(x, y + 1);
                }
            }
        }

        private void OpenAllField()
        {
            for (int i = 0; i < _fieldWidth; i++)
            {
                for (int j = 0; j < _fieldHeight; j++)
                {
                    if (_field[i][j] != (int) State.Mine && _state[i][j] == State.Flag)
                    {
                        _state[i][j] = State.Wrong;
                    }
                    else
                    {
                        if (_state[i][j] != State.Explosion)
                            _state[i][j] = State.Open;
                    }
                }
            }
        }

        internal void SetFlag(Point point)
        {
            int x = point.X/Cellsize;
            int y = point.Y/Cellsize;

            if (_state[x][y] == State.Closed)
                _state[x][y] = State.Flag;
            else if (_state[x][y] == State.Flag)
                _state[x][y] = State.Closed;
        }
    }

    internal enum State
    {
        Mine = 13,
        Closed = 10,
        Explosion = 11,
        Flag = 12,
        Wrong = 14,
        Open = 15,
    }

    internal struct Level
    {
        public int Height;
        public int Mines;
        public string Name;
        public int Width;


        public Level(string name, int width, int height, int mines)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.Mines = mines;
        }
    }
}