using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace MediaGarden {
    public class Migrations : DataMigrationImpl {

        public int Create() {
			// Creating table MediaSourceItemRecord
			SchemaBuilder.CreateTable("MediaSourceRecord", table => table
				.Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
				.Column("Location", DbType.String)
                .Column("MediaStereotype", DbType.String, col => col.WithLength(32))
                .Column("LastModified", DbType.DateTime)
                .Column("Metadata", DbType.String, column => column.Unlimited())
                .Column("SessionRecord_Id", DbType.Int32)
            );
			SchemaBuilder.CreateTable("MediaSessionRecord", table => table
				.Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
				.Column("Query", DbType.String)
                );
             
			// Creating table MediaPartRecord
			SchemaBuilder.CreateTable("MediaPartRecord", table => table
				.ContentPartRecord()
				.Column("Url", DbType.String)
				.Column("ViewerName", DbType.String)
				.Column("FormatName", DbType.String)
				.Column("Source_id", DbType.Int32)
			);

			// Creating table FileSizePartRecord
			SchemaBuilder.CreateTable("FileSizePartRecord", table => table
				.ContentPartRecord()
				.Column("Size", DbType.Int64)
			);

			// Creating table MediaLengthPartRecord
			SchemaBuilder.CreateTable("MediaLengthPartRecord", table => table
				.ContentPartRecord()
				.Column("Length", DbType.Decimal)
			);

			// Creating table PixelDimensionsPartRecord
			SchemaBuilder.CreateTable("PixelDimensionsPartRecord", table => table
				.ContentPartRecord()
				.Column("SizeX", DbType.Int32)
				.Column("SizeY", DbType.Int32)
			);

            return 1;
        }

        public int UpdateFrom1()
        {
            // Add FormatName column to MediaSource
            SchemaBuilder.AlterTable("MediaSourceRecord",table=>table
                .AddColumn<String>("FormatName", column=>column.WithLength(64)));
            return 2;
        }

        public int UpdateFrom2()
        {
            // Add FormatName column to MediaSource
            SchemaBuilder.AlterTable("MediaSourceRecord", table => table
                .AddColumn<String>("ViewerName", column => column.WithLength(64)));
            return 3;
        }

    }
}