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
    internal sealed class CreateDirectoryActionEditor : ArtifactoryActionBaseEditor 
    {
        private ValidatingTextBox txtDirectoryName;

        public CreateDirectoryActionEditor () 
        {
            this.extensionInstance = new CreateDirectoryAction();
        }

        public override void BindToForm(ActionBase extension)
        {
            var action = (CreateDirectoryAction) extension;
            this.EnsureChildControls();
            base.BindToForm(extension);
            this.txtDirectoryName.Text = action.DirectoryName;
        }

        public override ActionBase CreateFromForm()
        {
            this.EnsureChildControls();

            return PopulateProperties(new CreateDirectoryAction() { DirectoryName = this.txtDirectoryName.Text }
                );
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.txtDirectoryName = new ValidatingTextBox() { Width = 300 };
            this.Controls.Add(new FormFieldGroup("Directory","Directory Information",true,
                new StandardFormField("Directory Name:",txtDirectoryName)
                )
            );
        }
    }
}
