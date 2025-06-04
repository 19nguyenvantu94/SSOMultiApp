// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Authen.Users.Models
{    
    public class Log
    {
        public long Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public string MessageTemplate { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;

        public DateTimeOffset TimeStamp { get; set; }

        public string Exception { get; set; } = string.Empty;

        public string LogEvent { get; set; } = string.Empty;

        public string Properties { get; set; } = string.Empty;

        [NotMapped]
        public XElement PropertiesXml => XElement.Parse(Properties);
    }
}
