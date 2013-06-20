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
        "Set Item Properties",
        "Recursively sets properties on items in the specified location.",
        "Artifactory")]
    [CustomEditor(typeof(SetItemPropertiesActionEditor))]
    public class SetItemPropertiesAction : ArtifactoryActionBase 
    {
        [Persistent]
        public string ItemName {get;set;}

        [Persistent]
        public string Properties { get; set; }

        public override string ToString()
        {
            return string.Format("Set properties for {0} in repository {1}", this.ItemName, this.RepositoryKey);
        }

        public SetItemPropertiesAction()
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

            StringBuilder url = new StringBuilder();
            url.Append(this.Server.EndsWith("/") ? this.Server : this.Server + "/" +
                "api/storage/{0}/{1}");
            if(!string.IsNullOrEmpty(this.Properties))
            {
                url.Append("?properties=");
                foreach(var p in this.Properties.ParseNameValue())
                {
                    url.AppendFormat("{0}={1}|",p.Key, p.Value);
                }
                url.Remove(url.Length - 1, 1);
            }
            var resp = Request(RequestType.Put,null,url.ToString(),this.RepositoryKey, this.ItemName);
            if (HttpStatusCode.NoContent  != resp.StatusCode)
            {
                LogError("Error setting properties for {0} in repository {1}. Error code is {2}", this.ItemName, this.RepositoryKey, resp.StatusCode);
                return null;
            }
            return "OK";
        }
    }
}
