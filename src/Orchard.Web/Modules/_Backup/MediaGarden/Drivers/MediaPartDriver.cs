using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using MediaGarden.Models;
using Orchard.Core.Routable.Models;

namespace MediaGarden.Drivers
{
    [UsedImplicitly]
    public class MediaPartDriver : ContentPartDriver<MediaPart>
    {
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts.Media";

        public Localizer T { get; set; }

        public MediaPartDriver(INotifier notifier)
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;
        }
        protected override string Prefix
        {
            get
            {
                return "Media";
            }
        }
        protected override DriverResult Display(MediaPart part, string displayType, dynamic shapeHelper)
        {
            // Return media part shapes; this will themselves build a proper Media shape
            // TODO: Should perhaps be displaying slightly differently info in the summaries, altho individual parts can handle the obvious metadata
            return Combined(
                ContentShape("Parts_Media",
                    () => shapeHelper.Parts_Media(MediaItem: part)),
                ContentShape("Parts_Media_Embed",
                    () => shapeHelper.Parts_Media_Embed(MediaItem: part)),
                ContentShape("Parts_Media_Summary",
                    () => shapeHelper.Parts_Media_Summary(MediaItem: part)),
                ContentShape("Parts_Media_SummaryAdmin",
                    () => shapeHelper.Parts_Media_SummaryAdmin(MediaItem: part)));
        }

        protected override DriverResult Editor(MediaPart part, dynamic shapeHelper)
        {
            return new CombinedResult(
                new[]{
                ContentShape("Parts_Media_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName+".Edit", Model: part, Prefix: Prefix))
                });
        }

        protected override DriverResult Editor(MediaPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
            }
            else
            {
                _notifier.Error(T("Error updating Media details!"));
            }
            return Editor(part, shapeHelper);
        }

    }
}