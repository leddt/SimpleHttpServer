using System.Web;

namespace DDT.SimpleHttpServer
{
    public class HtmlHelper
    {
        public string Encode(string input)
        {
            return HttpUtility.HtmlEncode(input);
        }

        public string AttributeEncode(string input)
        {
            return HttpUtility.HtmlAttributeEncode(input);
        }

        public string UrlEncode(string input)
        {
            return HttpUtility.UrlEncode(input);
        }
    }
}
