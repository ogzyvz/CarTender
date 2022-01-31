using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Infoline
{
    public static class WinEventLog
    {
        public static string sSource = "Web";
        public static string sLog = "Infoline";
        public static void Error(Exception ex)
        {
            using (var wi = WindowsIdentity.GetCurrent())
            {
                var wp = new WindowsPrincipal(wi);
                //admin 
                if (new WindowsPrincipal(wi).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    if (!EventLog.SourceExists(sSource))
                        EventLog.CreateEventSource(sSource, sLog);
                    EventLog.WriteEntry(sSource, ex.Message, EventLogEntryType.Error, 999);
                }
            }
        }
        public static void Warning(Exception ex)
        {
            using (var wi = WindowsIdentity.GetCurrent())
            {
                var wp = new WindowsPrincipal(wi);
                //admin 
                if (new WindowsPrincipal(wi).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    if (!EventLog.SourceExists(sSource))
                        EventLog.CreateEventSource(sSource, sLog);
                    EventLog.WriteEntry(sSource, ex.Message, EventLogEntryType.Warning, 999);
                }
            }
        }
        public static void Information(Exception ex)
        {
            using (var wi = WindowsIdentity.GetCurrent())
            {
                var wp = new WindowsPrincipal(wi);
                //admin 
                if (new WindowsPrincipal(wi).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    if (!EventLog.SourceExists(sSource))
                        EventLog.CreateEventSource(sSource, sLog);
                    EventLog.WriteEntry(sSource, ex.Message, EventLogEntryType.Information, 999);
                }
            }
        }
    }
}