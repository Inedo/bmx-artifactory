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
    internal sealed class DeleteItemActionEditor : ArtifactoryActionBaseEditor 
    {
        private ValidatingTextBox txtItemName;

        public DeleteItemActionEditor () 
        {
            this.extensionInstance = new DeleteItemAction();
        }

        public override void BindToForm(ActionBase extension)
        {
            var action = (DeleteItemAction) extension;
            this.EnsureChildControls();
            base.BindToForm(extension);
            this.txtItemName.Text = action.ItemName;
        }

        public override ActionBase CreateFromForm()
        {
            this.EnsureChildControls();

            return PopulateProperties(new DeleteItemAction() { ItemName = this.txtItemName.Text }
                );
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.txtItemName = new ValidatingTextBox() { Width = 300 };
            this.Controls.Add(new FormFieldGroup("Item","Item Information, if item is a directory then all contents will be recursively deleted.",true,
                new StandardFormField("Item Name:",txtItemName)
                )
            );
        }
    }
}
