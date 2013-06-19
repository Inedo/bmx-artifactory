using Inedo.BuildMaster;
using Inedo.BuildMaster.Extensibility.Configurers.Extension;
using Inedo.BuildMaster.Web;

[assembly: ExtensionConfigurer(typeof(Inedo.BuildMasterExtensions.Artifactory.ArtifactoryConfigurer))]

namespace Inedo.BuildMasterExtensions.Artifactory
{
    public class ArtifactoryConfigurer : ExtensionConfigurerBase 
    {
        [Persistent]
        public string Server { get; set; }

        [Persistent]
        public Authentication Credentials { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
