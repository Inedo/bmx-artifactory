using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Text;
using System.IO;
using System.Linq;

using Inedo.BuildMaster;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Extensibility.Agents;

namespace Inedo.BuildMasterExtensions.Artifactory
{
    public abstract class ArtifactoryActionBase : RemoteActionBase
    {
        internal protected enum RequestType { Get, Post, Delete, Put };

        [Persistent]
        public string ActionUsername { get; set; }

        [Persistent]
        public string ActionPassword { get; set; }

        [Persistent]
        public string ActionServer { get; set; }

        [Persistent]
        public string RepositoryKey { get; set; }

        public bool UsesRepositoryKey { get; set; }

        public bool ForceAuthorizationHeader { get; set; }

        protected ArtifactoryConfigurer Configurer
        {
            get
            {
                return this.GetExtensionConfigurer() as ArtifactoryConfigurer;

                //if (null != TestConfigurer)
                //    return TestConfigurer;
                //else
                //    return Util.Actions.GetConfigurer(GetType()) as ArtifactoryConfigurer;
            }
        }

        protected Authentication Credentials
        {
            get
            {
                // use local first
                if (null != ActionUsername )
                    return new Authentication() {Username = ActionUsername, Password = ActionPassword } ;
                return new Authentication() { Username = Configurer.Username, Password = Configurer.Password };
            }
        }

        protected string Server
        {
            get
            {
                // use local first
                if (null != ActionServer)
                    return ActionServer;
                return Configurer.Server;
            }
        }

        protected string ResolveDirectory(string FilePath)
        {
            using (var sourceAgent2 = Util.Agents.CreateLocalAgent())
            {
                var sourceAgent = sourceAgent2.GetService<IFileOperationsExecuter>();

                char srcSeparator = sourceAgent.GetDirectorySeparator();
                var srcPath = sourceAgent.GetWorkingDirectory(this.Context.ApplicationId, this.Context.DeployableId ?? 0, FilePath);

                LogInformation("Source Path: " + srcPath);
                return srcPath;
            }
        }

        internal Response Request(RequestType RequestType, string Payload, string UriFormat, params object[] args)
        {
            Response retval = new Response();
            var uri = new Uri(string.Format(UriFormat, args));
            var req = (HttpWebRequest)HttpWebRequest.Create(uri);
            req.Credentials = new NetworkCredential(this.Credentials.Username, this.Credentials.Password);
            switch (RequestType)
            {
                case ArtifactoryActionBase.RequestType.Get:
                    req.Method = "GET";
                    break;
                case ArtifactoryActionBase.RequestType.Post:
                    req.Method = "POST";
                    break;
                case ArtifactoryActionBase.RequestType.Delete:
                    req.Method = "DELETE";
                    break;
                case ArtifactoryActionBase.RequestType.Put:
                    req.Method = "PUT";
                    break;
                default:
                    req.Method = "GET";
                    break;
            }
            if (this.ForceAuthorizationHeader)
            {
                string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(this.Credentials.Username + ":" + this.Credentials.Password));
                req.Headers.Add("Authorization", "Basic " + credentials);
            }
            req.ContentType = "application/json";
            if (!string.IsNullOrEmpty(Payload))
            {
                var buffer = Encoding.UTF8.GetBytes(Payload);
                req.ContentLength = buffer.Length;
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();
            }
            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                resp = (HttpWebResponse)ex.Response;
            }
            retval.StatusCode = resp.StatusCode;
            retval.Headers = resp.Headers;
            if (resp.ContentLength > 0)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                retval.Document = reader.ReadToEnd();
            }
            return retval;
        }


    }
}
