using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami;

namespace MediaGarden.Sources.FileSystem
{
    public class FileUploadInputModel
    {

        public IEnumerable<HttpPostedFileBase> UploadFiles { get; set; }
        public string MediaPath { get; set; }
    }
}