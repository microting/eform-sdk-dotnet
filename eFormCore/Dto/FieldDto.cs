namespace Microting.eForm.Dto
{
    public class FieldDto
    {
        #region con
        public FieldDto()
        {

        }

        public FieldDto(int id, string label, string description, int fieldTypeId, string fieldType, int checkListId)
        {
            Id = id;
            Label = label;
            Description = description;
            FieldTypeId = fieldTypeId;
            FieldType = fieldType;
            CheckListId = checkListId;
        }
        #endregion

        #region var
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Label
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Descrition
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Repeated
        /// </summary>
        public int FieldTypeId { get; }

        /// <summary>
        /// Descrition
        /// </summary>
        public string FieldType { get; }

        /// <summary>
        /// checkListId
        /// </summary>
        public int CheckListId { get; }

        public string ParentName { get; set; }

        #endregion
    }
}