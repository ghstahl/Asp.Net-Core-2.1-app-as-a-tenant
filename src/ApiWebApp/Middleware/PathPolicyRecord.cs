using System.Collections.Generic;

namespace ApiWebApp.Middleware
{
    public class PathPolicyRecord
    {
        public string Policy { get; set; }
        public List<string> Paths { get; set; }
    }
}