using System.Collections.Generic;

namespace ApiWebApp.Middleware
{
    public class PathPolicyConfig
    {
        public const string WellKnown_SectionName = "PathPolicy";
        public List<PathPolicyRecord> OptOut { get; set; }
        public List<PathPolicyRecord> OptIn { get; set; }
    }
}