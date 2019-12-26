using MoreLinq;

using SimMetricsMetricUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateMapper.OCR.Consolidated;

namespace TemplateMapper.OCR.PostProcessing
{
    public static class MultipleVendors
    {
        public static List<Sentence> ltransferOCR = new List<Sentence>();
        public static List<Sentence> lAnomalies = new List<Sentence>();
        public static List<Sentence> Clean(List<Sentence> transferOCR)
        {
            List<Sentence> localOCR = new List<Sentence>();
            try
            {
                Dictionary<string, List<int>> dTallies = new Dictionary<string, List<int>>();

                short Tolerance = 15;

                //All OCR attempts to consolidate OCR transferOCR need to be passed via this class
                List<string> lVendors = transferOCR.Select(s => s.Vendor).Distinct().ToList();
                Dictionary<string, List<Sentence>> dVendorsSource = new Dictionary<string, List<Sentence>>();
                foreach(string vendor in lVendors)
                {
                    List<Sentence> lSents = new List<Sentence>();
                    //cOCR.FileName = transferOCR.FileName;
                    lSents = transferOCR.Where(s => s.Vendor == vendor).ToList();
                    dVendorsSource.Add(vendor, lSents);
                }

                //Self vendor cleansing
                Dictionary<string, List<Sentence>> dVendorsCleaned = new Dictionary<string, List<Sentence>>();
                foreach (KeyValuePair<string, List<Sentence>> kvVendor in dVendorsSource)
                {
                    SelfLocal self = new SelfLocal();
                    List<Sentence> cOCR = self.Clean(kvVendor.Value);
                    //cOCR.FileName = transferOCR.FileName;
                    dVendorsCleaned.Add(kvVendor.Key, cOCR);
                    dTallies.Add(kvVendor.Key, self.TotalTally);
                }

                //Cross vendor cleansing

                foreach(KeyValuePair<string, List<Sentence>> kvVendor in dVendorsCleaned)
                {
                    transferOCR.AddRange(kvVendor.Value.ToList());
                }

            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return transferOCR;
        }
    }
}
