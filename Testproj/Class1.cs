using Microsoft.AspNetCore.Mvc;

namespace Testproj
{
    public class Class1Controller : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator) => _linkGenerator = linkGenerator;

    }
}
