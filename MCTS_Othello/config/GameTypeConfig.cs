using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS_Othello.config
{
    class GameTypeConfig
    {
        public static void Config(int selectIdx, Form1 form)
        {
            switch (selectIdx)
            {
                case 0: // "Human vs. Human"
                    /* enable. */
                    form.SetControlStatus(form.startButton, true);
                    form.startButton.Enabled = true;
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
    }
}
