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
    internal sealed class MoveItemActionEditor : ArtifactoryActionBaseEditor 
    {
        private ValidatingTextBox txtSourceItem;
        private ValidatingTextBox txtDestinationRepository;
        private ValidatingTextBox txtDestionationItemName;
        private CheckBox chkSuppressCrossLayoutTranslation;

        public MoveItemActionEditor () 
        {
            this.extensionInstance = new MoveItemAction();
        }

        public override void BindToForm(ActionBase extension)
        {
            var action = (MoveItemAction) extension;
            this.EnsureChildControls();
            base.BindToForm(extension);
            this.txtSourceItem.Text = action.SourceItemName;
            this.txtDestinationRepository.Text = action.DestinationRepository;
            this.txtDestionationItemName.Text = action.DestinationItemName;
            this.chkSuppressCrossLayoutTranslation.Checked = action.SuppressCrossLayoutTranslation;
        }

        public override ActionBase CreateFromForm()
        {
            this.EnsureChildControls();

            return PopulateProperties(new MoveItemAction() {
                SourceItemName = this.txtSourceItem.Text,
                DestinationRepository = this.txtDestinationRepository.Text,
                DestinationItemName = this.txtDestionationItemName.Text,
                SuppressCrossLayoutTranslation = this.chkSuppressCrossLayoutTranslation.Checked 
                }
            );
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.txtSourceItem = new ValidatingTextBox() { Width = 300 };
            this.txtDestinationRepository = new ValidatingTextBox() { Width = 300 };
            this.txtDestionationItemName = new ValidatingTextBox() { Width = 300 };
            this.chkSuppressCrossLayoutTranslation = new CheckBox() { Width = 300 };
            this.Controls.Add(new FormFieldGroup("Source", "Source Item Information", false,
                new StandardFormField("Source Name:",txtSourceItem)
                )
            );
            this.Controls.Add(new FormFieldGroup("Destination","Destination Information:",false,
                new StandardFormField("Destination Repository:",txtDestinationRepository),
                new StandardFormField("Destination Name:",txtDestionationItemName)
                )
            );
            this.Controls.Add(new FormFieldGroup("Options", "Other options", true,
                new StandardFormField("Suppress Cross Layout Translation", chkSuppressCrossLayoutTranslation)
                )
            );
        }
    }
}
