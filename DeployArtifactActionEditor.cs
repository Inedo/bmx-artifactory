using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web.Controls;
using Inedo.BuildMaster.Web.Controls.Extensions;
using Inedo.Web.Controls;

namespace Inedo.BuildMasterExtensions.Artifactory
{
    internal sealed class DeployArtifactActionEditor : ArtifactoryActionBaseEditor 
    {
        private ValidatingTextBox txtDirectoryName;
        private ValidatingTextBox txtProperties;
        private SourceControlFileFolderPicker ffpArtifactFile;

        public DeployArtifactActionEditor () 
        {
            this.extensionInstance = new DeployArtifactAction();
        }

        public override void BindToForm(ActionBase extension)
        {
            var action = (DeployArtifactAction) extension;
            this.EnsureChildControls();
            base.BindToForm(extension);
            this.txtDirectoryName.Text = action.DirectoryName;
            this.txtProperties.Text = action.Properties;
            this.ffpArtifactFile.Text = action.FileName;
        }

        public override ActionBase CreateFromForm()
        {
            this.EnsureChildControls();

            return PopulateProperties(new DeployArtifactAction() { 
                DirectoryName = this.txtDirectoryName.Text,
                Properties = this.txtProperties.Text,
                FileName = this.ffpArtifactFile.Text,
                FileServerID = 1
                }
                );
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.txtDirectoryName = new ValidatingTextBox() { Width = 300 };
            this.txtProperties = new ValidatingTextBox() {Width = 300, TextMode = TextBoxMode.MultiLine};
            ffpArtifactFile = new SourceControlFileFolderPicker() { ServerId = 1 };
            this.Controls.Add(new FormFieldGroup("Directory", "Directory Information", false,
                new StandardFormField("Directory Name:",txtDirectoryName)
                )
            );
            this.Controls.Add(new FormFieldGroup("Artifact", "Artifact information", false,
                new StandardFormField("Artifact File Disk Location:", ffpArtifactFile)
                )
            );
            this.Controls.Add(new FormFieldGroup("Properties",
                "Additional properties for artifacts. Enter each as a name=value pair on a line", true,
                new StandardFormField("Properties:", txtProperties)
                )
            );
        }
    }
}
