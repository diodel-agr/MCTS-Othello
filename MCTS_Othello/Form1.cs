using MCTS_Othello.game;
using MCTS_Othello.ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

//https://project.dke.maastrichtuniversity.nl/games/files/phd/Chaslot_thesis.pdf
namespace MCTS_Othello
{
    public partial class Form1 : Form
    {
        IMCTSGame game;
        Bitmap boardImage;
        Bitmap blackPiece;
        Bitmap whitePiece;
        Bitmap optionPiece;
        string botOne, botTwo;
        CancellationTokenSource cancelToken;
        delegate void uiUpdateDelegate(object token);
        public Form1()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(ReleaseResources);
            boardImage = (Bitmap)Image.FromFile("board.png");
            blackPiece = (Bitmap)Image.FromFile("black.png");
            whitePiece = (Bitmap)Image.FromFile("white.png");
            optionPiece = (Bitmap)Image.FromFile("option.png");
            /* disable all buttons and controls. */
            startButton.Enabled = false;
            restartButton.Enabled = false;
            botOneLabel.Enabled = false;
            botTwoLabel.Enabled = false;
            botOneComboBox.Enabled = false;
            botTwoComboBox.Enabled = false;
            botOne = botTwo = null;
            pictureBox.Refresh();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            /* set game type. */
            int gameIdx = gameTypeComboBox.SelectedIndex;
            switch (gameIdx)
            {
                case 0: // "Human vs. Human":
                    game = new HHGame();
                    break;
                case 1: // "Human vs. Computer":
                    game = new HCGame(botOne);
                    break;
                case 2: // "Computer vs. Computer":
                    game = new CCGame(botOne, botTwo);
                    break;
            }
            game.Start();
            
            pictureBox.Size = boardImage.Size;
            pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            Type a = game.GetType(), b = typeof(HHGame), c = typeof(HCGame);
            if (a.Equals(c) == true)
            {
                pictureBox.MouseUp += new MouseEventHandler(this.pictureBox_MouseUp);
                //new Thread(new ThreadStart(LaunchUpdateAuto)).Start();
            }
            else if (a.Equals(b) == true)// || a.Equals(c) == true)
            {
                pictureBox.MouseUp += new MouseEventHandler(this.pictureBox_MouseUp);
                //new Thread(new ThreadStart(LaunchUpdateAuto)).Start();
                
            }
            else
            {   /* launch a thread that will update the ui. */
                new Thread(new ThreadStart(LaunchUpdateAuto)).Start();
            }
            pictureBox.Refresh();
        }

        private void pictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int offsetX = 2, offsetY = 2;
            int mulX = 50, mulY = 50;
            /* draw board. */
            Bitmap board = boardImage;
            e.Graphics.DrawImage(board, 0, 0);
            /* draw pieces. */
            Piece[,] pieces = game.GetPieces();
            foreach (Piece p in pieces)
            {
                if (p != null && p.owner != null)
                {
                    if (p.owner.GetColor() == MCTS_Othello.player.Color.black)
                    {
                        e.Graphics.DrawImage(blackPiece, offsetX + p.X * mulX, offsetY + p.Y * mulY);
                    }
                    else if (p.owner.GetColor() == MCTS_Othello.player.Color.white)
                    {
                        e.Graphics.DrawImage(whitePiece, offsetX + p.X * mulX, offsetY + p.Y * mulY);
                    }
                }
            }
            /* show posible positions. */
            List<Piece> options = game.GetOptions();
            if (options != null)
            {
                foreach (Piece p in options)
                {
                    e.Graphics.DrawImage(optionPiece, offsetX + p.X * mulX, offsetY + p.Y * mulY);
                }
            }
        }

        private void pictureBox_Click(object sender, MouseEventArgs e) { }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            /* get the coords of the clicked tile. */
            int cursorX = (e.X - 2) / 50, cursorY = (e.Y - 2) / 50;
            
            if (cursorX >= 0 && cursorY >= 0)
            {
                game.PlayerClicked(cursorX, cursorY);
            }
            else
            {
                throw new MCTSException("[Form/pictureBox_MouseUp] - negative coords.");
            }
            /* refresh. */
            UpdateUI();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            game.RestartGame();
            pictureBox.Refresh();
        }

        private void UpdateUI()
        {
            /* update current player and score. */
            currentPlayerLabel.Text = "Current player: " + game.GetCurrentPlayer().GetColor().ToString();
            playerOneScoreLabel.Text = game.GetPlayer(1).GetColor().ToString() + " score: " + game.GetScore(1);
            playerTwoScoreLabel.Text = game.GetPlayer(2).GetColor().ToString() + " score: " + game.GetScore(2);
            /* check if the game is finished. */
            if (game.IsFinished() == true)
            {
                currentPlayerLabel.Text = game.GetGameResult();
                game.SetGameState(GameState.stopped);
            }
            pictureBox.Refresh();
        }

        private void gameTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (gameTypeComboBox.SelectedIndex)
            {
                case 0: // "Human vs. Human"
                    /* enable. */
                    startButton.Enabled = true;
                    restartButton.Enabled = true;
                    /* disable. */
                    botOneLabel.Enabled = false;
                    botOneComboBox.Enabled = false;
                    botTwoLabel.Enabled = false;
                    botTwoComboBox.Enabled = false;
                    botOne = botTwo = null;
                    break;
                case 1: // "Human vs. Computer"
                    /* enable. */
                    botOneLabel.Enabled = true;
                    botOneComboBox.Enabled = true;
                    /* disable. */
                    startButton.Enabled = false;
                    restartButton.Enabled = false;
                    botTwoLabel.Enabled = false;
                    botTwoComboBox.Enabled = false;
                    botTwo = "";
                    break;
                case 2: // "Computer vs. Computer"
                    /* enable. */
                    botOneLabel.Enabled = true;
                    botOneComboBox.Enabled = true;
                    botTwoLabel.Enabled = true;
                    botTwoComboBox.Enabled = true;
                    /* disbale. */
                    startButton.Enabled = false;
                    restartButton.Enabled = false;
                    break;
            }
        }

        private void botTwoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (botTwoComboBox.SelectedIndex)
            {
                case 0: // MCTS.
                    botTwo = "MCTS";
                    break;
                case 1: // random.
                    botTwo = "Random";
                    break;
                default:
                    botTwo = null;
                    break;
            }
            if (botOne != null)
            {
                startButton.Enabled = true;
                restartButton.Enabled = true;
            }
        }

        private void botOneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (botOneComboBox.SelectedIndex)
            {
                case 0: // MCTS.
                    botOne = "MCTS";
                    break;
                case 1: // random.
                    botOne = "Random";
                    break;
                default:
                    botOne = null;
                    break;
            }
            if (botTwo != null)
            {
                startButton.Enabled = true;
                restartButton.Enabled = true;
            }
        }

        private void UpdateUIAuto(object token)
        {
            Console.WriteLine("UI Update auto thread start.");
            CancellationTokenSource cancelToken = (CancellationTokenSource)token;
            while (game.IsFinished() == false && cancelToken.IsCancellationRequested == false)
            {
                UpdateUI();
                Thread.Sleep(500);
            }
            UpdateUI();
            Console.WriteLine("UI Update thread exit.");
        }

        private void LaunchUpdateAuto()
        {
            cancelToken = new CancellationTokenSource();
            uiUpdateDelegate d = new uiUpdateDelegate(this.UpdateUIAuto);
            this.Invoke(d, new object[] { cancelToken });
        }
        // de reparat functia asta.
        protected void ReleaseResources(object sender, FormClosingEventArgs e)
        {
            game.SetGameState(GameState.stopped);
            cancelToken.Cancel();
            Console.WriteLine("Cancel request.");
        }
    }
}
