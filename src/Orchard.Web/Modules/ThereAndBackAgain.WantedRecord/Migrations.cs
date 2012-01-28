using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ThereAndBackAgain.WantedRecord
{
    public class Migrations : DataMigrationImpl
    {
        public int Create() {

            SchemaBuilder.CreateTable("WantedRecordPartRecord",
                table => table
                .ContentPartRecord());

            ContentDefinitionManager.AlterPartDefinition("WantedRecordPart",
                cfg => cfg
                .WithField("MediaPickerField")
                .Attachable());

            ContentDefinitionManager.AlterTypeDefinition("Wanted Record",
                cfg => cfg
                    .WithPart("CommonPart", p => p.WithSetting("DateEditorSettings.ShowDateEditor", "true"))
                    .WithPart("PublishLaterPart")
                    .WithPart("RoutePart")
                    .WithPart("BodyPart")
                    .WithPart("WantedRecordPart")
                    .Creatable()
                    .Draftable());

            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.AlterTable("WantedRecordPartRecord",
                table => table
                .AddColumn("RecordArtists", DbType.String));

            return 2;
        }

        public int UpdateFrom2() {
            ContentDefinitionManager.AlterPartDefinition("WantedPartRecord",
                cfg => cfg
                .RemoveField("MediaPickerField"));

            ContentDefinitionManager.AlterPartDefinition("WantedPartRecord",
                cfg => cfg
                .WithField("Media", action => action.OfType("MediaPickerField")));

            return 3;
        }

        public int UpdateFrom3() {
            ContentDefinitionManager.DeletePartDefinition("WantedPartRecord");
            ContentDefinitionManager.AlterPartDefinition("WantedRecordPart",
                cfg => cfg.RemoveField("MediaPickerField"));
            ContentDefinitionManager.AlterPartDefinition("WantedRecordPart",
                cfg => cfg.WithField("Media", field => field.OfType("MediaPickerField")));

            return 4;
        }

        public int UpdateFrom4() {
            ContentDefinitionManager.AlterTypeDefinition("Wanted Record",
                cfg => cfg
                .WithPart("MediaGalleryPart"));

            return 5;
        }
    }
}
