﻿using System;

namespace Redola.Rpc
{
    public sealed class MessageHandleStrategy
    {
        public MessageHandleStrategy()
        {
            this.IsHandledInSeparateThread = true;
            this.IsRequestResponseModel = false;
        }

        public MessageHandleStrategy(Type messageType)
            : this()
        {
            this.MessageType = messageType;
        }

        public Type MessageType { get; set; }

        public bool IsHandledInSeparateThread { get; set; }

        public bool IsRequestResponseModel { get; set; }

        public override string ToString()
        {
            return string.Format("MessageType[{0}], IsHandledInSeparateThread[{1}], IsRequestResponseModel[{2}]",
                this.MessageType, this.IsHandledInSeparateThread, this.IsRequestResponseModel);
        }
    }
}
