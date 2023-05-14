using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizSolver.Model
{
    internal class Question
    {
        private readonly int _id;
        public int Id { get; set; }

        private string _title;
        public string Title { get; set; }

        private BindingList<Answer> _answers;
        public BindingList<Answer> Answers { get; set; }

        public struct Answer
        {
            public string content;
            public bool is_correct;
            public Answer( string content, bool is_correct)
            {
                this.content = content;
                this.is_correct = is_correct;
            }
            public override string ToString()
            {
                return $"{content}";
            }
        }
        public Question() 
        {
            _id = 0;
            _title = "New question";
            _answers = new BindingList<Answer>
            {
                new Answer("answer 1", true),
                new Answer("answer 2", false),
                new Answer("answer 3", false),
                new Answer("answer 4", false),
            };
        }
        public Question(int id, string title, BindingList<Answer> answers)
        {
            _id = id;
            _title = title;
            _answers = answers;
        }

        public override string ToString()
        {
            return $"{_title}";
        }

    }
}