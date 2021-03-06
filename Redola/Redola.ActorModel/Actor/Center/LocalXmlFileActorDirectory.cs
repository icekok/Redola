﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Logrila.Logging;

namespace Redola.ActorModel
{
    public class LocalXmlFileActorDirectory : IActorDirectory
    {
        private ILog _log = Logger.Get<LocalXmlFileActorDirectory>();
        private LocalXmlFileActorConfiguration _configuration;

        public LocalXmlFileActorDirectory(LocalXmlFileActorConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public bool Active { get; private set; }

        public void Activate(ActorIdentity localActor)
        {
            this.Active = true;
        }

        public void Close()
        {
            this.Active = false;
        }

        public IPEndPoint LookupRemoteActorEndPoint(string actorType, string actorName)
        {
            if (string.IsNullOrEmpty(actorType))
                throw new ArgumentNullException("actorType");
            if (string.IsNullOrEmpty(actorName))
                throw new ArgumentNullException("actorName");

            var endpoint = LookupRemoteActorEndPoint(
                actorType,
                (actors) =>
                {
                    return actors.FirstOrDefault(a => a.Type == actorType && a.Name == actorName);
                });

            if (endpoint == null)
                throw new ActorNotFoundException(string.Format(
                    "Cannot lookup remote actor, Type[{0}], Name[{1}].", actorType, actorName));

            return endpoint;
        }

        public IPEndPoint LookupRemoteActorEndPoint(string actorType)
        {
            if (string.IsNullOrEmpty(actorType))
                throw new ArgumentNullException("actorType");

            var endpoint = LookupRemoteActorEndPoint(
                actorType,
                (actors) =>
                {
                    return actors.Where(a => a.Type == actorType).OrderBy(t => Guid.NewGuid()).FirstOrDefault();
                });

            if (endpoint == null)
                throw new ActorNotFoundException(string.Format(
                    "Cannot lookup remote actor, Type[{0}].", actorType));

            return endpoint;
        }

        private IPEndPoint LookupRemoteActorEndPoint(string actorType, Func<IEnumerable<ActorIdentity>, ActorIdentity> matchActorFunc)
        {
            var actor = matchActorFunc(_configuration.ActorDirectory);
            if (actor != null)
            {
                IPAddress actorAddress = ResolveIPAddress(actor.Address);
                int actorPort = int.Parse(actor.Port);
                var actorEndPoint = new IPEndPoint(actorAddress, actorPort);
                return actorEndPoint;
            }

            return null;
        }

        private IPAddress ResolveIPAddress(string host)
        {
            IPAddress remoteIPAddress = null;

            IPAddress ipAddress;
            if (IPAddress.TryParse(host, out ipAddress))
            {
                remoteIPAddress = ipAddress;
            }
            else
            {
                if (host.ToLowerInvariant() == "localhost")
                {
                    remoteIPAddress = IPAddress.Parse(@"127.0.0.1");
                }
                else
                {
                    IPAddress[] addresses = Dns.GetHostAddresses(host);
                    if (addresses.Any())
                    {
                        remoteIPAddress = addresses.First();
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            string.Format("Cannot resolve host [{0}] from DNS.", host));
                    }
                }
            }

            return remoteIPAddress;
        }
    }
}
