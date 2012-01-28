using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contrib.HttpMp3Track.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Contrib.HttpMp3Track.Drivers
{
    public class HttpMp3TrackDriver : ContentPartDriver<HttpMp3TrackPart>
    {
        protected override DriverResult Display(HttpMp3TrackPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_HttpMp3Track",
                                () => shapeHelper.Parts_HttpMp3Track(Url: part.Url));
        }

        protected override DriverResult Editor(HttpMp3TrackPart part, dynamic shapeHelper) {
            return ContentShape("Parts_HttpMp3Track_Edit",
                                () => shapeHelper.EditorTemplate(
                                    TemplateName: "Parts/HttpMp3Track",
                                    Model: part,
                                    Prefix: Prefix));
        }

        protected override DriverResult Editor(HttpMp3TrackPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}
