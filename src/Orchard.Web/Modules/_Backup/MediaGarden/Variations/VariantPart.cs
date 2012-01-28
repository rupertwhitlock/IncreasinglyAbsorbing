using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace MediaGarden.Variations
{
    public class VariantPart : ContentPart<VariantPartRecord>
    {

        public ContentItemRecord VariantOf
        {
            get { return Record.VariantOf; }
            set
            {
                Record.VariantOf = value;
                
            }
        }

    }
}