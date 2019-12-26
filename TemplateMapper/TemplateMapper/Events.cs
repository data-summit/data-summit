using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateMapper
{
    public static class Events
    {
        private static List<String> lEvents = new List<String>();
        public static List<String> FindAll(Control cIn)
        {
            try
            {
                lEvents.Clear();
                lEvents = GetEventNames(cIn);
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return lEvents.OrderBy(e => e).ToList();
        }
        private static List<String> GetEventNames(Control cIn)
        {
            List<String> lLocalNames = new List<string>();
            try
            {
                if (cIn.Controls.Count > 0)
                {
                    foreach (Control c in cIn.Controls)
                    {
                        lLocalNames.AddRange(GetEventNames(c));
                    }
                }
                else
                {

                    EventHandlerList events = (EventHandlerList)typeof(Component)
                                                   .GetField("events", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField)
                                                   .GetValue(cIn);

                    object current = events.GetType()
                           .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField)[0]
                           .GetValue(events);

                    List<Delegate> delegates = new List<Delegate>();
                    while (current != null)
                    {
                        Delegate d = (Delegate)GetField(current, "handler");
                        lLocalNames.Add(d.Method.Name.ToString());
                        current = GetField(current, "next");
                    }
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return lLocalNames;
        }


        private static object GetField(object listItem, string fieldName)
        {
            return listItem.GetType()
               .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField)
               .GetValue(listItem);
        }
    }
}
