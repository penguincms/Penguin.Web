namespace Penguin.Web.Headers
{
    public class Connection : HttpHeader
    {
        public override HeaderMode Mode => HeaderMode.Ignore;
    }
}