using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaGarden.Pipeline;
using Downplay.Origami.Services;
using MediaGarden.ViewModels;

namespace MediaGarden.Sources
{
    public class QueryInputDriver : MediaInputDriver<QueryInputModel>
    {

        protected override string Prefix
        {
            get { return "Query"; }
        }

        protected override ModelDriverResult Update(MediaInputsViewModel model, QueryInputModel viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelEditorShapeContext context)
        {
            var prefix = FullPrefix(context);
            // Add the query
            if (updater!=null) {
                if (!updater.TryUpdateModel(viewModel, prefix, null, null))
                {
                }
                if (!String.IsNullOrWhiteSpace(viewModel.QueryText))
                {
                    model.Queries.Add(viewModel.QueryText);
                }
            }
            return ModelShape("Media_Inputs_Query", () =>
            {
                return shapeHelper.EditorTemplate(TemplateName: "Media.Inputs.Query", Model: viewModel, Prefix: prefix);
            });
        }

    }
}