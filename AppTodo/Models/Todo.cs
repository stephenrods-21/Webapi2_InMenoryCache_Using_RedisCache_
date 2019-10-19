using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTodo.Models
{
    public class Todo
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}