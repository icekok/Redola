﻿using System;
using Redola.ActorModel;

namespace Redola.Rpc
{
    public abstract class BlockingActorMessageHandlerBase : RouteActorMessageHandlerBase, IBlockingActorMessageHandler
    {
        private BlockingRouteActor _localActor;

        public BlockingActorMessageHandlerBase(BlockingRouteActor localActor)
            : base(localActor)
        {
            if (localActor == null)
                throw new ArgumentNullException("localActor");
            _localActor = localActor;
        }

        public new BlockingRouteActor Actor { get { return _localActor; } }

        protected override void DoHandleMessage(ActorIdentity remoteActor, ActorMessageEnvelope envelope)
        {
            if (GetAdmissibleMessageHandleStrategy(envelope.MessageType).IsRequestResponseModel)
            {
                envelope.HandledBy(this.Actor, GetAdmissibleMessageType(envelope.MessageType), this.Actor.Decoder, remoteActor,
                    (object o) =>
                    {
                        return o
                            .GetType()
                            .GetMethod("OnSyncMessage")
                            .MakeGenericMethod(GetAdmissibleMessageType(envelope.MessageType));
                    });
            }
            else
            {
                base.DoHandleMessage(remoteActor, envelope);
            }
        }
    }
}
