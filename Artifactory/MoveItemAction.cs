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
        "Move Item",
        "Moves an item to a new location.",
        "Artifactory")]
    [CustomEditor(typeof(MoveItemActionEditor))]
    public class MoveItemAction : ArtifactoryActionBase 
    {
        [Persistent]
        public string SourceItemName { get; set; }

        [Persistent]
        public string DestinationRepository { get; set; }

        [Persistent]
        public string DestinationItemName { get; set; }

        [Persistent]
        public bool SuppressCrossLayoutTranslation { get; set; }

        public override string ToString()
        {
            return string.Format("Move {0} in repository {1} to {2} in repository {3}", this.SourceItemName, this.RepositoryKey, 
                this.DestinationItemName, this.DestinationRepository);
        }

        public MoveItemAction()
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
                "api/move/{0}/{1}?to={2}/{3}";
            if (this.SuppressCrossLayoutTranslation)
            {
                url += "&suppressLayouts=1";
            }
            var resp = Request(RequestType.Post,null,url,this.RepositoryKey, this.SourceItemName, 
                this.DestinationRepository, this.DestinationItemName);
            if (HttpStatusCode.OK!= resp.StatusCode)
            {
                LogError("Error moving {0} in repository {1} to {2} in repository {3}. Error code is {4}", this.SourceItemName, this.RepositoryKey, 
                    this.DestinationItemName, this.DestinationRepository, resp.StatusCode);
                return null;
            }
            return "OK";
        }
    }
}
