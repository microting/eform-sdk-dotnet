/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

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

namespace Microting.eForm.Dto
{
    public class SiteWorkerDto
    {
        #region con

        public SiteWorkerDto()
        {
        }

        public SiteWorkerDto(int microtingUId, int siteUId, int workerUId)
        {
            MicrotingUId = microtingUId;
            SiteUId = siteUId;
            WorkerUId = workerUId;
        }

        #endregion

        #region var

        /// <summary>
        ///...
        /// </summary>
        public int MicrotingUId { get; }

        /// <summary>
        ///...
        /// </summary>
        public int SiteUId { get; }

        /// <summary>
        ///...
        /// </summary>
        public int WorkerUId { get; }

        #endregion

        public override string ToString()
        {
            return "MicrotingUId:" + MicrotingUId + " / SiteUId:" + SiteUId + " / WorkerUId:" + WorkerUId + ".";
        }
    }
}