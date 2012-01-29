using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Rupert.GetMeThatRecord
{
    public class Migrations : DataMigrationImpl
    {
        public int Create() {
            SchemaBuilder.CreateTable("ThatRecordPartRecord",
                table => table
                .ContentPartRecord()
                .Column("RecordUrl", DbType.String));

            ContentDefinitionManager.AlterPartDefinition("ThatRecordPart",
                cfg => cfg.Attachable());

            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.CreateTable("TrackRecord",
                table => table
                .Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
                .Column("TrackUrl", DbType.String));

            SchemaBuilder.CreateTable("ThatRecordTrackRecord",
                table => table
                .Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
                .Column("ThatRecordPartRecord_Id", DbType.Int32)
                .Column("TrackRecord_Id", DbType.Int32));

            return 2;
        }
    }
}
