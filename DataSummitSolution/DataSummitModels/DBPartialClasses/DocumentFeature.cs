using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class DocumentFeature
    {
        public override bool Equals(object obj)
        {
            var documentFeature = obj as DocumentFeature;
            var isEqual = (DocumentId == documentFeature.DocumentId) &&
                          (Value == documentFeature.Value) &&
                          (Left == documentFeature.Left) &&
                          (Top == documentFeature.Top) &&
                          (Width == documentFeature.Width) &&
                          (Height == documentFeature.Height) &&
                          (Feature == documentFeature.Feature) &&
                          (Confidence == documentFeature.Confidence);


            return isEqual;
        }

        public override int GetHashCode() => HashCode.Combine(DocumentId, Value, Left, Top, Width, Height, Feature, Confidence);
    }
}
