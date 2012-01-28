using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.jPlayer.Models;

namespace Orchard.jPlayer {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("MediaGalleryRecord", table => table
                    .ContentPartRecord()
                    .Column("MediaGalleryName", DbType.String)
                    .Column("SelectedType", DbType.Byte)
                    .Column<bool>("AutoPlay", column => column.WithDefault(true))
                );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(MediaGalleryPart).Name, cfg => cfg.Attachable());

            SchemaBuilder.CreateTable("MediaGallerySettingsRecord", table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("MediaGalleryName", column => column.WithLength(255))
                );

            SchemaBuilder.CreateTable("MediaGalleryMediaSettingsRecord", table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name", column => column.WithLength(255))
                    .Column<int>("MediaGallerySettingsRecord_Id")
                    .Column<string>("Title")
                    .Column("Position", DbType.Int32)
                );

            ContentDefinitionManager.AlterTypeDefinition("MediaGalleryWidget", cfg => cfg
            .WithPart("MediaGalleryPart")
            .WithPart("WidgetPart")
            .WithPart("CommonPart")

            .WithSetting("Stereotype", "Widget"));

            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.AlterTable("MediaGalleryRecord", table => {
                table.AddColumn<byte>("MediaType");
                table.DropColumn("SelectedType");
            });

            return 2;
        }
    }
}