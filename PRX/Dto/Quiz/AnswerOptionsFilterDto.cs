using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PRX.Dto.Quiz
{
    public class AnswerOptionsFilterDto
    {
        public string Type { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }

    }
}
