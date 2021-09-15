using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace QuizInterface
{
    public partial class Form1 : Form
    {
        /* Global variables */
        Random rnd = new Random();
        RadioButton[] btn;
        string activeBtn;

        // total question
        int totalQuestion;

        //first question
        int idx = 0;

        int ansChoice = 0; // you get one chance
        int score = 0; // initialize score/ globalized to prevent from setting to zero
        int totalScore = 0;
        bool selectionFeedBackValue = false;
 
        int questionNumber = 1; // initialize question number
        string result = "ans"; // initialize correct answer

        public Form1()
        {
            InitializeComponent();
   
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            infoLbl.Visible = false;
            textQuestion.Visible = false;
            quizNum.Visible = false;
            scoreLbl.Visible = false;
            submitBtn.Visible = false;
            // entry Note
            welcomeNote.Visible = true;
            welcomeNote.Text = "Welcome to Microsoft System and Software Academy compitency test. \r\nThere are 47 questions.\r\nEach question is worth 1 point. \r\nYour score will be added and displayed at the end. \r\n\nGood Luck! \r\nHit next to start your test.";
            welcomeNote.Location = new Point(120, 100);
            welcomeNote.Size = new Size(300, 40);
        }
        
        private void nextBtn_Click(object sender, EventArgs e)
        {
            // Clear and Reset
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load_1(e, e);

            // hide welcomeNote
            welcomeNote.Visible = false;

            // display Score
            scoreLbl.Visible = true;
            scoreLbl.Text = "Score: " + totalScore.ToString();
            scoreLbl.Location = new Point(600, 10);

            textQuestion.Visible = true;
            quizNum.Visible = true;

            // Read JSON file
            string compitencyQuiz = File.ReadAllText(@"Compitency.json");
            QuestionCollection quesCollection = JsonConvert.DeserializeObject<QuestionCollection>(compitencyQuiz);
            totalQuestion = quesCollection.Questions.Count;

            // check if last question
            if (idx < totalQuestion)
            {
                // display question number
                quizNum.Text = "Question " + questionNumber.ToString();
                quizNum.Location = new Point(30, 10);
                quizNum.Padding = new Padding(0, 0, 0, 0);
                questionNumber++;

                // display question
                textQuestion.Text = quesCollection.Questions[idx].Quiz.ToString();
                textQuestion.Location = new Point(30, 45);
                textQuestion.MaximumSize = new Size(600, 100);
                textQuestion.Padding = new Padding(0, 0, 0, 10);

                // display answer
                var answer = quesCollection.Questions[idx].Answer;

                // Create Dynamic Answers/Button
                // place dynamic button
                btn = new RadioButton[answer.Count];
                int i = 0; // initialize button index
                int top = textQuestion.Location.Y + textQuestion.Height + 10;
                foreach (KeyValuePair<string, bool> item in answer)
                {
                    btn[i] = new RadioButton();
                    btn[i].Name = "ans" + i.ToString();
                    btn[i].Font = new Font("ab", 10);
                    btn[i].Text = item.Key + Environment.NewLine;
                    btn[i].AutoSize = true;
                    btn[i].MaximumSize = new Size(600, 400);
                    btn[i].Location = new Point(60, top);
                    btn[i].FlatStyle = FlatStyle.Flat;
                    this.Controls.Add(btn[i]);
                    btn[i].Click += new EventHandler(this.button_click);
                    top += btn[i].Height + 10;
                 
                    // get correct answer button name
                    if (item.Value)
                    {
                        result = btn[i].Name;
                    }
                    // increase button index
                    i += 1;
                }
                // place label
                infoLbl.Location = new Point(60, top + 10);

                // Dynamic placement of Button NEXT
                nextBtn.Location = new Point(this.Width / 2, this.Height - 56);

                // Reset answer Choice
                ansChoice = 0;

                // next question
                idx += 1;
            }
           else
            {
                // if last question
                afterLastQuestion(totalScore, totalQuestion, e);
            }
        }

        // Dynamic button even handler
        void button_click(object sender, EventArgs e)
        {
            // get avtive button
            RadioButton btn = sender as RadioButton;
            activeBtn = btn.Name;

            // make visible only chance not used
            if (ansChoice < 1)
            {
                submitBtn.Visible = true;
                submitBtn.Location = new Point(nextBtn.Location.X - 100, nextBtn.Location.Y);
            }
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            // display feedback Correct/Wrong
            selectionFeedBackValue = (activeBtn == result);
            information(selectionFeedBackValue);

            // get/update score
            totalScore = scoreCalc(selectionFeedBackValue);
            scoreLbl.Text = "Score: " + totalScore.ToString();

            // hide submitBtn
            submitBtn.Visible = false;

            // Answer Choice has been used
            ansChoice += 1;
        }

        // score calculator
        public int scoreCalc(bool y)
        {            
            if (y)
            {
                score += 1;
            }
            return score;
        }

        // Correct/Wrong info
        void information(bool condition)
        {
            infoLbl.Visible = true;
            infoLbl.ForeColor = Color.Red;

            if (condition)
            {
                infoLbl.ForeColor = Color.Green;
                infoLbl.Text = "Correct!";
            }
            else
            {
                infoLbl.Text = "Incorrect!";
            }  
        }

        public void afterLastQuestion(int x, int count, EventArgs e)
        {
            // clear everything
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load_1(e, e);

            // turn off everything
            infoLbl.Visible = false;
            textQuestion.Visible = false;
            quizNum.Visible = false;
            scoreLbl.Visible = false;
            nextBtn.Visible = false;

            // Conclustion Note
            welcomeNote.Visible = true;
            welcomeNote.Location = new Point(240, 100);

            // int to float conversion
            float flt1 = x;
            float flt2 = count;
            float calc = (flt1 / flt2) * 100;
            int total = Convert.ToInt16(calc);

            if (total >= 60)
            {

                welcomeNote.Text = "Congratulation! You passed.\r\nYou scored: " + total.ToString() + "%\r\nYou got " + totalScore + " correct, out of " + totalQuestion + ".";
            }
            else
            {
                welcomeNote.Text = "Sorry! You did not pass.\r\nYou scored: " + total.ToString() + "%\r\nYou got " + totalScore + " correct, out of " + totalQuestion + ".";
            }
        }
    }
}
