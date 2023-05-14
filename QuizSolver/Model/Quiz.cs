using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizSolver.Model
{
    internal class Quiz
    {
        private string _title;
        public string Title { get { return _title; } set { _title = value; } }


        private List<Question> _questions;
        public List<Question> Questions { get { return _questions; } set { _questions = value; } }


        private int _timeLimit;
        public int TimeLimit { get { return _timeLimit; } set { _timeLimit = value; } }

        public Quiz()
        {
            _title = "Quiz testowy";
            _questions = new List<Question>
            {
                new Question()
            };
            _timeLimit = 10;
        }
        public Quiz(string name, List<Question> questions, int timeLimit) 
        {
            _title = name;
            _questions = questions;
            _timeLimit = timeLimit;
        }


        public override string ToString()
        {
            return $"{_title}";
        }
    }
}
