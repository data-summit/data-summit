﻿using System;

namespace DataSummitFunctions.Models.Amazon
{
    [Serializable]
    public class ResponseMetadata
    {
        public string RequestId { get; set; }
        public Metadata Metadata { get; set; }

        public ResponseMetadata()
        { }
    }
}
