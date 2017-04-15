using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Interfaces.Infrastructure;

namespace Eventualize.EventStore.Infrastructure
{
    public class ConnectionEventHandler
    {
        private IEventualizeLogger logger;

        public ConnectionEventHandler(IEventualizeLogger logger)
        {
            this.logger = logger;
        }

        public void SetConnection(IEventStoreConnection connection)
        {
            connection.AuthenticationFailed += this.Connection_AuthenticationFailed;
            connection.Closed += this.Connection_Closed;
            connection.Connected += this.Connection_Connected;
            connection.Disconnected += this.Connection_Disconnected;
            connection.ErrorOccurred += this.Connection_ErrorOccurred;
            connection.Reconnecting += this.Connection_Reconnecting;
        }

        private void Connection_Reconnecting(object sender, ClientReconnectingEventArgs e)
        {
            this.logger.Trace($"EventStore connection {e.Connection.ConnectionName} is reconnecting");
        }

        private void Connection_ErrorOccurred(object sender, ClientErrorEventArgs e)
        {
            this.logger.Trace($"Error occured in EventStore connection {e.Connection.ConnectionName}: {e.Exception.ToString()}");
        }

        private void Connection_Disconnected(object sender, ClientConnectionEventArgs e)
        {
            this.logger.Trace($"EventStore connection {e.Connection.ConnectionName} was disconnected from {e.RemoteEndPoint.ToString()}.");
        }

        private void Connection_Connected(object sender, ClientConnectionEventArgs e)
        {
            this.logger.Trace($"EventStore connection {e.Connection.ConnectionName} is now connected to {e.RemoteEndPoint.ToString()}");
        }

        private void Connection_Closed(object sender, ClientClosedEventArgs e)
        {
            this.logger.Trace($"EventStore connection {e.Connection.ConnectionName} was closed because: {e.Reason}");
        }

        private void Connection_AuthenticationFailed(object sender, ClientAuthenticationFailedEventArgs e)
        {
            this.logger.Trace($"Authentication failed for EventStore connection {e.Connection.ConnectionName}");
        }
    }
}
