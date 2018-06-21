using System.Collections.Generic;

namespace FrameworkDemo.Models
{
    public struct RequireJsConfig
    {
        public Dictionary<string, string> Paths { get; set; }

        public Dictionary<string, string[]> Shim { get; set; }
    }
}