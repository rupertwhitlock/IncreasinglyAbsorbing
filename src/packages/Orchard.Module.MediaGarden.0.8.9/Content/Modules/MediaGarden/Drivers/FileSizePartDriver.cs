using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using MediaGarden.Models;

namespace MediaGarden.Drivers
{
    [UsedImplicitly]
    public class FileSizePartDriver : ContentPartDriver<FileSizePart>
    {
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts/FileSize";

        public Localizer T { get; set; }

        public FileSizePartDriver(INotifier notifier)
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Display(FileSizePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_FileSize",
                () => shapeHelper.Parts_FileSize(ContentItem: part.ContentItem));
        }

        protected override DriverResult Editor(FileSizePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_FileSize",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(FileSizePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
            }
            else
            {
                _notifier.Error(T("Error updating File Size!"));
            }
            return Editor(part, shapeHelper);
        }

    }
}