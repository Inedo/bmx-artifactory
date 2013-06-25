using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

using Inedo.BuildMaster;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Extensibility.Agents;
using Inedo.BuildMaster.Web;

namespace Inedo.BuildMasterExtensions.Artifactory
{
    [ActionProperties(
    "Retrieve Artifact",
    "Retrieves an atifact from a repository.",
    "Artifactory")]
    [CustomEditor(typeof(RetrieveArtifactActionEditor))]
    public class RetrieveArtifactAction : ArtifactoryActionBase 
    {
        [Persistent]
        public int FileServerID { get; set; }

        [Persistent]
        public string FileName { get; set; }

        [Persistent]
        public string ItemName { get; set; }

        [Persistent]
        public string Properties { get; set; }

        public RetrieveArtifactAction()
        {
            this.UsesRepositoryKey = true;
        }

        public override string ToString()
        {
            return string.Format("Retrieve the {0} artifact from the {1} repository to {2}", this.ItemName, this.RepositoryKey, this.FileName);
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
            string fname = this.ResolveDirectory(this.FileName);
            string onlyFileName = Path.GetFileName(fname);
            StringBuilder url = new StringBuilder();
            url.Append(this.Server.EndsWith("/") ? this.Server : this.Server + "/" +
                "{0}/{1}");
            if (!string.IsNullOrEmpty(this.Properties))
            {
                foreach (var item in this.Properties.ParseNameValue())
                {
                    url.AppendFormat(";{0}={1}", item.Key, item.Value);
                }
            }
            var uri = new Uri(string.Format(url.ToString(), this.RepositoryKey, this.ItemName));
            var req = new WebClient();
            req.Credentials = new NetworkCredential(this.Credentials.Username, this.Credentials.Password);
            try
            {
                req.DownloadFile(uri, fname);
                return "OK";
            }
            catch (Exception ex)
            {
                LogError("Error retrieving the {0} artifact in repository {1}. Error: {2}", this.ItemName, this.RepositoryKey, ex.ToString());
            }
            return null;
        }        
    }
}
