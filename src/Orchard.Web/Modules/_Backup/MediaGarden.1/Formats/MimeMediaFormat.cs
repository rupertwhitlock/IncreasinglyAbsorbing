using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;
using MediaGarden.Models;

namespace MediaGarden.Formats
{
    /// <summary>
    /// Base abstract implementation for a media format identified by mime type(s) or file extension(s)
    /// </summary>
    public abstract class MimeMediaFormat : IMediaFormat
    {
        /// <summary>
        /// Identifier name for format
        /// </summary>
        public abstract string FormatName { get; }
        /// <summary>
        /// Human-readable format description
        /// </summary>
        public abstract string FormatDisplayName { get; }

        public abstract string MediaStereotype { get; }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool IsOfFormat(MediaSourceContext source)
        {
            var result = CheckMimeTypeAndExtension(source);
            if (!result) return false;
            // Perform further parsing
            return OnMatched(source);
        }

        /// <summary>
        /// Overridable method to perform further checking if required
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual bool OnMatched(MediaSourceContext source)
        {
            return true;
        }

        protected virtual bool CheckMimeTypeAndExtension(MediaSourceContext source)
        {
            // Check file extension then mime type
            var fileExt = source.Source.FileExtension();
            var isExt = true;
            if (!String.IsNullOrWhiteSpace(fileExt) && FileExtensions().Any())
            {
                isExt = (FileExtensions().Any(ext => String.Equals(fileExt,ext,StringComparison.InvariantCultureIgnoreCase)));
            }
            var isMime = true;
            var mimetype = source.Source.Metadata["MimeType"];
            if (!String.IsNullOrWhiteSpace(mimetype) && MimeTypes().Any())
            {
                isMime = (MimeTypes().Any(mime =>String.Equals(mimetype,mime,StringComparison.InvariantCultureIgnoreCase)));
            }
            // TODO: Not sure about having to match both but it's the only way now to distinguish between e.g. ogg video or ogg audio
            // Need to check how IIS handles this kind of thing; and probably we'll end up needing to check codecs in every instance
            // But that's for another week...
            return (isExt && isMime);
        }

        public abstract IEnumerable<String> FileExtensions();
        public abstract IEnumerable<String> MimeTypes();
    }
}