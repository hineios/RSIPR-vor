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
            this.GlanceConfederate = new System.Windows.Forms.Button();
            this.GlanceParticipant = new System.Windows.Forms.Button();
            this.GazeState = new System.Windows.Forms.Label();
            this.GazeStateLabel = new System.Windows.Forms.Label();
            this.GazeConfederate = new System.Windows.Forms.Button();
            this.GazeGame = new System.Windows.Forms.Button();
            this.GazeElsewhere = new System.Windows.Forms.Button();
            this.GazeParticipant = new System.Windows.Forms.Button();
            this.TagBox = new System.Windows.Forms.GroupBox();
            this.UpdateTagValues = new System.Windows.Forms.Button();
            this.TagValuesLabel = new System.Windows.Forms.Label();
            this.TagValuesList = new System.Windows.Forms.TextBox();
            this.TagNamesLabel = new System.Windows.Forms.Label();
            this.TagNamesList = new System.Windows.Forms.TextBox();
            this.ThalamusStateLabel = new System.Windows.Forms.Label();
            this.ThalamusState = new System.Windows.Forms.Label();
            this.GameControlsBox = new System.Windows.Forms.GroupBox();
            this.EndGameButton = new System.Windows.Forms.Button();
            this.StartGameButton = new System.Windows.Forms.Button();
            this.GameStateLabel = new System.Windows.Forms.Label();
            this.GameState = new System.Windows.Forms.Label();
            this.FillWordButton = new System.Windows.Forms.Button();
            this.GameFeedbackBox = new System.Windows.Forms.GroupBox();
            this.WordsNotCompletedLabel = new System.Windows.Forms.Label();
            this.WordsNotCompleted = new System.Windows.Forms.ComboBox();
            this.FillWordBool = new System.Windows.Forms.Label();
            this.FillWordNumber = new System.Windows.Forms.Label();
            this.FillWordLabel = new System.Windows.Forms.Label();
            this.PanicBox.SuspendLayout();
            this.LibrariesBox.SuspendLayout();
            this.GazeBox.SuspendLayout();
            this.TagBox.SuspendLayout();
            this.GameControlsBox.SuspendLayout();
            this.GameFeedbackBox.SuspendLayout();
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
            this.PanicBox.Location = new System.Drawing.Point(1031, 622);
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
            this.RobotStateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RobotStateLabel.Location = new System.Drawing.Point(766, 31);
            this.RobotStateLabel.Name = "RobotStateLabel";
            this.RobotStateLabel.Size = new System.Drawing.Size(166, 31);
            this.RobotStateLabel.TabIndex = 6;
            this.RobotStateLabel.Text = "Robot State:";
            // 
            // RobotState
            // 
            this.RobotState.AutoSize = true;
            this.RobotState.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RobotState.Location = new System.Drawing.Point(938, 31);
            this.RobotState.Name = "RobotState";
            this.RobotState.Size = new System.Drawing.Size(38, 31);
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
            this.LibrariesBox.Size = new System.Drawing.Size(248, 235);
            this.LibrariesBox.TabIndex = 10;
            this.LibrariesBox.TabStop = false;
            this.LibrariesBox.Text = "Libraries";
            // 
            // GetLibrariesButton
            // 
            this.GetLibrariesButton.Location = new System.Drawing.Point(127, 162);
            this.GetLibrariesButton.Name = "GetLibrariesButton";
            this.GetLibrariesButton.Size = new System.Drawing.Size(115, 67);
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
            this.ChangeLibraryButton.Location = new System.Drawing.Point(6, 162);
            this.ChangeLibraryButton.Name = "ChangeLibraryButton";
            this.ChangeLibraryButton.Size = new System.Drawing.Size(115, 67);
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
            this.LibrariesList.Size = new System.Drawing.Size(236, 124);
            this.LibrariesList.TabIndex = 0;
            this.LibrariesList.SelectedIndexChanged += new System.EventHandler(this.LibrariesList_SelectedIndexChanged);
            // 
            // LibCategories
            // 
            this.LibCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LibCategories.FormattingEnabled = true;
            this.LibCategories.ItemHeight = 29;
            this.LibCategories.Location = new System.Drawing.Point(12, 31);
            this.LibCategories.Name = "LibCategories";
            this.LibCategories.Size = new System.Drawing.Size(291, 758);
            this.LibCategories.TabIndex = 11;
            this.LibCategories.SelectedIndexChanged += new System.EventHandler(this.LibCategories_SelectedIndexChanged);
            // 
            // LibSubCategories
            // 
            this.LibSubCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LibSubCategories.FormattingEnabled = true;
            this.LibSubCategories.ItemHeight = 29;
            this.LibSubCategories.Location = new System.Drawing.Point(309, 31);
            this.LibSubCategories.Name = "LibSubCategories";
            this.LibSubCategories.Size = new System.Drawing.Size(283, 758);
            this.LibSubCategories.TabIndex = 12;
            this.LibSubCategories.SelectedIndexChanged += new System.EventHandler(this.LibSubCategories_SelectedIndexChanged);
            // 
            // PerformUtterance
            // 
            this.PerformUtterance.Enabled = false;
            this.PerformUtterance.Location = new System.Drawing.Point(598, 31);
            this.PerformUtterance.Name = "PerformUtterance";
            this.PerformUtterance.Size = new System.Drawing.Size(100, 100);
            this.PerformUtterance.TabIndex = 13;
            this.PerformUtterance.Text = "Perform Utterance";
            this.PerformUtterance.UseVisualStyleBackColor = true;
            this.PerformUtterance.Click += new System.EventHandler(this.PerformUtterance_Click);
            // 
            // GazeBox
            // 
            this.GazeBox.Controls.Add(this.GlanceConfederate);
            this.GazeBox.Controls.Add(this.GlanceParticipant);
            this.GazeBox.Controls.Add(this.GazeState);
            this.GazeBox.Controls.Add(this.GazeStateLabel);
            this.GazeBox.Controls.Add(this.GazeConfederate);
            this.GazeBox.Controls.Add(this.GazeGame);
            this.GazeBox.Controls.Add(this.GazeElsewhere);
            this.GazeBox.Controls.Add(this.GazeParticipant);
            this.GazeBox.Location = new System.Drawing.Point(598, 176);
            this.GazeBox.Name = "GazeBox";
            this.GazeBox.Size = new System.Drawing.Size(334, 266);
            this.GazeBox.TabIndex = 14;
            this.GazeBox.TabStop = false;
            this.GazeBox.Text = "GazeBox";
            // 
            // GlanceConfederate
            // 
            this.GlanceConfederate.BackColor = System.Drawing.Color.Tomato;
            this.GlanceConfederate.Location = new System.Drawing.Point(9, 153);
            this.GlanceConfederate.Name = "GlanceConfederate";
            this.GlanceConfederate.Size = new System.Drawing.Size(100, 100);
            this.GlanceConfederate.TabIndex = 7;
            this.GlanceConfederate.Text = "Glance Confederate";
            this.GlanceConfederate.UseVisualStyleBackColor = false;
            this.GlanceConfederate.Click += new System.EventHandler(this.GlanceConfederate_Click);
            // 
            // GlanceParticipant
            // 
            this.GlanceParticipant.BackColor = System.Drawing.Color.YellowGreen;
            this.GlanceParticipant.Location = new System.Drawing.Point(115, 153);
            this.GlanceParticipant.Name = "GlanceParticipant";
            this.GlanceParticipant.Size = new System.Drawing.Size(100, 100);
            this.GlanceParticipant.TabIndex = 6;
            this.GlanceParticipant.Text = "Glance Participant";
            this.GlanceParticipant.UseVisualStyleBackColor = false;
            this.GlanceParticipant.Click += new System.EventHandler(this.GlanceParticipant_Click);
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
            this.GazeConfederate.BackColor = System.Drawing.Color.Tomato;
            this.GazeConfederate.Location = new System.Drawing.Point(9, 47);
            this.GazeConfederate.Name = "GazeConfederate";
            this.GazeConfederate.Size = new System.Drawing.Size(100, 100);
            this.GazeConfederate.TabIndex = 3;
            this.GazeConfederate.Text = "Gaze Confederate";
            this.GazeConfederate.UseVisualStyleBackColor = false;
            this.GazeConfederate.Click += new System.EventHandler(this.GazeConfederate_Click);
            // 
            // GazeGame
            // 
            this.GazeGame.Location = new System.Drawing.Point(221, 47);
            this.GazeGame.Name = "GazeGame";
            this.GazeGame.Size = new System.Drawing.Size(100, 100);
            this.GazeGame.TabIndex = 2;
            this.GazeGame.Text = "Gaze Game";
            this.GazeGame.UseVisualStyleBackColor = true;
            this.GazeGame.Click += new System.EventHandler(this.GazeGame_Click);
            // 
            // GazeElsewhere
            // 
            this.GazeElsewhere.Location = new System.Drawing.Point(221, 153);
            this.GazeElsewhere.Name = "GazeElsewhere";
            this.GazeElsewhere.Size = new System.Drawing.Size(100, 100);
            this.GazeElsewhere.TabIndex = 1;
            this.GazeElsewhere.Text = "Gaze Elsewhere";
            this.GazeElsewhere.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.GazeElsewhere.UseVisualStyleBackColor = true;
            this.GazeElsewhere.Click += new System.EventHandler(this.GazeElsewhere_Click);
            // 
            // GazeParticipant
            // 
            this.GazeParticipant.BackColor = System.Drawing.Color.YellowGreen;
            this.GazeParticipant.Location = new System.Drawing.Point(115, 47);
            this.GazeParticipant.Name = "GazeParticipant";
            this.GazeParticipant.Size = new System.Drawing.Size(100, 100);
            this.GazeParticipant.TabIndex = 0;
            this.GazeParticipant.Text = "Gaze Participant";
            this.GazeParticipant.UseVisualStyleBackColor = false;
            this.GazeParticipant.Click += new System.EventHandler(this.GazeParticipant_Click);
            // 
            // TagBox
            // 
            this.TagBox.Controls.Add(this.UpdateTagValues);
            this.TagBox.Controls.Add(this.TagValuesLabel);
            this.TagBox.Controls.Add(this.TagValuesList);
            this.TagBox.Controls.Add(this.TagNamesLabel);
            this.TagBox.Controls.Add(this.TagNamesList);
            this.TagBox.Location = new System.Drawing.Point(598, 448);
            this.TagBox.Name = "TagBox";
            this.TagBox.Size = new System.Drawing.Size(412, 341);
            this.TagBox.TabIndex = 15;
            this.TagBox.TabStop = false;
            this.TagBox.Text = "Tag Box";
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
            this.TagValuesList.Text = "Fábio\r\no Fábio\r\nAndré\r\no André";
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
            this.TagNamesList.Text = "|participant|\r\n|participantGender|\r\n|confederate|\r\n|confederateGender|";
            // 
            // ThalamusStateLabel
            // 
            this.ThalamusStateLabel.AutoSize = true;
            this.ThalamusStateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThalamusStateLabel.Location = new System.Drawing.Point(769, 75);
            this.ThalamusStateLabel.Name = "ThalamusStateLabel";
            this.ThalamusStateLabel.Size = new System.Drawing.Size(141, 31);
            this.ThalamusStateLabel.TabIndex = 16;
            this.ThalamusStateLabel.Text = "Thalamus:";
            // 
            // ThalamusState
            // 
            this.ThalamusState.AutoSize = true;
            this.ThalamusState.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThalamusState.Location = new System.Drawing.Point(916, 75);
            this.ThalamusState.Name = "ThalamusState";
            this.ThalamusState.Size = new System.Drawing.Size(38, 31);
            this.ThalamusState.TabIndex = 17;
            this.ThalamusState.Text = "...";
            // 
            // GameControlsBox
            // 
            this.GameControlsBox.Controls.Add(this.EndGameButton);
            this.GameControlsBox.Controls.Add(this.StartGameButton);
            this.GameControlsBox.Controls.Add(this.GameStateLabel);
            this.GameControlsBox.Controls.Add(this.GameState);
            this.GameControlsBox.Location = new System.Drawing.Point(1031, 485);
            this.GameControlsBox.Name = "GameControlsBox";
            this.GameControlsBox.Size = new System.Drawing.Size(413, 131);
            this.GameControlsBox.TabIndex = 18;
            this.GameControlsBox.TabStop = false;
            this.GameControlsBox.Text = "Game Controls";
            // 
            // EndGameButton
            // 
            this.EndGameButton.Enabled = false;
            this.EndGameButton.Location = new System.Drawing.Point(112, 19);
            this.EndGameButton.Name = "EndGameButton";
            this.EndGameButton.Size = new System.Drawing.Size(100, 100);
            this.EndGameButton.TabIndex = 1;
            this.EndGameButton.Text = "End Game";
            this.EndGameButton.UseVisualStyleBackColor = true;
            this.EndGameButton.Click += new System.EventHandler(this.EndGameButton_Click);
            // 
            // StartGameButton
            // 
            this.StartGameButton.Enabled = false;
            this.StartGameButton.Location = new System.Drawing.Point(6, 19);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(100, 100);
            this.StartGameButton.TabIndex = 0;
            this.StartGameButton.Text = "Start Game";
            this.StartGameButton.UseVisualStyleBackColor = true;
            this.StartGameButton.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // GameStateLabel
            // 
            this.GameStateLabel.AutoSize = true;
            this.GameStateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameStateLabel.Location = new System.Drawing.Point(236, 19);
            this.GameStateLabel.Name = "GameStateLabel";
            this.GameStateLabel.Size = new System.Drawing.Size(159, 31);
            this.GameStateLabel.TabIndex = 0;
            this.GameStateLabel.Text = "GameState:";
            // 
            // GameState
            // 
            this.GameState.AutoSize = true;
            this.GameState.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameState.ForeColor = System.Drawing.Color.Firebrick;
            this.GameState.Location = new System.Drawing.Point(238, 50);
            this.GameState.Name = "GameState";
            this.GameState.Size = new System.Drawing.Size(102, 24);
            this.GameState.TabIndex = 1;
            this.GameState.Text = "Not Started";
            // 
            // FillWordButton
            // 
            this.FillWordButton.Enabled = false;
            this.FillWordButton.Location = new System.Drawing.Point(6, 183);
            this.FillWordButton.Name = "FillWordButton";
            this.FillWordButton.Size = new System.Drawing.Size(188, 37);
            this.FillWordButton.TabIndex = 3;
            this.FillWordButton.Text = "Fill Word";
            this.FillWordButton.UseVisualStyleBackColor = true;
            this.FillWordButton.Click += new System.EventHandler(this.FillWordButton_Click);
            // 
            // GameFeedbackBox
            // 
            this.GameFeedbackBox.Controls.Add(this.FillWordButton);
            this.GameFeedbackBox.Controls.Add(this.WordsNotCompletedLabel);
            this.GameFeedbackBox.Controls.Add(this.WordsNotCompleted);
            this.GameFeedbackBox.Controls.Add(this.FillWordBool);
            this.GameFeedbackBox.Controls.Add(this.FillWordNumber);
            this.GameFeedbackBox.Controls.Add(this.FillWordLabel);
            this.GameFeedbackBox.Location = new System.Drawing.Point(1031, 253);
            this.GameFeedbackBox.Name = "GameFeedbackBox";
            this.GameFeedbackBox.Size = new System.Drawing.Size(413, 226);
            this.GameFeedbackBox.TabIndex = 19;
            this.GameFeedbackBox.TabStop = false;
            this.GameFeedbackBox.Text = "GameFeedbackBox";
            // 
            // WordsNotCompletedLabel
            // 
            this.WordsNotCompletedLabel.AutoSize = true;
            this.WordsNotCompletedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WordsNotCompletedLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.WordsNotCompletedLabel.Location = new System.Drawing.Point(6, 114);
            this.WordsNotCompletedLabel.Name = "WordsNotCompletedLabel";
            this.WordsNotCompletedLabel.Size = new System.Drawing.Size(165, 20);
            this.WordsNotCompletedLabel.TabIndex = 6;
            this.WordsNotCompletedLabel.Text = "Words Not Completed";
            // 
            // WordsNotCompleted
            // 
            this.WordsNotCompleted.Enabled = false;
            this.WordsNotCompleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WordsNotCompleted.FormattingEnabled = true;
            this.WordsNotCompleted.Location = new System.Drawing.Point(6, 138);
            this.WordsNotCompleted.Name = "WordsNotCompleted";
            this.WordsNotCompleted.Size = new System.Drawing.Size(188, 39);
            this.WordsNotCompleted.TabIndex = 5;
            // 
            // FillWordBool
            // 
            this.FillWordBool.AutoSize = true;
            this.FillWordBool.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FillWordBool.Location = new System.Drawing.Point(277, 26);
            this.FillWordBool.Name = "FillWordBool";
            this.FillWordBool.Size = new System.Drawing.Size(38, 31);
            this.FillWordBool.TabIndex = 4;
            this.FillWordBool.Text = "...";
            // 
            // FillWordNumber
            // 
            this.FillWordNumber.AutoSize = true;
            this.FillWordNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FillWordNumber.Location = new System.Drawing.Point(225, 26);
            this.FillWordNumber.Name = "FillWordNumber";
            this.FillWordNumber.Size = new System.Drawing.Size(46, 31);
            this.FillWordNumber.TabIndex = 3;
            this.FillWordNumber.Text = "nr.";
            // 
            // FillWordLabel
            // 
            this.FillWordLabel.AutoSize = true;
            this.FillWordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FillWordLabel.Location = new System.Drawing.Point(99, 26);
            this.FillWordLabel.Name = "FillWordLabel";
            this.FillWordLabel.Size = new System.Drawing.Size(128, 31);
            this.FillWordLabel.TabIndex = 2;
            this.FillWordLabel.Text = "Fill Word:";
            // 
            // VorWOZForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1453, 802);
            this.Controls.Add(this.GameFeedbackBox);
            this.Controls.Add(this.GameControlsBox);
            this.Controls.Add(this.ThalamusState);
            this.Controls.Add(this.ThalamusStateLabel);
            this.Controls.Add(this.TagBox);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VorWOZForm_FormClosing);
            this.PanicBox.ResumeLayout(false);
            this.PanicBox.PerformLayout();
            this.LibrariesBox.ResumeLayout(false);
            this.LibrariesBox.PerformLayout();
            this.GazeBox.ResumeLayout(false);
            this.GazeBox.PerformLayout();
            this.TagBox.ResumeLayout(false);
            this.TagBox.PerformLayout();
            this.GameControlsBox.ResumeLayout(false);
            this.GameControlsBox.PerformLayout();
            this.GameFeedbackBox.ResumeLayout(false);
            this.GameFeedbackBox.PerformLayout();
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
        private System.Windows.Forms.GroupBox TagBox;
        private System.Windows.Forms.Label TagValuesLabel;
        private System.Windows.Forms.TextBox TagValuesList;
        private System.Windows.Forms.Label TagNamesLabel;
        private System.Windows.Forms.TextBox TagNamesList;
        private System.Windows.Forms.Button UpdateTagValues;
        private System.Windows.Forms.Button GlanceConfederate;
        private System.Windows.Forms.Button GlanceParticipant;
        private System.Windows.Forms.Label ThalamusStateLabel;
        private System.Windows.Forms.Label ThalamusState;
        private System.Windows.Forms.GroupBox GameControlsBox;
        private System.Windows.Forms.Button EndGameButton;
        private System.Windows.Forms.Button StartGameButton;
        private System.Windows.Forms.GroupBox GameFeedbackBox;
        private System.Windows.Forms.Label FillWordBool;
        private System.Windows.Forms.Label FillWordNumber;
        private System.Windows.Forms.Label FillWordLabel;
        private System.Windows.Forms.Label GameState;
        private System.Windows.Forms.Label GameStateLabel;
        private System.Windows.Forms.Button FillWordButton;
        private System.Windows.Forms.Label WordsNotCompletedLabel;
        private System.Windows.Forms.ComboBox WordsNotCompleted;
    }
}

