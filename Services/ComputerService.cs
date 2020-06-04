﻿using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using RDPShadow.Entities;

namespace RDPShadow.Services
{
    public class ComputerService
    {
        private readonly string _domain;
        private readonly string _containerDc;

        public ComputerService(string domain, string containerDc)
        {
            _domain = domain;
            _containerDc = containerDc;
        }

        public IEnumerable<Computer> Get()
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain, _containerDc);
            ComputerPrincipal searchExample = new ComputerPrincipal(context);
            PrincipalSearcher searcher = new PrincipalSearcher(searchExample);

            return searcher.FindAll()
                .OfType<ComputerPrincipal>()
                .Select(x => new Computer
                    {
                        Name = x.Name,
                        LastLogon = x.LastLogon,
                        DistinguishedName = x.DistinguishedName,
                        
                    });
        }
    }
}
