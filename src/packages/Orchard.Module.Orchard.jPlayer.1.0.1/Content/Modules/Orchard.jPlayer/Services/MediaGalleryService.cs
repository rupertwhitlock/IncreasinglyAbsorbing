using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data;
using Orchard.jPlayer.Models;
using Orchard.Media.Models;
using Orchard.Media.Services;

namespace Orchard.jPlayer.Services {
    public class MediaGalleryService : IMediaGalleryService {
        private const string MediaGalleriesMediaFolder = "MediaGalleries";

        private readonly IMediaService _mediaService;
        private readonly IRepository<MediaGallerySettingsRecord> _repository;
        private readonly IRepository<MediaGalleryMediaSettingsRecord> _mediaRepository;
        private readonly IRepository<MediaGalleryRecord> _mediaGalleryPartRepository;

        public MediaGalleryService(IMediaService mediaService, IRepository<MediaGallerySettingsRecord> repository,
                                  IRepository<MediaGalleryMediaSettingsRecord> mediaRepository,
                                  IRepository<MediaGalleryRecord> mediaGalleryPartRepository) {
            _mediaGalleryPartRepository = mediaGalleryPartRepository;
            _repository = repository;
            _mediaService = mediaService;
            _mediaRepository = mediaRepository;

            if(!_mediaService.GetMediaFolders(string.Empty).Any(o => o.Name == MediaGalleriesMediaFolder)) {
                _mediaService.CreateFolder(string.Empty, MediaGalleriesMediaFolder);
            }
        }

        public IEnumerable<MediaGallery> GetMediaGalleries() {
            return _mediaService.GetMediaFolders(MediaGalleriesMediaFolder).Select(CreateMediaGalleryFromMediaFolder);
        }

        private Models.MediaGallery CreateMediaGalleryFromMediaFolder(MediaFolder mediaFolder) {
            var medias = _mediaService.GetMediaFiles(mediaFolder.MediaPath);
            MediaGallerySettingsRecord mediaGallerySettings = GetMediaGallerySettings(GetName(mediaFolder.MediaPath)) ??
                                                              CreateMediaGallerySettings(mediaFolder.MediaPath);

            MediaGalleryRecord mediaGallery = _mediaGalleryPartRepository.Get(x => x.MediaGalleryName == mediaFolder.Name);

            var autoPlay = false;

            if(mediaGallery != null)
                autoPlay = mediaGallery.AutoPlay;

            return new Models.MediaGallery {
                Id = mediaGallerySettings.Id,
                LastUpdated = mediaFolder.LastUpdated,
                MediaPath = mediaFolder.MediaPath,
                Name = mediaFolder.Name,
                Size = mediaFolder.Size,
                User = mediaFolder.User,
                AutoPlay = autoPlay,
                Medias = medias.Select(media => CreateMediaFromMediaFile(media, mediaGallerySettings)).OrderBy(media => media.Position)
            };
        }

        private MediaGallerySettingsRecord CreateMediaGallerySettings(string imageGalleryMediaPath) {
            MediaGallerySettingsRecord imageGallerySettings = new MediaGallerySettingsRecord {
                MediaGalleryName = GetName(imageGalleryMediaPath)
            };
            _repository.Create(imageGallerySettings);

            return imageGallerySettings;
        }

        private MediaGalleryMedia CreateMediaFromMediaFile(MediaFile mediaFile, MediaGallerySettingsRecord mediaGallerySettings) {
            if(mediaGallerySettings == null) {
                throw new ArgumentNullException("imageGallerySettings");
            }

            var mediaSettings = GetMediaSettings(mediaGallerySettings, mediaFile.Name);

            return new MediaGalleryMedia {
                PublicUrl = _mediaService.GetPublicUrl(System.IO.Path.Combine(mediaFile.FolderName, mediaFile.Name)),
                Name = mediaFile.Name,
                Size = mediaFile.Size,
                User = mediaFile.User,
                LastUpdated = mediaFile.LastUpdated,
                Title = mediaSettings == null ? null : mediaSettings.Title,
                Position = mediaSettings == null ? 0 : mediaSettings.Position
            };
        }

        private MediaGalleryMediaSettingsRecord GetMediaSettings(MediaGallerySettingsRecord mediaGallerySettings, string imageName) {
            if(mediaGallerySettings == null)
                return null;
            return mediaGallerySettings.MediaSettings.SingleOrDefault(o => o.Name == imageName);
        }

        private MediaGallerySettingsRecord GetMediaGallerySettings(string mediaGalleryName) {
            if(mediaGalleryName.Contains("\\"))
                mediaGalleryName = GetName(mediaGalleryName);
            return _repository.Get(o => o.MediaGalleryName == mediaGalleryName);
        }

        private string GetName(string mediaPath) {
            return mediaPath.Split('\\').Last();
        }

        public MediaGallery GetMediaGallery(string mediaGalleryName) {
            if(mediaGalleryName.Contains("\\"))
                mediaGalleryName = GetName(mediaGalleryName);

            var mediaFolder = _mediaService.GetMediaFolders(MediaGalleriesMediaFolder).SingleOrDefault(m => m.Name == mediaGalleryName);

            if(mediaFolder != null) {
                return CreateMediaGalleryFromMediaFolder(mediaFolder);
            }
            return null;
        }

        public void CreateMediaGallery(string mediaGalleryName) {
            _mediaService.CreateFolder(MediaGalleriesMediaFolder, mediaGalleryName);
        }

        public void DeleteMediaGallery(string mediaGalleryName) {
            var gallerySettings = GetMediaGallerySettings(GetMediaPath(mediaGalleryName));

            foreach(MediaGalleryMedia media in GetMediaGallery(mediaGalleryName).Medias) {
                DeleteMedia(mediaGalleryName, media.Name, GetMediaSettings(gallerySettings, media.Name));
            }

            if(gallerySettings != null)
                _repository.Delete(gallerySettings);
            _mediaService.DeleteFolder(GetMediaPath(mediaGalleryName));
        }

        private void DeleteMedia(string mediaGalleryName, string mediaName, MediaGalleryMediaSettingsRecord mediaSettings) {
            if(mediaSettings != null) {
                _mediaRepository.Delete(mediaSettings);
            }
            _mediaService.DeleteFile(mediaName, GetMediaPath(mediaGalleryName));
        }

        private string GetMediaPath(string imageGalleryName) {
            return string.Concat(MediaGalleriesMediaFolder, "\\", imageGalleryName);
        }

        public void RenameMediaGallery(string mediaGalleryName, string newName) {
            string mediaPath = GetMediaPath(mediaGalleryName);
            _mediaService.RenameFolder(mediaPath, newName);

            MediaGallerySettingsRecord settings = GetMediaGallerySettings(mediaGalleryName);
            if(settings != null) {
                settings.MediaGalleryName = newName;
                _repository.Update(settings);
            }

            IEnumerable<MediaGalleryRecord> records = _mediaGalleryPartRepository.Fetch(partRecord => partRecord.MediaGalleryName == mediaGalleryName);

            foreach(MediaGalleryRecord imageGalleryRecord in records) {
                imageGalleryRecord.MediaGalleryName = newName;
                _mediaGalleryPartRepository.Update(imageGalleryRecord);
            }
        }

        public void UpdateMediaGalleryProperties(string name) {
            var mediaGallery = GetMediaGallery(name);
            var mediaGallerySettings = GetMediaGallerySettings(mediaGallery.MediaPath);

            if(mediaGallerySettings == null) {
                CreateMediaGallerySettings(mediaGallery.MediaPath);
            }
            else {
                _repository.Update(mediaGallerySettings);
            }
        }

        public MediaGalleryMedia GetMedia(string galleryName, string mediaName) {
            string imageGalleryMediaPath = GetMediaPath(galleryName);
            MediaGallerySettingsRecord imageGallerySettings = GetMediaGallerySettings(imageGalleryMediaPath);

            MediaFile file = _mediaService.GetMediaFiles(imageGalleryMediaPath)
                .SingleOrDefault(mediaFile => mediaFile.Name == mediaName);

            if(file == null) {
                return null;
            }

            return CreateMediaFromMediaFile(file, imageGallerySettings);
        }

        public void AddMedia(string mediaGalleryName, HttpPostedFileBase mediaFile) {
            _mediaService.UploadMediaFile(GetMediaPath(mediaGalleryName), mediaFile, false);
        }

        public void UpdateMediaProperties(string mediaGalleryName, string mediaName, string mediaTitle) {
            UpdateMediaProperties(mediaGalleryName, mediaName, mediaTitle, null);
        }

        private void UpdateMediaProperties(string mediaGalleryName, string mediaName, string mediaTitle, int? position) {
            var media = GetMedia(mediaGalleryName, mediaName);
            var mediaGallery = GetMediaGallery(mediaGalleryName);

            var mediaGallerySettings = GetMediaGallerySettings(mediaGallery.MediaPath);

            if(mediaGallerySettings.MediaSettings.Any(o => o.Name == media.Name)) {
                var mediaSetting = mediaGallerySettings.MediaSettings.Single(o => o.Name == media.Name);
                mediaSetting.Title = mediaTitle;
                if(position.HasValue)
                    mediaSetting.Position = position.Value;
                _mediaRepository.Update(mediaSetting); // TODO: Remove when cascade is fixed
            }
            else {
                var mediaSetting = new MediaGalleryMediaSettingsRecord { Name = media.Name, Title = mediaTitle };
                if(position.HasValue)
                    mediaSetting.Position = position.Value;
                mediaGallerySettings.MediaSettings.Add(mediaSetting);
                _mediaRepository.Create(mediaSetting); // TODO: Remove when cascade is fixed
            }

            // TODO: See how to cascade changes
            _repository.Update(mediaGallerySettings);
        }

        public void DeleteMedia(string mediaGalleryName, string mediaName) {
            var mediaSettings = GetMediaSettings(mediaGalleryName, mediaName);
            DeleteMedia(mediaGalleryName, mediaName, mediaSettings);
        }

        private MediaGalleryMediaSettingsRecord GetMediaSettings(string mediaGalleryName, string mediaName) {
            var mediaGallerySettings = GetMediaGallerySettings(GetMediaPath(mediaGalleryName));

            return mediaGallerySettings.MediaSettings.SingleOrDefault(o => o.Name == mediaName);
        }

        public string GetPublicUrl(string path) {
            return _mediaService.GetPublicUrl(path);
        }

        public bool IsFileAllowed(HttpPostedFileBase file) {
            return _mediaService.FileAllowed(file);
        }

        public void ReorderMedias(string mediaGalleryName, IEnumerable<string> medias) {
            Models.MediaGallery mediaGallery = GetMediaGallery(mediaGalleryName);
            int position = 0;

            foreach(string media in medias) {
                MediaGalleryMedia mediaGalleryMedia = mediaGallery.Medias.Single(o => o.Name == media);
                mediaGalleryMedia.Position = position++;
                UpdateMediaProperties(mediaGalleryName, mediaGalleryMedia.Name, mediaGalleryMedia.Title,
                                      mediaGalleryMedia.Position);
            }

            foreach(MediaGalleryMedia mediaGalleryMedia in mediaGallery.Medias.Where(o => !medias.Contains(o.Name))) {
                mediaGalleryMedia.Position = position++;
                UpdateMediaProperties(mediaGalleryName, mediaGalleryMedia.Name, mediaGalleryMedia.Title,
                                      mediaGalleryMedia.Position);
            }
        }
    }
}