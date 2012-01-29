using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace Rupert.GetMeThatRecord.Settings
{
    public class ThatRecordPartSettingsHooks : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if(definition.PartDefinition.Name != "ThatRecordPart") {
                yield break;
            }

            var model = definition.Settings.GetModel<ThatRecordPartSettings>();

            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(Orchard.ContentManagement.MetaData.Builders.ContentTypePartDefinitionBuilder builder, Orchard.ContentManagement.IUpdateModel updateModel)
        {
            if(builder.Name != "ThatRecordPart") {
                yield break;
            }

            var model = new ThatRecordPartSettings();

            updateModel.TryUpdateModel(model, "ThatRecordPartSettings", null, null);

            builder.WithSetting("ThatRecordPartSettings.XmlFilePathForRecordWebsiteUrls", model.XmlFilePathForRecordWebsiteUrls);

            yield return DefinitionTemplate(model);
        }
    }

    
}
