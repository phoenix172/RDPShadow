using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassia;
using RDPShadow.Entities;

namespace RDPShadow.Services
{
    public class SessionService
    {
        private readonly int _queryTimeout;

        public SessionService(int queryTimeout = 1000)
        {
            _queryTimeout = queryTimeout;
        }

        private IReadOnlyList<Session> EmptySessionList => new List<Session>();

        public async Task<IReadOnlyList<Session>> GetRemoteSessionsAsync(Computer computer)
        {
            Exception exception = null;
            IReadOnlyList<Session> sessions = new List<Session>();
            var querySessionTask = Task.Run(()=>
                        sessions = RemoteSessions(computer)
                    )
                .ContinueWith(t=>
                    {
                        exception = new SessionQueryException(t.Exception);
                    },
                    TaskContinuationOptions.OnlyOnFaulted);

            var completedTask = await Task.WhenAny(querySessionTask, Task.Delay(_queryTimeout));
            if (completedTask != querySessionTask)
            {
                return EmptySessionList;
            }

            return sessions;
        }

        private IReadOnlyList<Session> RemoteSessions(Computer computer)
        {
            ITerminalServicesManager manager = new TerminalServicesManager();
            using ITerminalServer server = manager.GetRemoteServer(computer.Name);
            server.Open();
            if(!server.IsOpen)
                return new List<Session>();

            try
            {
                return server.GetSessions()
                    .Where(x=>
                        (x.ConnectionState == ConnectionState.Active ||
                         x.ConnectionState==ConnectionState.Disconnected)
                        && !string.IsNullOrWhiteSpace(x.UserName))
                    .Select(x => new Session
                    {
                        UserName = x.UserName,
                        SessionId = x.SessionId,
                        Status = x.ConnectionState.ToString()
                    }).ToList();
            }
            catch
            {
                return EmptySessionList;
            }
        }
    }
}
