using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace MediaGarden.Defaults
{
    /// <summary>
    /// Creates the BinaryMedia content type
    /// </summary>
    [OrchardFeature("MediaGarden.Defaults")]
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            ContentDefinitionManager.AlterTypeDefinition("Binary", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("RoutePart")
                .WithPart("MediaPart")
                .WithPart("FileSizePart")
                .WithSetting("Stereotype","Content")
                .WithSetting("MediaStereotype","Binary")
                 .Creatable()
                 .Draftable()
                );

            return 1;
        }

    }
}