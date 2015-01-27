using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SandBox_WebAPI.Models
{
    //public class ResponseBase<T>
    //{
    //    public string Version { get; set; }
    //    public string Classname { get { return typeof(T).Name; } }
    //    public Exception Exception { get; set; }

    //    public string StatusCode { get; set; }

    //    public string RequestUrl { get; }
    //}
    //public class WebApiResponse<T> :ResponseBase<T>
    //{
    //    public T Data { get; set; }

    //    public Dictionary<string,string> Includes { get; set; }
    //}
    //public class WebApiResponseList<T> : ResponseBase<T>
    //{
    //    public List<T> List { get; set; }
    //    public int Count
    //    {
    //        get
    //        {
    //            return List != null ? List.Count : 0;
    //        }
    //    }

    //}
    public class Option
    {
        public Option()
        {

        }
        public Option(String text)
        {
            Text = text;
        }
        public int ID { get; set; }
        public String Text { get; set; }
    }

    [KnownType(typeof(SafetyQuestion))]
    public class SafetyQuestion
    {      
        public int ID { get; set; }
        public String Question { get; set; }
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