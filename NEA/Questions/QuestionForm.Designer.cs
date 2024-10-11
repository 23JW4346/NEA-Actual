namespace NEA.Questions
{
    partial class QuestionForm
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
            this.questionText = new System.Windows.Forms.TextBox();
            this.CheckButton = new System.Windows.Forms.Button();
            this.HintButton = new System.Windows.Forms.Button();
            this.AnswerLabel = new System.Windows.Forms.Label();
            this.answerText = new System.Windows.Forms.TextBox();
            this.correctAnswerText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // questionText
            // 
            this.questionText.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.questionText.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questionText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.questionText.Location = new System.Drawing.Point(31, 39);
            this.questionText.Name = "questionText";
            this.questionText.Size = new System.Drawing.Size(1838, 40);
            this.questionText.TabIndex = 0;
            // 
            // CheckButton
            // 
            this.CheckButton.Location = new System.Drawing.Point(659, 164);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(75, 23);
            this.CheckButton.TabIndex = 1;
            this.CheckButton.Text = "Submit";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // HintButton
            // 
            this.HintButton.Location = new System.Drawing.Point(527, 164);
            this.HintButton.Name = "HintButton";
            this.HintButton.Size = new System.Drawing.Size(75, 23);
            this.HintButton.TabIndex = 2;
            this.HintButton.Text = "Hint";
            this.HintButton.UseVisualStyleBackColor = true;
            // 
            // AnswerLabel
            // 
            this.AnswerLabel.AutoSize = true;
            this.AnswerLabel.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnswerLabel.Location = new System.Drawing.Point(1556, 153);
            this.AnswerLabel.Name = "AnswerLabel";
            this.AnswerLabel.Size = new System.Drawing.Size(218, 33);
            this.AnswerLabel.TabIndex = 3;
            this.AnswerLabel.Text = "Write Answer here";
            // 
            // answerText
            // 
            this.answerText.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.answerText.Location = new System.Drawing.Point(1562, 189);
            this.answerText.Name = "answerText";
            this.answerText.Size = new System.Drawing.Size(212, 40);
            this.answerText.TabIndex = 4;
            // 
            // correctAnswerText
            // 
            this.correctAnswerText.Location = new System.Drawing.Point(527, 220);
            this.correctAnswerText.Name = "correctAnswerText";
            this.correctAnswerText.Size = new System.Drawing.Size(207, 20);
            this.correctAnswerText.TabIndex = 5;
            // 
            // QuestionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.correctAnswerText);
            this.Controls.Add(this.answerText);
            this.Controls.Add(this.AnswerLabel);
            this.Controls.Add(this.HintButton);
            this.Controls.Add(this.CheckButton);
            this.Controls.Add(this.questionText);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "QuestionForm";
            this.Text = "QuestionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox questionText;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.Button HintButton;
        private System.Windows.Forms.Label AnswerLabel;
        private System.Windows.Forms.TextBox answerText;
        private System.Windows.Forms.TextBox correctAnswerText;
    }
}