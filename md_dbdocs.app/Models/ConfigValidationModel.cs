﻿using System;

namespace md_dbdocs.app.Models
{
    public class ConfigValidationModel
    {
        public string ItemId { get; set; }
        public string ValidationItem { get; set; }
        public bool IsValid { get; set; }
        public string ExtMessage { get; set; }
    }
}
