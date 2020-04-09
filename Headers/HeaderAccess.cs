using System;

namespace Penguin.Web.Headers
{
    [Flags]
    public enum HeaderAccess
    {
        Unset = 0,
        Server = 1,
        Client = 2,
        ServerAndClient = Server | Client
    }
}