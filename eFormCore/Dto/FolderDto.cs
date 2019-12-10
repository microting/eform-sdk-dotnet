using System;

namespace Microting.eForm.Dto
{
    public class FolderDto
    {
        #region con

        public FolderDto(int? id, string name, string description, int? parentId, DateTime? createdAt, DateTime? updatedAt,int? microtingUId)
        {
            Id = id;
            Name = name;
            Description = description;
            ParentId = parentId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            MicrotingUId = microtingUId;
        }
        #endregion
        
        #region var
        public int? Id { get; }
        
        public string Name { get; }
        
        public string Description { get; }
        
        public int? ParentId { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? UpdatedAt { get; }
        /// <summary>
        ///...
        /// </summary>
        public int? MicrotingUId { get; }
        #endregion
    }
}