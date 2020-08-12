using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Documents;
using RDPShadow.Entities;

namespace RDPShadow.Services
{
    public class RDPService
    {
        private readonly IComputerRepository _computerRepo;
        private readonly ISessionRepository _sessionRepo;

        public RDPService(IComputerRepository computerRepo, ISessionRepository sessionRepo)
        {
            _computerRepo = computerRepo;
            _sessionRepo = sessionRepo;
        }

        public void ShadowConnect(Computer computer, Session session, bool allowControl = false)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mstsc.exe",
                    Arguments = $"/shadow:{session.SessionId} /v:{computer.Name} {(allowControl? "/control" : "")} /noConsentPrompt",
                }
            };
            proc.Start();
        }

        public void Connect(Computer computer)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mstsc.exe",
                    Arguments = $"/v:{computer.Name}",
                }
            };
            proc.Start();
        }
        
        public async Task<IEnumerable<Session>> GetRemoteSessionsAsync(Computer computer) 
            => await _sessionRepo.GetRemoteSessionsAsync(computer);

        public IEnumerable<Computer> GetComputers() => _computerRepo.Get();
    }
}
