/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


using System.ComponentModel.DataAnnotations.Schema;

namespace eFormSqlController
{
    public partial class questions : base_entity
    {
        [ForeignKey("question_set")]
        public int questionSetId { get; set; }
        
        public string questionType { get; set; }
        
        public int minimum { get; set; }
        
        public int maximum { get; set; }
        
        public string type { get; set; }
        
        public int refId { get; set; }
        
        public int questionIndex { get; set; }
        
        public bool image { get; set; }
        
        public int continuousQuestionId { get; set; }
        
        public string imagePostion { get; set; }
        
        public bool prioritised { get; set; }
        
        public bool backButtonEnabled { get; set; }
        
        public string fontSize { get; set; }
        
        public int minDuration { get; set; }
        
        public int maxDuration { get; set; }
        
        public bool validDisplay { get; set; }
    }
}