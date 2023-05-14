using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using QuizSolver.Model;
using static QuizSolver.Model.Question;


namespace QuizSolver.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _canStartTheQuiz = false;
        public bool CanStartTheQuiz
        {
            get { return _canStartTheQuiz; }
            set { _canStartTheQuiz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanStartTheQuiz)));
            }
        }

        private bool _canLoadTheQuiz = true;
        public bool CanLoadTheQuiz
        {
            get { return _canLoadTheQuiz; }
            set
            {
                _canLoadTheQuiz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanLoadTheQuiz)));
            }
        }
        private bool _canSelectTheQuiz = false;
        public bool CanSelectTheQuiz
        {
            get { return _canSelectTheQuiz; }
            set
            {
                _canSelectTheQuiz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanSelectTheQuiz)));
            }
        }
        private bool _canFinishTheQuiz = false;
        public bool CanFinishTheQuiz
        {
            get { return _canFinishTheQuiz; }
            set
            {
                _canFinishTheQuiz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanFinishTheQuiz)));
            }
        }

        private BindingList<Quiz> _loadedQuizzes = new BindingList<Quiz>();
        public BindingList<Quiz> LoadedQuizzes
        {
            get => _loadedQuizzes;
            set
            {
                _loadedQuizzes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadedQuizzes)));
            }
        }

        public void enableControls()
        {
            CanLoadTheQuiz = true;
            CanStartTheQuiz = true;
            CanSelectTheQuiz = true;
        }

        private Quiz _selectedQuiz;
        public Quiz SelectedQuiz
        {
            get => _selectedQuiz;
            set
            {
                _selectedQuiz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedQuiz)));
                TimeLeft = SelectedQuiz.TimeLimit;
                SelectedQuestionIndex = 1;
                CurrentQuestion = SelectedQuiz.Questions[0];
            }
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (TimeLeft > 0)
                TimeLeft--;
            else
            {
                _timer.Stop();
                MessageBox.Show("Time's up!");
                enableControls();
                CanFinishTheQuiz = false;
                CanGoToNextQuestion = false;
            }
        }

        private DispatcherTimer _timer = new DispatcherTimer();
        public MainViewModel()
        {
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += TimerTick;
        }

        private int _timeLeft;
        public int TimeLeft
        {
            get { return _timeLeft; } set
            {
                if (_timeLeft == value) return;
                _timeLeft = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeLeft)));
            }
        }


        //polecenie 
        private ICommand buttonLoad_Click;

        public ICommand ButtonLoad_Click
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return buttonLoad_Click ?? (buttonLoad_Click = new BaseClass.RelayCommand(
                //co wykonuje polecenie
                    (canExecute) =>
                    {
                        BindingList<Answer> answers1 = new BindingList<Answer>
                        {
                            new Answer("answer 1", true),
                            new Answer("answer 2", false),
                            new Answer("answer 3", false),
                            new Answer("answer 4", false),
                        };
                        BindingList<Answer> answers2 = new BindingList<Answer>
                        {
                            new Answer("answer 5", false),
                            new Answer("answer 6", false),
                            new Answer("answer 7", true),
                            new Answer("answer 8", false),
                        };
                        List<Question> questions1 = new List<Question>
                        {
                            new Question(0, "First Question is...", answers1),
                            new Question(1, "Second Question is...", answers2),
                            new Question(2, "Third Question is...", answers1)
                        };
                        List<Question> questions2 = new List<Question>
                        {
                            new Question(0, "First Question is...", answers1),
                        };
                        LoadedQuizzes.Add(new Quiz("First Quiz", questions1, 3));
                        LoadedQuizzes.Add(new Quiz("Second Quiz", questions2, 20));

                        CanStartTheQuiz = true;
                        CanSelectTheQuiz = true;
                    }
                    ,
                    canExecute => CanLoadTheQuiz)
                    );
            }
        }
        private string _currentFirstAnswer = "Test";
        public string CurrentFirstAnswer
        {
            get { return _currentFirstAnswer; } 
            set 
            {
                _currentFirstAnswer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFirstAnswer)));
            }
        }

        private Question _currentQuestion = new Question();
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                if (CurrentQuestion != null && CurrentQuestion.Answers != null && CurrentQuestion.Answers.Count > 0)
                {
                    CurrentFirstAnswer = CurrentQuestion.Answers[0].ToString();
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentQuestion)));
            }
        }
        private int _selectedQuestionIndex = 1;
        public int SelectedQuestionIndex
        {
            get => _selectedQuestionIndex;
            set
            {
                _selectedQuestionIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedQuestionIndex)));
            }
        }

        private BindingList<Answer> _currentAnswers = new BindingList<Answer>();
        public BindingList<Answer> CurrentAnswers
        {
            get => _currentAnswers;
            set
            {
                _currentAnswers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentAnswers)));
            }
        }

        private bool _canGoToNextQuestion = false;
        public bool CanGoToNextQuestion
        {
            get => _canGoToNextQuestion;
            set
            {
                _canGoToNextQuestion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanGoToNextQuestion)));
            }
        }

        private ICommand _nextQuestionBtn_Click;
        public ICommand NextQuestionBtn_Click
        {
            get
            {
                return _nextQuestionBtn_Click ?? (_nextQuestionBtn_Click = new BaseClass.RelayCommand(
                    (canExecute) =>
                    {
                        if (SelectedQuestionIndex < SelectedQuiz.Questions.Count)
                        {
                            SelectedQuestionIndex++;
                            CurrentQuestion = SelectedQuiz.Questions[SelectedQuestionIndex - 1];
                            CurrentAnswers = CurrentQuestion.Answers;
                        }
                        else CanGoToNextQuestion = false;
                    }
                    ,
                    canExecute => CanGoToNextQuestion)
                    );
            }
        }
        private ICommand _startBtn_Click;
        public ICommand StartBtn_Click
        {
            get
            {
                return _startBtn_Click ?? (_startBtn_Click = new BaseClass.RelayCommand(
                    (canExecute) =>
                    {
                        TimeLeft = SelectedQuiz.TimeLimit;
                        _timer.Start();
                        CanLoadTheQuiz = false;
                        CanStartTheQuiz = false;
                        CanSelectTheQuiz = false;
                        CanFinishTheQuiz = true;
                        CanGoToNextQuestion = true;
                    }
                    ,
                    canExecute => CanStartTheQuiz)
                    );
            }
        }

        private ICommand _finishBtn_Click;
        public ICommand FinishBtn_Click
        {
            get
            {
                return _finishBtn_Click ?? (_finishBtn_Click = new BaseClass.RelayCommand(
                    (canExecute) =>
                    {
                        _timer.Stop();
                        MessageBox.Show("You stopped the quiz.");
                        enableControls();
                        CanFinishTheQuiz = false;
                        CanGoToNextQuestion = false;
                    }
                    ,
                    canExecute => CanFinishTheQuiz)
                    );
            }
        }
    }
}

