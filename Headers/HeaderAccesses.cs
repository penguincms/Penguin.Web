using System;

namespace Penguin.Web.Headers
{
    [Flags]
    public enum HeaderAccesses
    {
        Unset = 0,
        Server = 1,
        Client = 2,
        ServerAndClient = Server | Client
    }
}