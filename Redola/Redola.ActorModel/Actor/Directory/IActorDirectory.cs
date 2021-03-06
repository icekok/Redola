﻿using System.Net;

namespace Redola.ActorModel
{
    public interface IActorDirectory
    {
        bool Active { get; }
        void Activate(ActorIdentity localActor);
        void Close();

        IPEndPoint LookupRemoteActorEndPoint(string actorType, string actorName);
        IPEndPoint LookupRemoteActorEndPoint(string actorType);
    }
}
