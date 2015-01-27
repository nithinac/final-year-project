using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SandBox_WebAPI.Models
{

    public class SafetyQuestionViewModel
    {
        public String Question { get; set; }
        public List<String> Options { get; set; }
        public int AnswerId { get; set; }
    }

    public class ContentTypeViewModel
    {
        public String Extension { get; set; }
        public String Type { get; set; }
        //public int SafetyInstructionId { get; set; }
    }
    public class SafetyInstructionViewModel
    {
        public String Description { get; set; }

        public String Type { get; set; }
        public String Attachment { get; set; }
    }

    public class DoneOnDueOnViewModel
    {
        public int TaskId { get; set; }
    }
}