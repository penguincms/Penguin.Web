namespace Penguin.Web.Headers
{
    public class ContentLength : HttpHeader
    {
        public override HeaderMode Mode => HeaderMode.Ignore;
    }
}