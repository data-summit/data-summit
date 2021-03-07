using System;
using System.ComponentModel;

namespace DataSummitModels.BHoM.Geometry
{
    [Description("An axis aligned cuboid, which describes the smallest volume Box, oriented with the global coordinate system, that fully encompases a set of geometry." +
                 "\nDefined by its two extreme corner Points, these six coordinate values of Min and Max X, Y and Z are combined to specify all eight Box corners.")]
    public class BoundingBox : IGeometry
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("The lower bound values for the X, Y and Z coordinates of the Box corner Points.")]
        public virtual Point Min { get; set; } = new Point();

        [Description("The upper bound values for the X, Y and Z coordinates of the Box corner Points.")]
        public virtual Point Max { get; set; } = new Point();


        /***************************************************/
        /**** Static Operators Override                 ****/
        /***************************************************/

        public static BoundingBox operator +(BoundingBox a, BoundingBox b)
        {
            if (a == null || b == null)
                return null;

            return new BoundingBox
            {
                Min = new Point { X = Math.Min(a.Min.X, b.Min.X), Y = Math.Min(a.Min.Y, b.Min.Y), Z = Math.Min(a.Min.Z, b.Min.Z) },
                Max = new Point { X = Math.Max(a.Max.X, b.Max.X), Y = Math.Max(a.Max.Y, b.Max.Y), Z = Math.Max(a.Max.Z, b.Max.Z) }
            };
        }

        /***************************************************/

        public static BoundingBox operator +(BoundingBox box, Vector v)
        {
            if (box == null || v == null)
                return null;

            return new BoundingBox { Min = box.Min + v, Max = box.Max + v };
        }

        /***************************************************/
    }
}