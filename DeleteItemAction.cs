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
        "Delete Item",
        "Recursively deletes an item (artifact or directory) from a repository.",
        "Artifactory")]
    [CustomEditor(typeof(DeleteItemActionEditor))]
    public class DeleteItemAction : ArtifactoryActionBase 
    {
        [Persistent]
        public string ItemName {get;set;}

        public override string ToString()
        {
            return string.Format("Delete {0} from repository {1}", this.ItemName, this.RepositoryKey);
        }

        public DeleteItemAction()
        {
            this.UsesRepositoryKey = true;
            this.ForceAuthorizationHeader = true;
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
            var resp = Request(RequestType.Delete ,null,url,this.RepositoryKey, this.ItemName);
            if (HttpStatusCode.NoContent != resp.StatusCode)
            {
                LogError("Error deleting {0} in repository {1}. Error code is {2}", this.ItemName, this.RepositoryKey, resp.StatusCode);
                return null;
            }
            return "OK";
        }
    }
}
