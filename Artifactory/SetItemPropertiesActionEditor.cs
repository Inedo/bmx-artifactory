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
    internal sealed class SetItemPropertiesActionEditor : ArtifactoryActionBaseEditor 
    {
        private ValidatingTextBox txtItemName;
        private ValidatingTextBox txtProperties;

        public SetItemPropertiesActionEditor () 
        {
            this.extensionInstance = new SetItemPropertiesAction();
        }

        public override void BindToForm(ActionBase extension)
        {
            var action = (SetItemPropertiesAction) extension;
            this.EnsureChildControls();
            base.BindToForm(extension);
            this.txtItemName.Text = action.ItemName;
            this.txtProperties.Text = action.Properties;
        }

        public override ActionBase CreateFromForm()
        {
            this.EnsureChildControls();

            return PopulateProperties(new SetItemPropertiesAction() { ItemName= this.txtItemName.Text, Properties = this.txtProperties.Text }
                );
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.txtItemName = new ValidatingTextBox() { Width = 300 };
            this.txtProperties = new ValidatingTextBox() { Width = 300, TextMode = TextBoxMode.MultiLine };
            this.Controls.Add(new FormFieldGroup("Information","Item Information. If item is a directory then the properties will be set recursivly on all items.",true,
                new StandardFormField("Item Name:",txtItemName),
                new StandardFormField("Properties:",txtProperties )
                )
            );
        }
    }
}
