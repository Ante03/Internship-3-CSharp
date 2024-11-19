using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_3_CSharp
{
    internal class Project
    {
        private Guid id;
        public string NameOfProject { get; set; }
        public string DescriptionOfProject { get; set; }
        public DateTime DateStartOfProject { get; set; }
        public DateTime DateEndOfProject { get; set;}
        public ProjectStatus StatusOfProject { get; set; }

        public enum ProjectStatus
        {
            Active,
            OnWait,
            Finished
        }
    }
}
