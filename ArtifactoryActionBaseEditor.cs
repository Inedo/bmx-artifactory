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
    public abstract class ArtifactoryActionBaseEditor : ActionEditorBase
    {
        protected ValidatingTextBox txtUserName;
        protected ValidatingTextBox txtPassword;
        protected ValidatingTextBox txtServer;
        protected ValidatingTextBox txtRepository;

        protected ArtifactoryActionBase extensionInstance;

        public ArtifactoryActionBaseEditor()
        {
            this.txtUserName = new ValidatingTextBox() { Width = 300};
            this.txtPassword = new ValidatingTextBox() { Width = 300 };
            this.txtServer = new ValidatingTextBox() { Width = 300 };
            this.txtRepository = new ValidatingTextBox() { Width = 300 };
        }

        protected virtual ArtifactoryActionBase PopulateProperties(ArtifactoryActionBase Value)
        {
            if (Value.UsesRepositoryKey)
                Value.RepositoryKey = txtRepository.Text;
            if (!string.IsNullOrEmpty(this.txtUserName.Text))
            {
                Value.ActionUsername = this.txtUserName.Text;
                Value.ActionPassword = this.txtPassword.Text;
            }
            else
            {
                Value.ActionUsername = null;
                Value.ActionPassword = null;
            }
            if (!string.IsNullOrEmpty(this.txtServer.Text))
                Value.ActionServer = this.txtServer.Text;
            return Value;
        }

        public override void BindToForm(ActionBase extension)
        {
            this.EnsureChildControls();

            var action = (ArtifactoryActionBase)extension;
            this.txtServer.Text = action.ActionServer;
            this.txtRepository.Text = action.RepositoryKey;
            if (!string.IsNullOrEmpty(action.ActionUsername))
            {
                this.txtUserName.Text = action.ActionUsername;
                this.txtPassword.Text = action.ActionPassword;
            }
        }

        protected override void CreateChildControls()
        {
            AddActionAuthentication();
            AddServerInformation();
            AddRepositoryInformation();
        }

        private void AddActionAuthentication()
        {
            this.Controls.Add(new FormFieldGroup("Custom Authentication",
                "Authentication used for this action. If not populated then the credentials defined for the Artifactory extension will be used.",
                false,
                new StandardFormField("Username:", txtUserName),
                new StandardFormField("Password:",txtPassword)
                )
            );
        }

        private void AddServerInformation()
        {
            var ff = new FormFieldGroup("Server", "Server used for this action. If not populated then the server defined for the Artifactory extension will be used.", false);
            ff.FormFields.Add(new StandardFormField("Server:",txtServer));
            this.Controls.Add(ff);
        }

        private void AddRepositoryInformation()
        {
            if (extensionInstance.UsesRepositoryKey)
            {
                this.Controls.Add(new FormFieldGroup("Repository", "The repository used as the source for this action.",false,
                    new StandardFormField("Repository:", txtRepository))
                 );
            }
        }
    }
}
