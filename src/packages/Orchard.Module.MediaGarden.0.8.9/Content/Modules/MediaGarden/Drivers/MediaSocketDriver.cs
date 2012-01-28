using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Downplay.Origami.Services;
using MediaGarden.Services;
using Orchard.ContentManagement.MetaData;

namespace MediaGarden.Drivers
{
    public class MediaSocketDriver : ModelDriver<SocketEventContext>
    {
        private readonly Lazy<IMediaGardenService> _mediaGarden;
        private readonly Lazy<IContentDefinitionManager> _contentDefinitionManager;

        public MediaSocketDriver(
            Lazy<IMediaGardenService> mediaGarden,
            Lazy<IContentDefinitionManager> contentDefinitionManager
            ) {
            _mediaGarden = mediaGarden;
            _contentDefinitionManager = contentDefinitionManager;
        }

        protected override string Prefix
        {
            get { return "Media"; }
        }

        protected override ModelDriverResult Display(SocketEventContext model, dynamic shapeHelper, ModelDisplayShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Editor(SocketEventContext model, dynamic shapeHelper, ModelEditorShapeContext context)
        {
            return Update(model, shapeHelper, null, context);
        }

        protected override ModelDriverResult Update(SocketEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelEditorShapeContext context)
        {
/*            var rightContentTypes = model.ConnectorSettings.ListAllowedContentRight()
                .Select(c=>_contentDefinitionManager.Value.GetTypeDefinition(c))
                .Where(d=>d.Settings.ContainsKey("MediaStereotype") && d.Parts.Any(p=>p.PartDefinition.Name=="MediaPart"));
            var stereotypes = rightContentTypes
                .Select(d=>d.Settings["MediaStereotype"]).Distinct()
                .Where(d=>!String.IsNullOrEmpty(d));

            if (stereotypes.Any()) {
                dynamic build;
                if (updater!=null) {
                    build = _mediaGarden.Value.UpdateSources(
                }
                else {
                    build = _mediaGarden.Value.BuildSources(
                }
            }
            */
            return null;
        }

    }
}