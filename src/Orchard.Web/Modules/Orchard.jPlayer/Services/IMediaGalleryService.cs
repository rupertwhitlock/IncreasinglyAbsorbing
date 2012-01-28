using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Orchard.jPlayer.Models;

namespace Orchard.jPlayer.Services {
    public interface IMediaGalleryService : IDependency {
        IEnumerable<MediaGallery> GetMediaGalleries();
        MediaGallery GetMediaGallery(string mediaGalleryName);

        void CreateMediaGallery(string mediaGalleryName);
        void DeleteMediaGallery(string mediaGalleryName);
        void RenameMediaGallery(string mediaGalleryName, string newName);
        void UpdateMediaGalleryProperties(string name);

        MediaGalleryMedia GetMedia(string galleryName, string mediaName);
        void AddMedia(string mediaGalleryName, HttpPostedFileBase mediaFile);
        void UpdateMediaProperties(string mediaGalleryName, string mediaName, string mediaTitle);
        void DeleteMedia(string mediaGalleryName, string mediaName);

        string GetPublicUrl(string path);
        bool IsFileAllowed(HttpPostedFileBase file);

        void ReorderMedias(string mediaGalleryName, IEnumerable<string> medias);
    }
}