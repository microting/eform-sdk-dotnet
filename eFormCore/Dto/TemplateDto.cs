using System;
using System.Collections.Generic;
using Microting.eForm.Dto;

namespace Microting.eForm.Dto
{
    #region Template_Dto
    public class Template_Dto
    {
        #region con
        public Template_Dto()
        {

        }

        public Template_Dto(int id, DateTime? createdAt, DateTime? updatedAt, string label, string description, int repeated, string folderName, string workflowState, List<SiteNameDto> deployedSites, bool hasCases, int? displayIndex, List<KeyValuePair<int, string>> tags)
        {
            Id = id;
            Label = label;
            Description = description;
            Repeated = repeated;
            FolderName = folderName;
            WorkflowState = workflowState;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            DeployedSites = deployedSites;
            HasCases = hasCases;
            DisplayIndex = displayIndex;
            Tags = tags;
        }



        public Template_Dto(int id, 
            DateTime? createdAt, 
            DateTime? updatedAt, 
            string label, 
            string description, 
            int repeated, 
            string folderName, 
            string workflowState, 
            List<SiteNameDto> deployedSites, 
            bool hasCases, 
            int? displayIndex, 
            FieldDto field1, 
            FieldDto field2, 
            FieldDto field3, 
            FieldDto field4, 
            FieldDto field5, 
            FieldDto field6, 
            FieldDto field7, 
            FieldDto field8, 
            FieldDto field9, 
            FieldDto field10, 
            List<KeyValuePair<int, string>> tags,
            bool jasperExportEnabled,
            bool docxExportEnabled)
        {
            Id = id;
            Label = label;
            Description = description;
            Repeated = repeated;
            FolderName = folderName;
            WorkflowState = workflowState;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            DeployedSites = deployedSites;
            HasCases = hasCases;
            DisplayIndex = displayIndex;
            Field1 = field1;
            Field2 = field2;
            Field3 = field3;
            Field4 = field4;
            Field5 = field5;
            Field6 = field6;
            Field7 = field7;
            Field8 = field8;
            Field9 = field9;
            Field10 = field10;
            Tags = tags;
            JasperExportEnabled = jasperExportEnabled;
            DocxExportEnabled = docxExportEnabled;
        }
        #endregion

        #region var
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? UpdatedAt { get; }

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
        public int Repeated { get; }

        /// <summary>
        /// FolderName
        /// </summary>
        public string FolderName { get; }

        /// <summary>
        /// WorkflowState
        /// </summary>
        public string WorkflowState { get; }

        /// <summary>
        /// WorkflowState
        /// </summary>
        public List<SiteNameDto> DeployedSites { get; }

        /// <summary>
        /// WorkflowState
        /// </summary>
        public bool HasCases { get; }

        /// <summary>
        /// DiplayIndex
        /// </summary>
        public int? DisplayIndex { get; }

        /// <summary>
        /// Field1
        /// </summary>
        public FieldDto Field1 { get; }

        /// <summary>
        /// Field2
        /// </summary>
        public FieldDto Field2 { get; }

        /// <summary>
        /// Field3
        /// </summary>
        public FieldDto Field3 { get; }

        /// <summary>
        /// Field4
        /// </summary>
        public FieldDto Field4 { get; }

        /// <summary>
        /// Field5
        /// </summary>
        public FieldDto Field5 { get; }

        /// <summary>
        /// Field6
        /// </summary>
        public FieldDto Field6 { get; }

        /// <summary>
        /// Field7
        /// </summary>
        public FieldDto Field7 { get; }

        /// <summary>
        /// Field8
        /// </summary>
        public FieldDto Field8 { get; }

        /// <summary>
        /// Field9
        /// </summary>
        public FieldDto Field9 { get; }

        /// <summary>
        /// Field10
        /// </summary>
        public FieldDto Field10 { get; }

        public bool JasperExportEnabled { get; set; }
        
        public bool DocxExportEnabled { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<KeyValuePair<int, string>> Tags { get; } 
        #endregion
    }
    #endregion
}