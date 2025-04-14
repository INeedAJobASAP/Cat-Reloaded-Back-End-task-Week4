using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catproject
{
    public abstract class question
    {
        public string header
        { 
            get;
            set;
        }
        public string body 
        { 
            get;
            set;
        }
        public int mark 
        {
            get; 
            set; 
        }
        public question(string Header, string Body, int Mark)
        {
            header = Header;
            body = Body;
            mark = Mark;
        }
        public abstract void Show();
    }
    public class TorF : question
    {
        public Ans[] AnsList
        { 
            get; 
            set;
        }
        public Ans RightAns 
        { 
            get; 
            set; 
        }
        public TorF(string header, string body, int mark, Ans rightAns)
            : base(header, body, mark)
        {
            AnsList = new Ans[2]
            {
                new Ans(1, "true"),
                new Ans(2, "false")
            };

            RightAns = rightAns;
        }
        public override void Show()
        {
            Console.WriteLine($"[true/false] {header} - {body} ({mark} marks)");
            foreach (var ans in AnsList)
            {
                Console.WriteLine(ans);
            }
        }
    }
    public class mcq : question
    {
        public Ans[] AnsList 
        { 
            get; 
            set; 
        }
        public Ans RightAns 
        {
            get;
            set; 
        }
        public mcq(string header, string body, int mark, Ans[] answers, Ans rightAns)
            : base(header, body, mark)
        {
            AnsList = answers;
            RightAns = rightAns;
        }
        public override void Show()
        {
            Console.WriteLine($"[mcq] {header} - {body} ({mark} marks)");
            foreach (var ans in AnsList)
            {
                Console.WriteLine(ans);
            }
        }
    }
    public class Ans
    {
        public int AnsId
        { 
            get;
            set;
        }
        public string AnsText
        { 
            get;
            set;
        }
        public Ans(int id, string text)
        {
            AnsId = id;
            AnsText = text;
        }
        public override string ToString()
        {
            return $"{AnsId}. {AnsText}";
        }
        public override bool Equals(object obj)
        {
            if (obj is Ans other)
            {
                return AnsId == other.AnsId && AnsText == other.AnsText;
            }
            else
            {
                return false;
            }
        }
    }
    public abstract class exam
    {
        public int time
        {
            get;
            set;
        }
        public int numOfquestions 
        { 
            get; 
            set;
        }
        public question[] questions 
        { 
            get;
            set; 
        }
        public exam(int Time, int NUMOfquestions)
        {
            time = Time;
            numOfquestions = NUMOfquestions;
            questions = new question[NUMOfquestions];
        }
        public abstract void Show();
    }
    public class finalexam : exam
    {
        public finalexam(int time, int numOfQuestions) : base(time, numOfQuestions)
        {
        }
        public override void Show()
        {
            Console.WriteLine($"final exam ({time} minutes, {numOfquestions} questions):");
            int totalMarks = 0;
            foreach (var question in questions)
            {
                question.Show();
                string userAns = string.Empty;
                if (question is TorF trueFalseQuestion)
                {
                    Console.Write($"question: {question.header} (true/false): ");
                    userAns = Console.ReadLine();
                    if (userAns.Equals(trueFalseQuestion.RightAns.AnsText, StringComparison.OrdinalIgnoreCase))
                    {
                        totalMarks += question.mark;
                    }
                }
                else if (question is mcq mcqQuestion)
                {
                    Console.WriteLine($"question: {question.header} (choose an answer):");
                    for (int i = 0; i < mcqQuestion.AnsList.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {mcqQuestion.AnsList[i].AnsText}");
                    }
                    Console.Write("your choice (enter number): ");
                    int choice = int.Parse(Console.ReadLine()) - 1;
                    userAns = mcqQuestion.AnsList[choice].AnsText;
                    if (userAns.Equals(mcqQuestion.RightAns.AnsText, StringComparison.OrdinalIgnoreCase))
                    {
                        totalMarks += question.mark;
                    }
                }
            }
            Console.WriteLine($"\ntotal grade: {totalMarks}/{questions.Length * 5}");
        }
    }
    public class practicalexam : exam
    {
        public practicalexam(int time, int numOfquestions) : base(time, numOfquestions)
        {
        }
        public override void Show()
        {
            Console.WriteLine($"practical exam ({time} minutes, {numOfquestions} questions):");
            foreach (var question in questions)
            {
                question.Show();
            }
            Console.WriteLine("\nplease provide your answers:");
            foreach (var question in questions)
            {
                string userAns = string.Empty;
                if (question is TorF)
                {
                    Console.Write($"question: {question.header} (true/false): ");
                    userAns = Console.ReadLine();
                }
                else if (question is mcq)
                {
                    Console.WriteLine($"question: {question.header} (choose an answer):");
                    for (int i = 0; i < ((mcq)question).AnsList.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {((mcq)question).AnsList[i].AnsText}");
                    }

                    Console.Write("your choice (enter number): ");
                    int choice = int.Parse(Console.ReadLine()) - 1;
                    userAns = ((mcq)question).AnsList[choice].AnsText;
                }
                Console.WriteLine($"your answer: {userAns}");
            }
            Console.WriteLine("\nright answers:");
            foreach (var question in questions)
            {
                Console.WriteLine($"question: {question.header}");
                if (question is TorF trueFalseQuestion)
                {
                    Console.WriteLine($"correct answer: {trueFalseQuestion.RightAns.AnsText}");
                }
                else if (question is mcq mcqQuestion)
                {
                    Console.WriteLine($"correct answer: {mcqQuestion.RightAns.AnsText}");
                }
            }
        }
    }

    public class subject
    {
        public int subjectid 
        {
            get;
            set;
        }
        public string subjectname 
        {
            get;
            set;
        }
        public exam exam
        { 
            get; 
            set; 
        }
        public subject(int subjectId, string subjectName)
        {
            subjectid = subjectId;
            subjectname = subjectName;
        }
        public void CreateExam(exam exam)
        {
            this.exam = exam;
        }
        public void ShowExam()
        {
            Console.WriteLine($"subject: {subjectname}");
            exam.Show();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var trueAns = new Ans(1, "true");
            var falseAns = new Ans(2, "false");
            var mcqAns = new Ans[]
            {
                new Ans(1, "cairo"),
                new Ans(2, "alexandria"),
                new Ans(3, "giza")
            };
            var TorFQuestion1 = new TorF("does 2+2 equal 4?", "yes or no", 5, trueAns);
            var TorFQuestion2 = new TorF("egypt has most pyramids in the world", "yes or no", 5, falseAns);
            var mcqQuestion = new mcq("capital of egypt?", "choose correct answer", 5, mcqAns, mcqAns[0]);
            var questions = new question[] { TorFQuestion1, mcqQuestion, TorFQuestion2 };
            var finalExam = new finalexam(60, questions.Length);
            finalExam.questions = questions;
            var practicalExam = new practicalexam(60, questions.Length);
            practicalExam.questions = questions;
            var subject = new subject(101, "geography");
            subject.CreateExam(finalExam);
            subject.ShowExam();
        }
    }
}
