namespace VorWOZ
{
    partial class VorWOZForm
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
            this.PanicSentences = new System.Windows.Forms.ListBox();
            this.PanicSendButton = new System.Windows.Forms.Button();
            this.PanicBox = new System.Windows.Forms.GroupBox();
            this.PanicTextBox = new System.Windows.Forms.TextBox();
            this.RobotStateLabel = new System.Windows.Forms.Label();
            this.RobotState = new System.Windows.Forms.Label();
            this.LibrariesBox = new System.Windows.Forms.GroupBox();
            this.GetLibrariesButton = new System.Windows.Forms.Button();
            this.LibrarySelected = new System.Windows.Forms.Label();
            this.ChangeLibraryButton = new System.Windows.Forms.Button();
            this.LibrariesList = new System.Windows.Forms.ListBox();
            this.LibCategories = new System.Windows.Forms.ListBox();
            this.LibSubCategories = new System.Windows.Forms.ListBox();
            this.PerformUtterance = new System.Windows.Forms.Button();
            this.GazeBox = new System.Windows.Forms.GroupBox();
            this.GazeState = new System.Windows.Forms.Label();
            this.GazeStateLabel = new System.Windows.Forms.Label();
            this.GazeConfederate = new System.Windows.Forms.Button();
            this.GazeGame = new System.Windows.Forms.Button();
            this.GazeElsewhere = new System.Windows.Forms.Button();
            this.GazeParticipant = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TagValuesLabel = new System.Windows.Forms.Label();
            this.TagValuesList = new System.Windows.Forms.TextBox();
            this.TagNamesLabel = new System.Windows.Forms.Label();
            this.TagNamesList = new System.Windows.Forms.TextBox();
            this.UpdateTagValues = new System.Windows.Forms.Button();
            this.PanicBox.SuspendLayout();
            this.LibrariesBox.SuspendLayout();
            this.GazeBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanicSentences
            // 
            this.PanicSentences.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.PanicSentences.FormattingEnabled = true;
            this.PanicSentences.ItemHeight = 25;
            this.PanicSentences.Items.AddRange(new object[] {
            "Concentrem-se no jogo por favor!",
            "Não faças isso por favor.",
            "Não se distraiam por favor.",
            "Vamos lá pessoal."});
            this.PanicSentences.Location = new System.Drawing.Point(6, 19);
            this.PanicSentences.Name = "PanicSentences";
            this.PanicSentences.Size = new System.Drawing.Size(319, 104);
            this.PanicSentences.Sorted = true;
            this.PanicSentences.TabIndex = 3;
            // 
            // PanicSendButton
            // 
            this.PanicSendButton.Location = new System.Drawing.Point(331, 19);
            this.PanicSendButton.Name = "PanicSendButton";
            this.PanicSendButton.Size = new System.Drawing.Size(75, 100);
            this.PanicSendButton.TabIndex = 4;
            this.PanicSendButton.Text = "Send";
            this.PanicSendButton.UseVisualStyleBackColor = true;
            this.PanicSendButton.Click += new System.EventHandler(this.PanicSendButton_Click);
            // 
            // PanicBox
            // 
            this.PanicBox.Controls.Add(this.PanicTextBox);
            this.PanicBox.Controls.Add(this.PanicSentences);
            this.PanicBox.Controls.Add(this.PanicSendButton);
            this.PanicBox.Location = new System.Drawing.Point(1032, 540);
            this.PanicBox.Name = "PanicBox";
            this.PanicBox.Size = new System.Drawing.Size(412, 167);
            this.PanicBox.TabIndex = 5;
            this.PanicBox.TabStop = false;
            this.PanicBox.Text = "Panic Sentences";
            // 
            // PanicTextBox
            // 
            this.PanicTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.PanicTextBox.Location = new System.Drawing.Point(6, 126);
            this.PanicTextBox.Name = "PanicTextBox";
            this.PanicTextBox.Size = new System.Drawing.Size(400, 29);
            this.PanicTextBox.TabIndex = 5;
            // 
            // RobotStateLabel
            // 
            this.RobotStateLabel.AutoSize = true;
            this.RobotStateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RobotStateLabel.Location = new System.Drawing.Point(11, 8);
            this.RobotStateLabel.Name = "RobotStateLabel";
            this.RobotStateLabel.Size = new System.Drawing.Size(67, 13);
            this.RobotStateLabel.TabIndex = 6;
            this.RobotStateLabel.Text = "Robot State:";
            // 
            // RobotState
            // 
            this.RobotState.AutoSize = true;
            this.RobotState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RobotState.Location = new System.Drawing.Point(76, 8);
            this.RobotState.Name = "RobotState";
            this.RobotState.Size = new System.Drawing.Size(16, 13);
            this.RobotState.TabIndex = 7;
            this.RobotState.Text = "...";
            // 
            // LibrariesBox
            // 
            this.LibrariesBox.Controls.Add(this.GetLibrariesButton);
            this.LibrariesBox.Controls.Add(this.LibrarySelected);
            this.LibrariesBox.Controls.Add(this.ChangeLibraryButton);
            this.LibrariesBox.Controls.Add(this.LibrariesList);
            this.LibrariesBox.Location = new System.Drawing.Point(1196, 12);
            this.LibrariesBox.Name = "LibrariesBox";
            this.LibrariesBox.Size = new System.Drawing.Size(248, 190);
            this.LibrariesBox.TabIndex = 10;
            this.LibrariesBox.TabStop = false;
            this.LibrariesBox.Text = "Libraries";
            // 
            // GetLibrariesButton
            // 
            this.GetLibrariesButton.Location = new System.Drawing.Point(130, 114);
            this.GetLibrariesButton.Name = "GetLibrariesButton";
            this.GetLibrariesButton.Size = new System.Drawing.Size(112, 67);
            this.GetLibrariesButton.TabIndex = 3;
            this.GetLibrariesButton.Text = "Get Libraries";
            this.GetLibrariesButton.UseVisualStyleBackColor = true;
            this.GetLibrariesButton.Click += new System.EventHandler(this.GetLibrariesButton_Click);
            // 
            // LibrarySelected
            // 
            this.LibrarySelected.AutoSize = true;
            this.LibrarySelected.ForeColor = System.Drawing.Color.Red;
            this.LibrarySelected.Location = new System.Drawing.Point(6, 16);
            this.LibrarySelected.Name = "LibrarySelected";
            this.LibrarySelected.Size = new System.Drawing.Size(23, 13);
            this.LibrarySelected.TabIndex = 2;
            this.LibrarySelected.Text = "null";
            // 
            // ChangeLibraryButton
            // 
            this.ChangeLibraryButton.Enabled = false;
            this.ChangeLibraryButton.Location = new System.Drawing.Point(6, 114);
            this.ChangeLibraryButton.Name = "ChangeLibraryButton";
            this.ChangeLibraryButton.Size = new System.Drawing.Size(118, 67);
            this.ChangeLibraryButton.TabIndex = 1;
            this.ChangeLibraryButton.Text = "Change";
            this.ChangeLibraryButton.UseVisualStyleBackColor = true;
            this.ChangeLibraryButton.Click += new System.EventHandler(this.ChangeLibraryButton_Click);
            // 
            // LibrariesList
            // 
            this.LibrariesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LibrariesList.FormattingEnabled = true;
            this.LibrariesList.ItemHeight = 24;
            this.LibrariesList.Location = new System.Drawing.Point(6, 32);
            this.LibrariesList.Name = "LibrariesList";
            this.LibrariesList.Size = new System.Drawing.Size(236, 76);
            this.LibrariesList.TabIndex = 0;
            this.LibrariesList.SelectedIndexChanged += new System.EventHandler(this.LibrariesList_SelectedIndexChanged);
            // 
            // LibCategories
            // 
            this.LibCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LibCategories.FormattingEnabled = true;
            this.LibCategories.ItemHeight = 24;
            this.LibCategories.Location = new System.Drawing.Point(12, 28);
            this.LibCategories.Name = "LibCategories";
            this.LibCategories.Size = new System.Drawing.Size(291, 676);
            this.LibCategories.TabIndex = 11;
            this.LibCategories.SelectedIndexChanged += new System.EventHandler(this.LibCategories_SelectedIndexChanged);
            // 
            // LibSubCategories
            // 
            this.LibSubCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LibSubCategories.FormattingEnabled = true;
            this.LibSubCategories.ItemHeight = 24;
            this.LibSubCategories.Location = new System.Drawing.Point(309, 28);
            this.LibSubCategories.Name = "LibSubCategories";
            this.LibSubCategories.Size = new System.Drawing.Size(283, 676);
            this.LibSubCategories.TabIndex = 12;
            // 
            // PerformUtterance
            // 
            this.PerformUtterance.Location = new System.Drawing.Point(598, 28);
            this.PerformUtterance.Name = "PerformUtterance";
            this.PerformUtterance.Size = new System.Drawing.Size(100, 100);
            this.PerformUtterance.TabIndex = 13;
            this.PerformUtterance.Text = "Perform Utterance";
            this.PerformUtterance.UseVisualStyleBackColor = true;
            this.PerformUtterance.Click += new System.EventHandler(this.PerformUtterance_Click);
            // 
            // GazeBox
            // 
            this.GazeBox.Controls.Add(this.GazeState);
            this.GazeBox.Controls.Add(this.GazeStateLabel);
            this.GazeBox.Controls.Add(this.GazeConfederate);
            this.GazeBox.Controls.Add(this.GazeGame);
            this.GazeBox.Controls.Add(this.GazeElsewhere);
            this.GazeBox.Controls.Add(this.GazeParticipant);
            this.GazeBox.Location = new System.Drawing.Point(1032, 244);
            this.GazeBox.Name = "GazeBox";
            this.GazeBox.Size = new System.Drawing.Size(412, 290);
            this.GazeBox.TabIndex = 14;
            this.GazeBox.TabStop = false;
            this.GazeBox.Text = "GazeBox";
            // 
            // GazeState
            // 
            this.GazeState.AutoSize = true;
            this.GazeState.Location = new System.Drawing.Point(75, 28);
            this.GazeState.Name = "GazeState";
            this.GazeState.Size = new System.Drawing.Size(16, 13);
            this.GazeState.TabIndex = 5;
            this.GazeState.Text = "...";
            // 
            // GazeStateLabel
            // 
            this.GazeStateLabel.AutoSize = true;
            this.GazeStateLabel.Location = new System.Drawing.Point(6, 28);
            this.GazeStateLabel.Name = "GazeStateLabel";
            this.GazeStateLabel.Size = new System.Drawing.Size(63, 13);
            this.GazeStateLabel.TabIndex = 4;
            this.GazeStateLabel.Text = "Gaze State:";
            // 
            // GazeConfederate
            // 
            this.GazeConfederate.Location = new System.Drawing.Point(9, 47);
            this.GazeConfederate.Name = "GazeConfederate";
            this.GazeConfederate.Size = new System.Drawing.Size(123, 113);
            this.GazeConfederate.TabIndex = 3;
            this.GazeConfederate.Text = "Gaze Confederate";
            this.GazeConfederate.UseVisualStyleBackColor = true;
            this.GazeConfederate.Click += new System.EventHandler(this.GazeConfederate_Click);
            // 
            // GazeGame
            // 
            this.GazeGame.Location = new System.Drawing.Point(138, 166);
            this.GazeGame.Name = "GazeGame";
            this.GazeGame.Size = new System.Drawing.Size(123, 113);
            this.GazeGame.TabIndex = 2;
            this.GazeGame.Text = "Gaze Game";
            this.GazeGame.UseVisualStyleBackColor = true;
            this.GazeGame.Click += new System.EventHandler(this.GazeGame_Click);
            // 
            // GazeElsewhere
            // 
            this.GazeElsewhere.Location = new System.Drawing.Point(9, 166);
            this.GazeElsewhere.Name = "GazeElsewhere";
            this.GazeElsewhere.Size = new System.Drawing.Size(123, 113);
            this.GazeElsewhere.TabIndex = 1;
            this.GazeElsewhere.Text = "Gaze Elsewhere";
            this.GazeElsewhere.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.GazeElsewhere.UseVisualStyleBackColor = true;
            this.GazeElsewhere.Click += new System.EventHandler(this.GazeElsewhere_Click);
            // 
            // GazeParticipant
            // 
            this.GazeParticipant.Location = new System.Drawing.Point(138, 47);
            this.GazeParticipant.Name = "GazeParticipant";
            this.GazeParticipant.Size = new System.Drawing.Size(123, 113);
            this.GazeParticipant.TabIndex = 0;
            this.GazeParticipant.Text = "Gaze Participant";
            this.GazeParticipant.UseVisualStyleBackColor = true;
            this.GazeParticipant.Click += new System.EventHandler(this.GazeParticipant_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UpdateTagValues);
            this.groupBox1.Controls.Add(this.TagValuesLabel);
            this.groupBox1.Controls.Add(this.TagValuesList);
            this.groupBox1.Controls.Add(this.TagNamesLabel);
            this.groupBox1.Controls.Add(this.TagNamesList);
            this.groupBox1.Location = new System.Drawing.Point(598, 366);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 341);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // TagValuesLabel
            // 
            this.TagValuesLabel.AutoSize = true;
            this.TagValuesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.TagValuesLabel.Location = new System.Drawing.Point(293, 20);
            this.TagValuesLabel.Name = "TagValuesLabel";
            this.TagValuesLabel.Size = new System.Drawing.Size(113, 25);
            this.TagValuesLabel.TabIndex = 3;
            this.TagValuesLabel.Text = "Tag Values";
            // 
            // TagValuesList
            // 
            this.TagValuesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.TagValuesList.Location = new System.Drawing.Point(220, 51);
            this.TagValuesList.Multiline = true;
            this.TagValuesList.Name = "TagValuesList";
            this.TagValuesList.Size = new System.Drawing.Size(186, 226);
            this.TagValuesList.TabIndex = 2;
            this.TagValuesList.Text = "Fábio\r\nAndré\r\no Fábio\r\no André";
            // 
            // TagNamesLabel
            // 
            this.TagNamesLabel.AutoSize = true;
            this.TagNamesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.TagNamesLabel.Location = new System.Drawing.Point(6, 20);
            this.TagNamesLabel.Name = "TagNamesLabel";
            this.TagNamesLabel.Size = new System.Drawing.Size(114, 25);
            this.TagNamesLabel.TabIndex = 1;
            this.TagNamesLabel.Text = "Tag Names";
            // 
            // TagNamesList
            // 
            this.TagNamesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.TagNamesList.Location = new System.Drawing.Point(6, 51);
            this.TagNamesList.Multiline = true;
            this.TagNamesList.Name = "TagNamesList";
            this.TagNamesList.Size = new System.Drawing.Size(194, 226);
            this.TagNamesList.TabIndex = 0;
            this.TagNamesList.Text = "/participant/\r\n/participantGender/\r\n/confederate/\r\n/confederateGender/";
            // 
            // UpdateTagValues
            // 
            this.UpdateTagValues.Location = new System.Drawing.Point(137, 283);
            this.UpdateTagValues.Name = "UpdateTagValues";
            this.UpdateTagValues.Size = new System.Drawing.Size(150, 52);
            this.UpdateTagValues.TabIndex = 4;
            this.UpdateTagValues.Text = "Update";
            this.UpdateTagValues.UseVisualStyleBackColor = true;
            this.UpdateTagValues.Click += new System.EventHandler(this.UpdateTagValues_Click);
            // 
            // VorWOZForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1456, 719);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GazeBox);
            this.Controls.Add(this.PerformUtterance);
            this.Controls.Add(this.LibSubCategories);
            this.Controls.Add(this.LibCategories);
            this.Controls.Add(this.LibrariesBox);
            this.Controls.Add(this.RobotState);
            this.Controls.Add(this.RobotStateLabel);
            this.Controls.Add(this.PanicBox);
            this.Name = "VorWOZForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vor WOZ";
            this.PanicBox.ResumeLayout(false);
            this.PanicBox.PerformLayout();
            this.LibrariesBox.ResumeLayout(false);
            this.LibrariesBox.PerformLayout();
            this.GazeBox.ResumeLayout(false);
            this.GazeBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox PanicSentences;
        private System.Windows.Forms.Button PanicSendButton;
        private System.Windows.Forms.GroupBox PanicBox;
        private System.Windows.Forms.TextBox PanicTextBox;
        private System.Windows.Forms.Label RobotStateLabel;
        private System.Windows.Forms.Label RobotState;
        private System.Windows.Forms.GroupBox LibrariesBox;
        private System.Windows.Forms.Button ChangeLibraryButton;
        private System.Windows.Forms.ListBox LibrariesList;
        private System.Windows.Forms.Label LibrarySelected;
        private System.Windows.Forms.Button GetLibrariesButton;
        private System.Windows.Forms.ListBox LibCategories;
        private System.Windows.Forms.ListBox LibSubCategories;
        private System.Windows.Forms.Button PerformUtterance;
        private System.Windows.Forms.GroupBox GazeBox;
        private System.Windows.Forms.Label GazeState;
        private System.Windows.Forms.Label GazeStateLabel;
        private System.Windows.Forms.Button GazeConfederate;
        private System.Windows.Forms.Button GazeGame;
        private System.Windows.Forms.Button GazeElsewhere;
        private System.Windows.Forms.Button GazeParticipant;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label TagValuesLabel;
        private System.Windows.Forms.TextBox TagValuesList;
        private System.Windows.Forms.Label TagNamesLabel;
        private System.Windows.Forms.TextBox TagNamesList;
        private System.Windows.Forms.Button UpdateTagValues;
    }
}

