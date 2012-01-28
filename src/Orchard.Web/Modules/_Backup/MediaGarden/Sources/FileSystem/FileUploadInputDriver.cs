using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;
using Orchard.ContentManagement.Drivers;
using Downplay.Origami;
using Orchard.FileSystems.Media;
using Orchard;
using Orchard.UI.Notify;
using Orchard.Localization;
using Downplay.Origami.Services;
using MediaGarden.ViewModels;

namespace MediaGarden.Sources.FileSystem
{
    public class FileUploadInputDriver : MediaInputDriver<FileUploadInputModel>
    {
        private readonly IStorageProvider _storageProvider;
        public FileUploadInputDriver(
            IOrchardServices services,
            IStorageProvider storageProvider
            )
        {
            Services = services;
            _storageProvider = storageProvider;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; } 

        protected override string Prefix
        {
            get { return "FileUpload"; }
        }

        protected override ModelDriverResult Update(MediaInputsViewModel model, FileUploadInputModel viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);
            if (updater != null)
            {
                if (updater.TryUpdateModel(viewModel, prefix, null, null))
                {
                    var path = viewModel.MediaPath ?? "";
                    // Multi upload (jQuery)
                    if (viewModel.UploadFiles != null)
                    {
                        List<string> uploaded = new List<string>();
                        // TODO: Right now we have MIME types and really we have to use them because there's no easy way to extract them once file is saved
                        foreach (var file in viewModel.UploadFiles.Where(f => f != null))
                        {
                            var fileName = (path.Trim('/') + "/" + file.FileName).Trim('/');
                            if (_storageProvider.TrySaveStream(fileName, file.InputStream))
                            {
                                uploaded.Add(fileName);
                            }
                            else
                            {
                                updater.AddModelError(prefix + ".MediaPath", T("An error occurred uploading file '{0}' to path '{1}'. Check the file does not already exist and that write permissions are enabled on the corresponding folder.", fileName, path));
                            }
                        }
                        if (uploaded.Any())
                        {
                            model.Queries.Add(uploaded.Glue(";"));
                        }
                    }
                }
            }
            return EditorShape("Media_Inputs_FileUpload",viewModel,context);
            // TODO: Also needs a separate entry point into the pipeline to handle uploads via service (thru upload
            // component). And we need to get to be able to get a target media folder from the rest of the UI.
        }
    }
}