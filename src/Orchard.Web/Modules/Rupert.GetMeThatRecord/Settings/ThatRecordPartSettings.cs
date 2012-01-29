using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Rupert.GetMeThatRecord.Settings
{
    public class ThatRecordPartSettings
    {
        [Required]
        public string XmlFilePathForRecordWebsiteUrls { get; set; }
    }
}
