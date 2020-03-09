namespace MCTS_Othello
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.botOneLabel = new System.Windows.Forms.Label();
            this.botOneComboBox = new System.Windows.Forms.ComboBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.startButton = new System.Windows.Forms.Button();
            this.currentPlayerLabel = new System.Windows.Forms.Label();
            this.playerOneScoreLabel = new System.Windows.Forms.Label();
            this.playerTwoScoreLabel = new System.Windows.Forms.Label();
            this.restartButton = new System.Windows.Forms.Button();
            this.gameTypeLabel = new System.Windows.Forms.Label();
            this.gameTypeComboBox = new System.Windows.Forms.ComboBox();
            this.botTwoLabel = new System.Windows.Forms.Label();
            this.botTwoComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(573, 30);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(400, 400);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // botOneLabel
            // 
            this.botOneLabel.AutoSize = true;
            this.botOneLabel.Location = new System.Drawing.Point(15, 78);
            this.botOneLabel.Name = "botOneLabel";
            this.botOneLabel.Size = new System.Drawing.Size(68, 13);
            this.botOneLabel.TabIndex = 1;
            this.botOneLabel.Text = "Bot1 settings";
            // 
            // botOneComboBox
            // 
            this.botOneComboBox.FormattingEnabled = true;
            this.botOneComboBox.Items.AddRange(new object[] {
            "MCTS",
            "Random"});
            this.botOneComboBox.Location = new System.Drawing.Point(100, 78);
            this.botOneComboBox.Name = "botOneComboBox";
            this.botOneComboBox.Size = new System.Drawing.Size(121, 21);
            this.botOneComboBox.TabIndex = 7;
            this.botOneComboBox.SelectedIndexChanged += new System.EventHandler(this.botOneComboBox_SelectedIndexChanged);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(492, 30);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 10;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // currentPlayerLabel
            // 
            this.currentPlayerLabel.AutoSize = true;
            this.currentPlayerLabel.Location = new System.Drawing.Point(574, 12);
            this.currentPlayerLabel.Name = "currentPlayerLabel";
            this.currentPlayerLabel.Size = new System.Drawing.Size(39, 13);
            this.currentPlayerLabel.TabIndex = 11;
            this.currentPlayerLabel.Text = "Player:";
            // 
            // playerOneScoreLabel
            // 
            this.playerOneScoreLabel.AutoSize = true;
            this.playerOneScoreLabel.Location = new System.Drawing.Point(718, 12);
            this.playerOneScoreLabel.Name = "playerOneScoreLabel";
            this.playerOneScoreLabel.Size = new System.Drawing.Size(65, 13);
            this.playerOneScoreLabel.TabIndex = 12;
            this.playerOneScoreLabel.Text = "black score:";
            // 
            // playerTwoScoreLabel
            // 
            this.playerTwoScoreLabel.AutoSize = true;
            this.playerTwoScoreLabel.Location = new System.Drawing.Point(870, 12);
            this.playerTwoScoreLabel.Name = "playerTwoScoreLabel";
            this.playerTwoScoreLabel.Size = new System.Drawing.Size(64, 13);
            this.playerTwoScoreLabel.TabIndex = 13;
            this.playerTwoScoreLabel.Text = "white score:";
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(492, 55);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(75, 23);
            this.restartButton.TabIndex = 14;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // gameTypeLabel
            // 
            this.gameTypeLabel.AutoSize = true;
            this.gameTypeLabel.Location = new System.Drawing.Point(15, 11);
            this.gameTypeLabel.Name = "gameTypeLabel";
            this.gameTypeLabel.Size = new System.Drawing.Size(64, 13);
            this.gameTypeLabel.TabIndex = 15;
            this.gameTypeLabel.Text = "Game type: ";
            // 
            // gameTypeComboBox
            // 
            this.gameTypeComboBox.FormattingEnabled = true;
            this.gameTypeComboBox.Items.AddRange(new object[] {
            "Human vs. Human",
            "Human vs. Computer",
            "Computer vs. Computer"});
            this.gameTypeComboBox.Location = new System.Drawing.Point(85, 9);
            this.gameTypeComboBox.Name = "gameTypeComboBox";
            this.gameTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.gameTypeComboBox.TabIndex = 16;
            this.gameTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.gameTypeComboBox_SelectedIndexChanged);
            // 
            // botTwoLabel
            // 
            this.botTwoLabel.AutoSize = true;
            this.botTwoLabel.Location = new System.Drawing.Point(15, 112);
            this.botTwoLabel.Name = "botTwoLabel";
            this.botTwoLabel.Size = new System.Drawing.Size(68, 13);
            this.botTwoLabel.TabIndex = 17;
            this.botTwoLabel.Text = "Bot2 settings";
            // 
            // botTwoComboBox
            // 
            this.botTwoComboBox.FormattingEnabled = true;
            this.botTwoComboBox.Items.AddRange(new object[] {
            "MCTS",
            "Random"});
            this.botTwoComboBox.Location = new System.Drawing.Point(100, 112);
            this.botTwoComboBox.Name = "botTwoComboBox";
            this.botTwoComboBox.Size = new System.Drawing.Size(121, 21);
            this.botTwoComboBox.TabIndex = 18;
            this.botTwoComboBox.SelectedIndexChanged += new System.EventHandler(this.botTwoComboBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 442);
            this.Controls.Add(this.botTwoComboBox);
            this.Controls.Add(this.botTwoLabel);
            this.Controls.Add(this.gameTypeComboBox);
            this.Controls.Add(this.gameTypeLabel);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.playerTwoScoreLabel);
            this.Controls.Add(this.playerOneScoreLabel);
            this.Controls.Add(this.currentPlayerLabel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.botOneComboBox);
            this.Controls.Add(this.botOneLabel);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label botOneLabel;
        private System.Windows.Forms.ComboBox botOneComboBox;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label currentPlayerLabel;
        private System.Windows.Forms.Label playerOneScoreLabel;
        private System.Windows.Forms.Label playerTwoScoreLabel;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Label gameTypeLabel;
        private System.Windows.Forms.ComboBox gameTypeComboBox;
        private System.Windows.Forms.Label botTwoLabel;
        private System.Windows.Forms.ComboBox botTwoComboBox;
    }
}

