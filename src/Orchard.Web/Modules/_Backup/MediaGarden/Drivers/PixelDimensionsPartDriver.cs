using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using MediaGarden.Models;

namespace MediaGarden.Drivers
{
    [UsedImplicitly]
    public class PixelDimensionsPartDriver : ContentPartDriver<PixelDimensionsPart>
    {
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts.PixelDimensions.Edit";

        public Localizer T { get; set; }

        public PixelDimensionsPartDriver(INotifier notifier)
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Display(PixelDimensionsPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_PixelDimensions",
                () => shapeHelper.Parts_PixelDimensions(ContentItem: part.ContentItem));
        }

        protected override DriverResult Editor(PixelDimensionsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_PixelDimensions_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(PixelDimensionsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            // Not required
            /*
            {
            }
            else
            {
                _notifier.Error(T("Error updating Pixel Dimensions!"));
            }*/
            return Editor(part, shapeHelper);
        }

    }
}