using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SandBox_WebAPI.Utilities
{

    public class WebApi
    {
        public static string Version = "2.2";
    }
    public class ResponseBase<T>
    {
        public string Version { get; set; }
        public string Classname { get { return typeof(T).Name; } }
        public Exception Exception { get; set; }

        public string StatusCode { get; set; }

        public string RequestUrl { get; set; }
    }
    public class WebApiResponse<T> : ResponseBase<T>
    {
        public T Data { get; set; }

        public Dictionary<string, string> Includes { get; set; }
    }
    public class WebApiResponseList<T> : ResponseBase<T>
    {
        public dynamic List { get; set; }
        public int Count
        {
            get
            {
                if (List != null)
                {
                    if (List is ICollection)
                        return List.Count;
                    else
                        return 1;
                }
                else
                    return 0;

            }
        }
    }

}