using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models.Azure
{
    [Serializable]
    public class ListOCRs
    {
        public List<BlobOCR> lRes = new List<BlobOCR>();

        public ListOCRs()
        { }
    }
}
