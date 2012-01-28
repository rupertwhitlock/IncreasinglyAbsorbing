using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Contrib.HttpMp3Track
{
    public class Migrations : DataMigrationImpl
    {
        public int Create() {
            SchemaBuilder.CreateTable("HttpMp3TrackPartRecord",
                table => table
                .ContentPartRecord()
                .Column("Url", DbType.String));

            ContentDefinitionManager.AlterPartDefinition("HttpMp3TrackPart",
                cfg => cfg
                    .Attachable());

            return 1;
        }
    }
}
