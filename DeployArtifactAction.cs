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
    "Deploy Artifact",
    "Deploys an atifact to a repository.",
    "Artifactory")]
    [CustomEditor(typeof(DeployArtifactActionEditor))]
    public class DeployArtifactAction : ArtifactoryActionBase 
    {
        [Persistent]
        public int FileServerID { get; set; }

        [Persistent]
        public string FileName { get; set; }

        [Persistent]
        public string DirectoryName { get; set; }

        [Persistent]
        public string Properties { get; set; }

        public DeployArtifactAction()
        {
            this.UsesRepositoryKey = true;
        }

        public override string ToString()
        {
            return string.Format("Deploy {0} artifact to the {1} directory of {2}", this.FileName, this.DirectoryName, this.RepositoryKey);
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
            if (!File.Exists(fname))
            {
                LogError("Artifactory Deploy Artifact unable to find file: {0}", this.FileName);
                return null;
            }
            string onlyFileName = Path.GetFileName(fname);
            StringBuilder url = new StringBuilder();
            url.Append(this.Server.EndsWith("/") ? this.Server : this.Server + "/" +
                "{0}/{1}/{2}");
            if (!string.IsNullOrEmpty(this.Properties))
            {
                foreach (var item in this.Properties.ParseNameValue())
                {
                    url.AppendFormat(";{0}={1}", item.Key, item.Value);
                }
            }
            var uri = new Uri(string.Format(url.ToString(), this.RepositoryKey, this.DirectoryName, onlyFileName));
            var req = new WebClient();
            req.Credentials = new NetworkCredential(this.Credentials.Username, this.Credentials.Password);
            try
            {
                var resp = req.UploadFile(uri, "PUT", fname);
                return System.Text.Encoding.ASCII.GetString(resp);
            }
            catch (Exception ex)
            {
                LogError("Error deploying artifact {0} to {1} in repository {2}. Error: {3}", this.FileName, this.DirectoryName, this.RepositoryKey, ex.ToString());
            }
            return null;
        }        
    }
}
