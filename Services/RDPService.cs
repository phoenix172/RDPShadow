using System.Diagnostics;
using RDPShadow.Entities;

namespace RDPShadow.Services
{
    public class RDPService
    {
        public void ShadowConnect(Computer computer, Session session)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mstsc.exe",
                    Arguments = $"/shadow:{session.SessionId} /v:{computer.Name} /control /noConsentPrompt",
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
    }
}
