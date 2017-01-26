﻿using Alchemy4Tridion.Plugins.Clients.CoreService;
using System;

namespace Alchemy4Tridion.Plugins.Clients
{
    public class AlchemyClients : IDisposable
    {
        private string currentUser;

        private IAlchemyCoreServiceClient sessionAwareCoreServiceClient;

        /// <summary>
        /// The default session aware core service endpoint to use.
        /// TODO: Make this dynamic based on what version of Tridion is running.
        /// </summary>
        public SessionAwareCoreServiceEndPoint DefaultCoreServiceEndPoint
        {
            get;
            set;
        } = SessionAwareCoreServiceEndPoint.NetTcp2013;

        /// <summary>
        /// Gets a session aware core service client that can be used for scope duration.  Uses the DefaultCoreServiceEndPoint to open.
        /// </summary>
        public IAlchemyCoreServiceClient SessionAwareCoreServiceClient
        {
            get
            {
                if (this.sessionAwareCoreServiceClient == null)
                {
                    
                    this.sessionAwareCoreServiceClient = CreateSessionAwareCoreServiceClient(DefaultCoreServiceEndPoint);
                }
                return this.sessionAwareCoreServiceClient;
            }
        }

        public AlchemyClients()
        {

        }

        public AlchemyClients(string currentUser)
        {
            this.currentUser = currentUser;
        }

        public IAlchemyCoreServiceClient CreateSessionAwareCoreServiceClient(SessionAwareCoreServiceEndPoint endpoint)
        {
            switch (endpoint)
            {
                case SessionAwareCoreServiceEndPoint.NetTcp201501:
                    var client201501 = new AlchemyCoreServiceClient201501();
                    client201501.Impersonate(this.currentUser);
                    return client201501;
                default:
                    var client2013 = new AlchemyCoreServiceClient2013();
                    client2013.Impersonate(this.currentUser);
                    return client2013;
            }
            
        }

        public void Dispose()
        {
            if (this.sessionAwareCoreServiceClient != null)
            {
                this.sessionAwareCoreServiceClient.Dispose();
            }
        }
    }
}