using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NEA.Questions.Loci;
using NEA.Questions.ModArg;
using NEA.Questions.MultiDivide;
using NEA.Questions.Polynomial_Roots;
using NEA.Questions;

namespace NEA.Questions
{
    public partial class QuestionForm : Form
    {
        private IQuestion question;

        public QuestionForm(IQuestion q)
        {
            InitializeComponent();
            question = q;
            questionText.Text = question.PrintQuestion();
            answerText.Text = "";            
        }



        private void CheckButton_Click(object sender, EventArgs e)
        {
            if(question.CheckAnswer(answerText.Text))
            {
                correctAnswerText.Text = question.PrintAnswer(true);
            }
            else
            {
                correctAnswerText.Text = question.PrintAnswer(false);
            }

        }
    }
}
