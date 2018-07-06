using System;

namespace eForm.Messages
{
    public class TranscribeAudioFile
    {
        public int uploadedDataId { get; protected set; }

        public TranscribeAudioFile(int uploadedDataId)
        {
            this.uploadedDataId = uploadedDataId;
        }
    }
}
