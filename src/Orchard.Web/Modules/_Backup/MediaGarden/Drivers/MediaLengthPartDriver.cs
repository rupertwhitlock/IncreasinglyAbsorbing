using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using MediaGarden.Models;

namespace MediaGarden.Drivers
{
    [UsedImplicitly]
    public class MediaLengthPartDriver : ContentPartDriver<MediaLengthPart>
    {
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts/MediaLengthPart";

        public Localizer T { get; set; }

        public MediaLengthPartDriver(INotifier notifier)
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Display(MediaLengthPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_MediaLengthPart",
                () => shapeHelper.Parts_MediaLengthPart(ContentItem: part.ContentItem));
        }

        protected override DriverResult Editor(MediaLengthPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_MediaLengthPart",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(MediaLengthPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
            }
            else
            {
                _notifier.Error(T("Error during MediaLengthPart update!"));
            }
            return Editor(part, shapeHelper);
        }

    }
}