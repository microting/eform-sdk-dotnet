namespace Microting.eForm.Dto
{
    public class NoteDto
    {
        #region con
        public NoteDto()
        {

        }

        public NoteDto(string id, int? microtingUId, string activity)
        {
            Id = id;
            MicrotingUId = microtingUId;
            Activity = activity;
        }
        #endregion

        #region var
        public string Id { get; }

        public int? MicrotingUId { get; }

        public string Activity { get; }
        #endregion

        public override string ToString()
        {
            return "Id:" + Id + " / MicrotingUId:" + MicrotingUId + " / Activity:" + Activity + ".";
        }
    }
}