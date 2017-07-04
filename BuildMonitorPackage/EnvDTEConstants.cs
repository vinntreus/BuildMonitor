using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMonitorPackage
{
    // I don't unerstand why we need this class, but it mimics EnvDTE.Constants
    // However using EnvDTE.Constants produces compile errors, which is why we have this instead
    // More information here: https://blogs.msdn.microsoft.com/mshneer/2009/12/07/vs-2010-compiler-error-interop-type-xxx-cannot-be-embedded-use-the-applicable-interface-instead/
    internal class EnvDTEConstants
    {
        public const string vsWindowKindOutput = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}";
    }
}
