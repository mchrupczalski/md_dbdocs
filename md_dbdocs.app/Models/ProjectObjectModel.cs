﻿using md_dbdocs.app.Models.YamlModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace md_dbdocs.app.Models
{
    public class ProjectObjectModel
    {
        public FileInfo FileInfo { get; set; }
        public string CreateObjectType { get; set; }
        public string CreateObjectSchema { get; set; }
        public string CreateObjectName { get; set; }
        public bool HasTagDbDocs { get; set; }
        public bool HasTagDiagram { get; set; }
        public bool HasTagChangeLog { get; set; }
        public HeaderCommentModel HeaderModel { get; set; }
        public List<Exception> ProcessingExceptions { get; set; }

        public ProjectObjectModel()
        {
            this.ProcessingExceptions = new List<Exception>();
        }

        public StreamReader GetFileText()
        {
            StreamReader reader = new StreamReader(this.FileInfo.FullName);
            return reader;
        }
    }
}
