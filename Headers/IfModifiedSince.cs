namespace Penguin.Web.Headers
{
    public class IfModifiedSince : HttpHeader
    {
        public override HeaderMode Mode => HeaderMode.Ignore;
    }
}