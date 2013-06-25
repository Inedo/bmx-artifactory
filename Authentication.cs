using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inedo.BuildMasterExtensions.Artifactory
{
    [Serializable]
    public class Authentication
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
