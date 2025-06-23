namespace Bridge.Models.ClientServer
{
    public class AgeConfirmationQuizResponse
    {
        public bool IsAgeConfirmed { get; set; }

        public bool IsRunOutOfQuestions { get; set; }

        public QuestionInfo[] Questions { get; set; } = { };
    }

    public class QuestionInfo
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public AnswerInfo[] Answers { get; set; }
    }

    public class AnswerInfo
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }

    public class AgeConfirmationResult
    {
        public bool IsAgeConfirmed { get; set; }
    }
    
    public class AgeConfirmationAnswer
    {
        public long QuestionId { get; set; }

        public long AnswerId { get; set; }
    }
}