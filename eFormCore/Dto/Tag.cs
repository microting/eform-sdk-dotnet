namespace Microting.eForm.Dto
{
    public class Tag
    {
        #region con
        public Tag(int id, string name, int? taggingCount)
        {
            Id = id;
            Name = name;
            if (taggingCount == null)
                TaggingCount = 0;
            else
                TaggingCount = taggingCount;
        }
        #endregion

        #region var
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// TaggingCount
        /// </summary>
        public int? TaggingCount { get; }
        #endregion
    }
}