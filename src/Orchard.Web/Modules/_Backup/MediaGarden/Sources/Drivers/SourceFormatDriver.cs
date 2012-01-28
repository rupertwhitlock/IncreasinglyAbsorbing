using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Sources.ViewModels;
using MediaGarden.ViewModels;
using Downplay.Origami.Services;
using MediaGarden.Services;
using System.Web.Mvc;
using Orchard.Localization;
using MediaGarden.Pipeline;
using MediaGarden.Models;

namespace MediaGarden.Sources.Drivers
{
    public class SourceFormatDriver : ModelDriver<MediaSourceViewModel,SourceFormatViewModel>, IMediaSourceFilter
    {

        private readonly IMediaGardenService _gardenService;

        public SourceFormatDriver(
            IMediaGardenService gardenService)
        {
            _gardenService = gardenService;
        }

        public Localizer T { get; set; }

        protected override string Prefix
        {
            get { return "Format"; }
        }

        /// <summary>
        /// TODO: This is totally similar to what we need to do for MediaPart_Edit ... so maybe that can call the origami service and general the same model?
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override SourceFormatViewModel ViewModel(MediaSourceViewModel model, ModelShapeContext context)
        {
            var viewer = _gardenService.FindViewer(model.Preview);
            var viewModel = new SourceFormatViewModel()
            {
                Formats =
                new[]{
                    new SelectListItem() {
                        Selected = String.IsNullOrWhiteSpace(model.Source.Record.FormatName),
                        Text =T("(default)").Text,
                        Value = ""
                    }
                    }.Concat(
                    _gardenService.MatchFormats(model.Source, model.Source.MediaStereotype).Select(f =>
                    new SelectListItem()
                    {
                        Selected = model.Source.Record.FormatName == f.FormatName,
                        Text = f.FormatDisplayName,
                        Value = f.FormatName
                    })),
                Stereotypes =
                                new[]{
                    new SelectListItem() {
                        Selected = String.IsNullOrWhiteSpace(model.Source.Record.MediaStereotype),
                        Text =T("(default)").Text,
                        Value = ""
                    }
                    }.Concat(

                _gardenService.AllStereotypes().Select(s =>
                new SelectListItem()
                {
                    Selected = model.Source.Record.MediaStereotype == s,
                    Text = s,
                    Value = s
                })),
                ContentTypes =
                                new[]{
                    new SelectListItem() {
                        Selected = String.IsNullOrWhiteSpace(model.ContentTypeName),
                        Text =T("(default)").Text,
                        Value = ""
                    }
                    }.Concat(

                _gardenService.AllMediaTypes().Select(s =>
                new SelectListItem()
                {
                    Selected = model.ContentTypeName == s.Name,
                    Text = s.DisplayName,
                    Value = s.Name
                })),

                Viewers =
                                                new[]{
                    new SelectListItem() {
                        Selected = String.IsNullOrWhiteSpace(model.Source.Record.ViewerName),
                        Text =T("(default)").Text,
                        Value = ""
                    }
                    }.Concat(
                        _gardenService.MatchViewers(model.Preview).Select(v => new SelectListItem()
                        {
                            Selected = model.Source.Record.ViewerName == v.ViewerName,
                            Value = v.ViewerName,
                            Text = v.ViewerDescription
                        })
                             )
            };
            viewModel.ContentTypeName = model.ContentTypeName;
            return viewModel;
        }

        protected override ModelDriverResult Update(MediaSourceViewModel model, SourceFormatViewModel viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);
            if (updater != null)
            {
                updater.TryUpdateModel(viewModel, prefix, null, null);

                // Validation
                if (viewModel.ContentTypeName!=null)
                {
                    if (!String.IsNullOrWhiteSpace(viewModel.ContentTypeName) && !viewModel.ContentTypes.Any(v => v.Value == viewModel.ContentTypeName))
                    {
                        updater.AddModelError(FullPrefix(context, "ContentTypeName"), T("Content type not supported"));
                    }
                    else {
                        model.Source.Metadata["ContentTypeName"] = viewModel.ContentTypeName;
                    }
                }
                if (viewModel.FormatName!=null)
                {
                    if (!String.IsNullOrWhiteSpace(viewModel.FormatName) && !viewModel.Formats.Any(v => v.Value == viewModel.FormatName))
                    {
                        updater.AddModelError(FullPrefix(context, "FormatName"), T("Format not available"));
                    }
                    else {
                        model.Source.Record.FormatName = viewModel.FormatName;
                    }
                }
                if (viewModel.ViewerName!= null )
                {
                    if (!String.IsNullOrWhiteSpace(viewModel.ViewerName) && !viewModel.Viewers.Any(v => v.Value == viewModel.ViewerName))
                    {
                        updater.AddModelError(FullPrefix(context, "ViewerName"), T("Viewer not available"));
                    }
                    else
                    {
                        model.Source.Record.ViewerName = viewModel.ViewerName;
                    }
                }
                if (viewModel.Stereotype!=null)
                {
                    if (!String.IsNullOrWhiteSpace(viewModel.Stereotype) && !viewModel.Stereotypes.Any(v => v.Value == viewModel.Stereotype))
                    {
                        updater.AddModelError(FullPrefix(context, "Stereotype"), T("Stereotype not supported"));
                    }
                    else
                    {
                        model.Source.Record.MediaStereotype = viewModel.Stereotype;
                    }
                }

            }

            return EditorShape("Media_Source_Format", viewModel, context);
        }

        public IEnumerable<string> SupportedFormats()
        {
            yield return "*";
        }


        public void BuildSourceMetadata(MediaQueryContext query, MediaLocationContext location, MediaSourceContext source)
        {
        }

        public void SourceFiltering(MediaContentContext contentContext, IMediaSource source)
        {
            // Register a creator for this source
            var creator = new MediaCreateContext(contentContext, source);

            // Determine content type to create
            string contentType = null;
            if (source.Metadata.ContainsKey("ContentTypeName"))
            {
                contentType = source.Metadata["ContentTypeName"];
            }
            // Get best available?
            if (String.IsNullOrWhiteSpace(contentType))
            {
                // TODO: Following is pretty random; should have some sort of priority setting
                var def = _gardenService.AllMediaTypes().Where(t => t.Settings["MediaStereotype"] == source.MediaStereotype).FirstOrDefault();
                if (def!=null) contentType = def.Name;
            }

            if (!String.IsNullOrWhiteSpace(contentType)) {
                creator.ContentType = contentType;
                contentContext.Creators.Add(creator);
            }
        }

        public void SourceFiltered(MediaContentContext contentContext, IMediaSource source)
        {
        }
    }
}