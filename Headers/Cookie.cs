namespace Penguin.Web.Headers
{
    public class Cookie : HttpHeader
    {
        public override HeaderMode Mode => HeaderMode.MultipleValues;
    }
}