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

using System;
using System.Collections.Generic;

namespace Microting.eForm.Dto
{
    #region Template_Dto

    public class Template_Dto
    {
        #region con

        public Template_Dto()
        {
        }

        public Template_Dto(int id, DateTime? createdAt, DateTime? updatedAt, string label, string description,
            int repeated, string folderName, string workflowState, List<SiteNameDto> deployedSites, bool hasCases,
            int? displayIndex, List<KeyValuePair<int, string>> tags)
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
            bool docxExportEnabled, bool excelExportEnabled)
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
            ExcelExportEnabled = excelExportEnabled;
        }

        #endregion

        #region var

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Descrition
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Repeated
        /// </summary>
        public int Repeated { get; set; }

        /// <summary>
        /// FolderName
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// WorkflowState
        /// </summary>
        public string WorkflowState { get; set; }

        /// <summary>
        /// WorkflowState
        /// </summary>
        public List<SiteNameDto> DeployedSites { get; set; }

        /// <summary>
        /// WorkflowState
        /// </summary>
        public bool HasCases { get; set; }

        /// <summary>
        /// DiplayIndex
        /// </summary>
        public int? DisplayIndex { get; set; }

        /// <summary>
        /// Field1
        /// </summary>
        public FieldDto Field1 { get; set; }

        /// <summary>
        /// Field2
        /// </summary>
        public FieldDto Field2 { get; set; }

        /// <summary>
        /// Field3
        /// </summary>
        public FieldDto Field3 { get; set; }

        /// <summary>
        /// Field4
        /// </summary>
        public FieldDto Field4 { get; set; }

        /// <summary>
        /// Field5
        /// </summary>
        public FieldDto Field5 { get; set; }

        /// <summary>
        /// Field6
        /// </summary>
        public FieldDto Field6 { get; set; }

        /// <summary>
        /// Field7
        /// </summary>
        public FieldDto Field7 { get; set; }

        /// <summary>
        /// Field8
        /// </summary>
        public FieldDto Field8 { get; set; }

        /// <summary>
        /// Field9
        /// </summary>
        public FieldDto Field9 { get; set; }

        /// <summary>
        /// Field10
        /// </summary>
        public FieldDto Field10 { get; set; }

        public int? FolderId { get; set; }

        public bool JasperExportEnabled { get; set; }

        public bool DocxExportEnabled { get; set; }

        public bool ExcelExportEnabled { get; set; }

        public string ReportH1 { get; set; }

        public string ReportH2 { get; set; }

        public string ReportH3 { get; set; }

        public string ReportH4 { get; set; }

        public string ReportH5 { get; set; }

        public bool IsLocked { get; set; }

        public bool IsEditable { get; set; }

        public bool IsHidden { get; set; }

        public bool IsAchievable { get; set; }

        public bool IsDoneAtEditable { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        public List<KeyValuePair<int, string>> Tags { get; set; }

        #endregion
    }

    #endregion
}