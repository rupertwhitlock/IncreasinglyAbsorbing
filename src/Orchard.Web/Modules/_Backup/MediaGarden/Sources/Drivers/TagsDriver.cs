using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami;
using MediaGarden.Pipeline;
using Orchard.ContentManagement;
using MediaGarden.Models;
using Downplay.Origami.Services;
using MediaGarden.ViewModels;
using MediaGarden.Sources.ViewModels;
using Orchard.Tags.Models;
using Orchard.Tags.Helpers;
using Orchard.Tags.Services;

namespace MediaGarden.Sources.Drivers
{
    /// <summary>
    /// TODO: Maybe Tags should be supported in MediaGarden.Defaults - or maybe they're just way too useful and we should be supporting them always anyway. For now this is fine.
    /// </summary>
    public class TagsDriver : MediaSourceDriver<TagsModel,TagsPart>
    {
        private readonly ITagService _tagService;

        public TagsDriver(
            ITagService tagService
            )
        {
            _tagService = tagService;
        }

        protected override TagsModel ViewModel(IMediaSource source)
        {
            return new TagsModel()
            {
                Tags = source.Metadata.ValueOrDefault("Tags","")
            };
        }

        protected override string Prefix
        {
            get { return "Title"; }
        }
        protected override ModelDriverResult Update(MediaSourceViewModel source, TagsModel viewModel, dynamic shapeHelper, IUpdateModel updater, ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);
            if (updater != null)
            {
                if (updater.TryUpdateModel<TagsModel>(viewModel, prefix, null, null))
                {
                    // Save to source record
                    source.Source.Metadata["Tags"] = viewModel.Tags;
                }
            }
            return EditorShape("Media_Source_Tags",viewModel,context);
        }


        protected override void Apply(TagsPart part, TagsModel model, IMediaSource source, MediaCreateContext context)
        {
        }

        protected override void Applied(TagsPart part, TagsModel model, IMediaSource source, MediaCreateContext context)
        {
            // Copied from Orchard.Tags. Needs to happen after other updates, since the item will have been Created by now, so that tag links can be done properly.
            var tagNames = TagHelpers.ParseCommaSeparatedTagNames(model.Tags);
            if (part.ContentItem.Id != 0)
            {
                _tagService.UpdateTagsForContentItem(part.ContentItem, tagNames);
            }
        }

    }
}