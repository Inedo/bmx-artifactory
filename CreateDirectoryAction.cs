using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

using Inedo.BuildMaster;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web;

namespace Inedo.BuildMasterExtensions.Artifactory
{
    [ActionProperties(
        "Create Directory",
        "Creates a new directory in the specified location.",
        "Artifactory")]
    [CustomEditor(typeof(CreateDirectoryActionEditor))]
    public class CreateDirectoryAction : ArtifactoryActionBase 
    {
        [Persistent]
        public string DirectoryName {get;set;}

        public override string ToString()
        {
            return string.Format("Create directory named {0} in repository {1}", this.DirectoryName, this.RepositoryKey);
        }

        public CreateDirectoryAction()
        {
            this.UsesRepositoryKey = true;
        }

        internal string Test()
        {
            return ProcessRemoteCommand(null, null);
        }

        protected override void Execute()
        {
            this.ExecuteRemoteCommand(null);
        }

        protected override string ProcessRemoteCommand(string name, string[] args)
        {
            return MakeRequest();
        }

        internal string MakeRequest()
        {
            string url = this.Server.EndsWith("/") ? this.Server : this.Server + "/" + 
                "{0}/{1}";
            var resp = Request(RequestType.Put,null,url,this.RepositoryKey, this.DirectoryName.EndsWith("/") ? this.DirectoryName : this.DirectoryName + "/");
            if (HttpStatusCode.Created != resp.StatusCode)
            {
                LogError("Error creating Artifactory directory {0} in repository {1}. Error code is {2}", this.DirectoryName, this.RepositoryKey, resp.StatusCode);
                return null;
            }
            return "OK";
        }
    }
}
