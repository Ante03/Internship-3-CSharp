using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_3_CSharp
{
    internal class Task
    {
        public string NameOfTask { get; set; }
        public string DescriptionOfTask { get; set; }
        public DateTime DateEndOfTask { get; set; }
        public DateTime StartDateOfTask { get; set; }
        public TaskStatus StatusOfTask { get; set; }
        public TaskPriority PriorityOfTask { get; set; }

        public enum TaskStatus
        {
            Active,
            Done,
            Postponed
        }

        public enum TaskPriority
        {
            Low,
            Middle,
            High
        }
    }
}
