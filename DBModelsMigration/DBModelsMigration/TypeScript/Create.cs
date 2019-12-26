using DBModelsMigration.DBMigrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DBModelsMigration.TypeScript
{
    public static class Create
    {
        public static void Files()
        {
            try
            {
                //string inputPath = @"C:\DataSummit\DBModelsMigration\DBModelsMigration\DBMigrations";
                string outputPath = @"C:\DataSummit\DBModelsMigration\DBModelsMigration\TypeScript\Models";

                if (Directory.Exists(outputPath))
                {
                    var s = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && t.Namespace == "DBModelsMigration.DBMigrations").ToList();
                    foreach(Type t in s)
                    {
                        var u = t.GetProperties().ToList();
                        if (u.Count > 0 && t.Name.Contains("Context") == false)
                        {
                            string sClassName = DePluraliseClassName(t.Name.ToString(), false);
                            string sFileName = DePluraliseClassName(t.Name.ToString(), true);
                            using (StreamWriter sw = new StreamWriter(outputPath + @"\" + sFileName + ".ts"))
                            {
                                sw.WriteLine();
                                sw.WriteLine("export class " + sClassName + " {");
                                sw.WriteLine();
                                foreach (PropertyInfo p in u)    //List variables
                                {
                                    string s1 = "\t" + p.Name + IsNullable(p) + ": " + TSVariableConverter(p) + ";";
                                    sw.WriteLine(s1);
                                }
                                sw.WriteLine();
                                List<string> pNamesExtended = new List<string>();
                                List<string> pNames= new List<string>();
                                foreach (PropertyInfo p in u)    //constructor parameters
                                {
                                    if (p.Name != string.Empty && char.IsUpper(p.Name[0]))
                                    {
                                        string pName = char.ToLower(p.Name[0]) + p.Name.Substring(1);
                                        pNamesExtended.Add(pName + "?: " + TSVariableConverter(p) + ",");
                                        pNames.Add(pName);
                                    }
                                }
                                pNamesExtended[0] = "\tconstructor(" + pNamesExtended[0];
                                pNamesExtended[pNamesExtended.Count - 1] = pNamesExtended[pNamesExtended.Count - 1].Substring(0, pNamesExtended[pNamesExtended.Count - 1].Length - 1) + ")";
                                for (int i = 0; i < pNamesExtended.Count; i = i + 3)
                                {
                                    string s1 = "";
                                    if (i + 3 > pNamesExtended.Count)
                                    { s1 = string.Join(" ", pNamesExtended.GetRange(i, pNamesExtended.Count % 3)); }
                                    else
                                    { s1 = string.Join(" ", pNamesExtended.GetRange(i, 3)); }
                                    if (i == 0)
                                    { sw.WriteLine(s1); }
                                    else
                                    { sw.WriteLine("\t\t" + s1); }
                                }
                                sw.WriteLine("\t" + "{");
                                foreach (PropertyInfo p in u)    //constructor set variables with parameters
                                {
                                    string s1 = "\t\tthis." + p.Name + " = " + ParameterSetPair(p, pNames[u.IndexOf(p)], false) + ";";
                                    sw.WriteLine(s1);
                                }
                                sw.WriteLine("\t" + "}");
                                sw.WriteLine("}");
                                sw.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
        }
        private static string TSVariableConverter(PropertyInfo p)
        {
            string sOut = "";
            try
            {
                if (p.PropertyType.Name == "Nullable`1")
                { return TSVariableConverter(p.PropertyType.GetProperties()[1]); }
                else if (p.PropertyType.Name == "Int64" || p.PropertyType.Name == "Int32" ||
                    p.PropertyType.Name == "Int16" || p.PropertyType.Name == "Byte" ||
                    p.PropertyType.Name == "Decimal" || p.PropertyType.Name == "Single" ||
                    p.PropertyType.Name == "Double")
                { return "number"; }
                else if (p.PropertyType.Name == "String")
                { return "string"; }
                else if (p.PropertyType.Name == "bool" || p.PropertyType.Name == "Boolean")
                { return "boolean"; }
                else if (p.PropertyType.Name == "DateTime")
                { return "Date | string"; }
                else if (p.PropertyType.Name.Contains("ICollection") || p.PropertyType.Name.Contains("IList"))
                {
                    List<Type> lGA = p.PropertyType.GetGenericArguments().ToList();
                    return lGA[0].Name + "[]";
                }
                else
                { return p.Name; }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return sOut;
        }
        private static string IsNullable(PropertyInfo p)
        {
            try
            {
                if (p.PropertyType.Name == "Nullable`1")
                { return "?"; }
                else
                { return ""; }
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return "";
        }
        private static string ParameterSetPair(PropertyInfo p, string va, bool IsNullable)
        {
            string s = "";
            try
            {
                if (p.PropertyType.Name == "Nullable`1")
                { s = ParameterSetPair(p.PropertyType.GetProperties()[1], va, true); }
                else if (p.PropertyType.Name == "Int64" || p.PropertyType.Name == "Int32" ||
                    p.PropertyType.Name == "Int16" || p.PropertyType.Name == "Byte" ||
                    p.PropertyType.Name == "Decimal" || p.PropertyType.Name == "Single" ||
                    p.PropertyType.Name == "Double")
                { s = va + " || 0"; }
                else if (p.PropertyType.Name == "String")
                { s = va + " || \"\""; }
                else if (p.PropertyType.Name == "bool" || p.PropertyType.Name == "Boolean")
                { return " || false"; }
                else if (p.PropertyType.Name == "DateTime")
                { s = va + " || new Date(Date.now())"; }
                else if (p.PropertyType.Name.Contains("ICollection") || p.PropertyType.Name.Contains("IList"))
                { s = va; }
                else
                { s = va; }

                if (IsNullable == true && s.Contains("null") == false) s = s + " || null";
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return s;
        }
        private static string DePluraliseClassName(string s, bool LowerFirstLetter)
        {
            string sOut = "";
            try
            {
                if (s.Substring(s.Length - 3, 3) == "ies") sOut = s.Substring(0, s.Length - 3) + "y";
                else if (s.Substring(s.Length - 4, 4) == "sses") sOut = s.Substring(0, s.Length - 2);
                else if (s.Substring(s.Length - 1, 1) == "s") sOut = s.Substring(0, s.Length - 1);
                else sOut = s;

                if (LowerFirstLetter == true)
                {
                    if (sOut != string.Empty && char.IsUpper(sOut[0]))
                    {
                        sOut = char.ToLower(sOut[0]) + sOut.Substring(1);
                    }
                }
            }
            catch (Exception ae)
            {
                string strError = ae.ToString();
            }
            return sOut;
        }
    }
}