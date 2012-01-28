using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace MediaGarden.Variations
{
    public class VariantPartRecord : ContentPartRecord
    {
        public Orchard.ContentManagement.Records.ContentItemRecord VariantOf { get; set; }
    }
}
