using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormRequest
{
    public class Answer : DataItem
    {
        public string fieldType { get; set; }
        public DateTime DateOfDoing { get; set; }
        public string Value { get; set; }
        public string ValueReadable { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Altitude { get; set; }
        public string Heading { get; set; }
        public string Accuracy { get; set; }
        public DateTime? Date { get; set; }
        public string UploadedData { get; set; }
    }
}
