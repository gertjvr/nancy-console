using Nancy;

namespace Console.Features
{
    public class Index : NancyModule
    {
        public Index()
        {
            Get["/"] = _ => "Hello World";
        }
    }
}