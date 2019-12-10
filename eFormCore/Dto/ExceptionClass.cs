using System;

namespace Microting.eForm.Dto
{
    public class ExceptionClass
    {
        private ExceptionClass()
        {
            Description = "";
            Time = DateTime.Now;
            Occurrence = 1;
        }

        public ExceptionClass(string description, DateTime time)
        {
            Description = description;
            Time = time;
            Occurrence = 1;
        }

        public string Description { get; set; }
        public DateTime Time { get; set; }
        public int Occurrence { get; set; }
    }
}