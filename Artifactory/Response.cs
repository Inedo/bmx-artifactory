using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace Inedo.BuildMasterExtensions.Artifactory
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Document { get; set; }

        public WebHeaderCollection Headers { get; set; }
    }
}
