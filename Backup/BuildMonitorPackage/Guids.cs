// Guids.cs
// MUST match guids.h

using System;

namespace BuildMonitorPackage
{
    static class GuidList
    {
        public const string guidBuildMonitorPackagePkgString = "d350a95e-5d09-4b5d-9075-0a5f7bb6b1dc";
        public const string guidBuildMonitorPackageCmdSetString = "ec1339d5-c4f2-44de-9908-af4b79ce6f74";

        public static readonly Guid guidBuildMonitorPackageCmdSet = new Guid(guidBuildMonitorPackageCmdSetString);
    };
}