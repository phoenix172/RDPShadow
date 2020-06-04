using System;

namespace RDPShadow.Entities
{
    public class Computer
    {
        public string Name { get; set; }
        public DateTime? LastLogon { get; set; }
        public string DistinguishedName { get; set; }
        public string Container { get; set; }
    }
}