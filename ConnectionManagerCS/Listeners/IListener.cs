//************************************************
// IListener.cs
//
// Interface for all listeners
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System.Collections.Generic;

namespace ConnectionManagerCS.Listeners
{
    public interface IListener
    {
        void Start();
        void Stop();
        Connection[] Clients { get; }
    }
}
