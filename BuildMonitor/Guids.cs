// Guids.cs
// MUST match guids.h

using System.Linq;
using System.Collections.Generic;
using System;

namespace BuildMonitor
{
    static class GuidList
    {
        public const string guidBuildMonitorPkgString = "04bd76e5-b9bd-4772-aca9-60a1dc767583";
        public const string guidBuildMonitorCmdSetString = "8cdaca99-207d-4de6-9ac4-99b142250ce5";

        public static readonly Guid guidBuildMonitorCmdSet = new Guid(guidBuildMonitorCmdSetString);
    };
}