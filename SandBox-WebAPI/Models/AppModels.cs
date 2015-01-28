using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SandBox_WebAPI.Models
{

    public class Option
    {
        public Option()
        {

        }
        public Option(string text)
        {
            Text = text;
        }
        public int ID { get; set; }
        public string Text { get; set; }
    }

    [KnownType(typeof(SafetyQuestion))]
    public class SafetyQuestion
    {      
        public int ID { get; set; }
        public string Question { get; set; }
        public ICollection<Option> Options { get; set; }
        public int Answer { get; set; }
    }
    public class SafetyInstruction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Attachment Attachment { get; set; }
    }

    public class Attachment
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public string FileName { get; set; }
        public ContentType Content { get; set; }
    }

    public class ContentType
    {
        public int Id { get; set; }
        public string Extension { get; set; }
        public string Type { get; set; }
        //public SafetyInstruction SafetyInstruction { get; set; }
    }

    public class MaintenanceTask
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Period { get; set; }

    }
    public class DoneOnDueOn
    {
        public int ID { get; set; }
        public MaintenanceTask Checklist { get; set; }
        public DateTime DoneOn { get; set; }
        public DateTime DueOn { get; set; }
    }
}