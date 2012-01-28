using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard.Core.Contents.Controllers;
using Orchard.jPlayer.Services;
using Orchard.jPlayer.ViewModels;
using Orchard.Localization;
using Orchard.UI.Notify;

namespace Orchard.jPlayer.Controllers {
    public class AdminController : Controller {
        private readonly IMediaGalleryService _mediaGalleryService;

        public AdminController(IOrchardServices services, IMediaGalleryService mediaGalleryService) {
            Services = services;
            _mediaGalleryService = mediaGalleryService;

            T = NullLocalizer.Instance;
        }

        public IOrchardServices Services { get; set; }

        public Localizer T { get; set; }

        [HttpGet]
        public ViewResult Index() {
            return View(new MediaGalleryIndexViewModel { MediaGalleries = _mediaGalleryService.GetMediaGalleries() });
        }

        [HttpGet]
        public ActionResult Create() {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Couldn't create media gallery"))) {
                return new HttpUnauthorizedResult();
            }

            return View(new CreateGalleryViewModel());
        }

        [HttpPost]
        public ActionResult Create(CreateGalleryViewModel addGalleryViewModel) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Couldn't create media gallery"))) {
                return new HttpUnauthorizedResult();
            }
            if(!ModelState.IsValid) {
                return View(addGalleryViewModel);
            }

            try {
                _mediaGalleryService.CreateMediaGallery(addGalleryViewModel.GalleryName);

                Services.Notifier.Information(T("Media gallery created"));
                return RedirectToAction("Index");
            }
            catch(Exception exception) {
                Services.Notifier.Error(T("Creating media gallery failed: {0}", exception.Message));
                return View(addGalleryViewModel);
            }
        }

        [HttpGet]
        public ActionResult Medias(string mediaGalleryName) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot edit media gallery"))) {
                return new HttpUnauthorizedResult();
            }

            var mediaGallery = _mediaGalleryService.GetMediaGallery(mediaGalleryName);

            return View(new MediaGalleryMediasViewModel {
                MediaGalleryName = mediaGallery.Name,
                Medias = mediaGallery.Medias,
                GripIconPublicUrl = _mediaGalleryService.GetPublicUrl("Content/grip.png")
            });
        }

        [HttpGet]
        public ActionResult EditProperties(string mediaGalleryName) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot edit media gallery"))) {
                return new HttpUnauthorizedResult();
            }

            return View(new MediaGalleryEditPropertiesViewModel { MediaGallery = _mediaGalleryService.GetMediaGallery(mediaGalleryName) });
        }

        [HttpPost]
        [FormValueRequired("submit.Save")]
        public ActionResult EditProperties(MediaGalleryEditPropertiesViewModel viewModel, string newName) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot edit media gallery"))) {
                return new HttpUnauthorizedResult();
            }

            if(!ModelState.IsValid) {
                return View(viewModel);
            }

            if(string.IsNullOrEmpty(newName)) {
                ModelState.AddModelError("NewName", T("Invalid media gallery name").ToString());
                return View(viewModel);
            }

            try {
                _mediaGalleryService.UpdateMediaGalleryProperties(viewModel.MediaGallery.Name);

                if(viewModel.MediaGallery.Name != newName) {
                    _mediaGalleryService.RenameMediaGallery(viewModel.MediaGallery.Name, newName);
                }

                Services.Notifier.Information(T("Media gallery properties successfully modified"));
                return RedirectToAction("Medias", new { mediaGalleryName = newName });
            }
            catch(Exception exception) {
                Services.Notifier.Error(T("Editing media gallery failed: {0}", exception.Message));
                return View(viewModel);
            }
        }

        [HttpGet]
        public ActionResult AddMedias(string mediaGalleryName) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot add medias to media gallery"))) {
                return new HttpUnauthorizedResult();
            }

            return View(new MediaAddViewModel());
        }

        [HttpPost]
        public ActionResult AddMedias(MediaAddViewModel viewModel) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Couldn't upload media file"))) {
                return new HttpUnauthorizedResult();
            }

            if(!ModelState.IsValid)
                return View(viewModel);

            try {
                if(viewModel.MediaFiles == null || viewModel.MediaFiles.Count() == 0) {
                    ModelState.AddModelError("File", T("Select a file to upload").ToString());

                    return View(new MediaAddViewModel());
                }

                if(viewModel.MediaFiles.Any(file => !_mediaGalleryService.IsFileAllowed(file))) {
                    ModelState.AddModelError("File", T("That file type is not allowed.").ToString());
                    return View(viewModel);
                }

                foreach(var file in viewModel.MediaFiles) {
                    _mediaGalleryService.AddMedia(viewModel.MediaGalleryName, file);
                }
            }
            catch(Exception exception) {
                Services.Notifier.Error(T("Adding media failed: {0}", exception.Message));
                return View(new MediaAddViewModel());
            }

            return RedirectToAction("Medias", new { mediaGalleryName = viewModel.MediaGalleryName });
        }

        public ActionResult EditMedia(string mediaGalleryName, string mediaName) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot edit media"))) {
                return new HttpUnauthorizedResult();
            }

            return
                View(new MediaEditViewModel { MediaGalleryName = mediaGalleryName, Media = _mediaGalleryService.GetMedia(mediaGalleryName, mediaName) });
        }

        [HttpPost]
        [FormValueRequired("submit.Save")]
        public ActionResult EditMedia(MediaEditViewModel viewModel) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot edit media"))) {
                return new HttpUnauthorizedResult();
            }

            try {
                try {
                    _mediaGalleryService.UpdateMediaProperties(viewModel.MediaGalleryName, viewModel.Media.Name,
                                                               viewModel.Media.Title);
                }
                catch(Exception exception) {
                    Services.Notifier.Error(T("Editing media properties failed: {0}", exception.Message));
                    return
                        View(new MediaEditViewModel {
                            MediaGalleryName = viewModel.MediaGalleryName,
                            Media = _mediaGalleryService.GetMedia(viewModel.MediaGalleryName, viewModel.Media.Name)
                        });
                }

                Services.Notifier.Information(T("Media properties successfully modified"));
            }
            catch(Exception exception) {
                Services.Notifier.Error(T("Saving media failed: {0}", exception.Message));
            }

            return RedirectToAction("Medias", new { mediaGalleryName = viewModel.MediaGalleryName });
        }

        [HttpPost]
        [FormValueRequired("submit.DeleteMedia")]
        [ActionName("EditMedia")]
        public ActionResult DeleteMedia(string mediaGalleryName, string mediaName) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot delete media"))) {
                return new HttpUnauthorizedResult();
            }

            try {
                _mediaGalleryService.DeleteMedia(mediaGalleryName, mediaName);

                Services.Notifier.Information(T("Media successfully deleted"));
            }
            catch(Exception exception) {
                Services.Notifier.Error(T("Deleting media failed: {0}", exception.Message));
            }

            return RedirectToAction("Medias", new { mediaGalleryName });
        }

        [HttpPost]
        [FormValueRequired("submit.Delete")]
        [ActionName("EditProperties")]
        public ActionResult Delete(string mediaGalleryName) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot delete media gallery"))) {
                return new HttpUnauthorizedResult();
            }

            try {
                _mediaGalleryService.DeleteMediaGallery(mediaGalleryName);
                Services.Notifier.Information(T(string.Format("Media gallery \"{0}\" deleted", mediaGalleryName)));
            }
            catch(Exception exception) {
                Services.Notifier.Error(T("Deleting media gallery failed: {0}", exception.Message));
            }

            return RedirectToAction("Index");
        }

        public JsonResult Reorder(string mediaGalleryName, IEnumerable<string> medias) {
            if(!Services.Authorizer.Authorize(Permissions.ManageMediaGallery, T("Cannot delete media gallery"))) {
                return Json(new HttpUnauthorizedResult());
            }

            _mediaGalleryService.ReorderMedias(mediaGalleryName, medias);

            return new JsonResult();
        }
    }
}