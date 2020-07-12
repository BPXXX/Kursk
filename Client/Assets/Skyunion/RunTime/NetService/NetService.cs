using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyunion
{
    internal class NetService : Module, INetServcice
    {
        private List<NetClient> netClients = new List<NetClient>();

        public INetClient CreateClient(Action<NetEvent,int> connectEvent, Action<MemoryStream> reciveEvent, ProtocolResolverDelegate protocolResolver)
        {
            IProtocolResolver resolver;
            if (protocolResolver == null)
            {
                resolver = new BytesProtocolResolver();
            }
            else
            {
                resolver = new ProtocolResolverCallback(protocolResolver);
            }
            var netClient = new NetClient(connectEvent, reciveEvent, resolver);
            netClients.Add(netClient);
            return netClient;
        }

        public override void Update()
        {
            foreach(var netClient in netClients)
            {
                netClient.Update();
            }
        }
    }
}
