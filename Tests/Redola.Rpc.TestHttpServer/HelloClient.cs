﻿using System;
using System.Collections.Generic;
using Logrila.Logging;
using Redola.Rpc.TestContracts;

namespace Redola.Rpc.TestHttpServer
{
    public class HelloClient : RpcService
    {
        private ILog _log = Logger.Get<HelloClient>();

        public HelloClient(RpcActor localActor)
            : base(localActor)
        {
        }

        protected override IEnumerable<RpcMessageRegistration> RegisterRpcMessages()
        {
            var messages = new List<RpcMessageRegistration>();

            messages.Add(new RpcMessageRegistration(typeof(HelloResponse)) { IsRequestResponseModel = true });
            messages.Add(new RpcMessageRegistration(typeof(Hello10000Response)) { IsRequestResponseModel = true });

            return messages;
        }

        public ActorMessageEnvelope<HelloResponse> SayHello()
        {
            var request = new ActorMessageEnvelope<HelloRequest>()
            {
                Message = new HelloRequest() { Text = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss.fffffff") },
            };

            return this.Actor.Send<HelloRequest, HelloResponse>("server", request);
        }

        public ActorMessageEnvelope<Hello10000Response> SayHello10000()
        {
            var request = new ActorMessageEnvelope<Hello10000Request>()
            {
                Message = new Hello10000Request() { Text = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss.fffffff") },
            };

            return this.Actor.Send<Hello10000Request, Hello10000Response>("server", request);
        }
    }
}
