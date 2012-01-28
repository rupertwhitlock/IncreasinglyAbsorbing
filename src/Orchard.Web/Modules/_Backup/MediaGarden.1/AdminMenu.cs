using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Navigation;
using Orchard.Localization;
using Orchard;
using MediaGarden.Services;
using Orchard.Core.Contents.Settings;

namespace MediaGarden
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IOrchardServices _services;
        private readonly IMediaGardenService _gardenService;

        public Localizer T { get; set; }
        public string MenuName
        {
            get { return "admin"; }
        }

        public AdminMenu(IOrchardServices services, IMediaGardenService gardenService)
        {
            _services = services;
            _gardenService = gardenService;
        }


        public void GetNavigation(NavigationBuilder builder)
        {
            
            builder.Add(T("Media"), menu => {
                // Rename existing media to "Files" to avoid confusion
                // TODO: Doesn't work
                menu.Caption(T("Files"));
            });
            // Add submenus under content
            // TODO: This is generally useful. Should be a separate feature or even part of an entirely
            // different utility module.
            builder.Add(T("Content"), menu =>
            {
                menu.Action("List", "Admin", new { area = "Contents",id="" });
                var types = _services.ContentManager.GetContentTypeDefinitions()
                    .Where(ctd=>ctd.Settings.Has("Stereotype","Content")
                        && ctd.Settings.GetModel<ContentTypeSettings>().Creatable)
                        // TODO: Extra setting for display ordering
                        .OrderBy(ctd=>ctd.DisplayName);

                int pos = 1;
                foreach (var t in types)
                {
                    menu.Add(T(t.DisplayName),pos.ToString(), item => item
                        .Action("List", "Admin", new { area = "Contents", id = t.Name }));
                    pos++;
                }
            });
            // Place Media Garden directly beneath Content; should be as (if not more!) important
            builder.AddImageSet("media-garden");
            builder.Add(T("Media Garden"), "2.5", menu =>
            {
               // menu.Action("List", "Admin", new { area = "MediaGarden",id="" });
                menu /*.Add(T("Media"), "0", item => item.
                    Action("List", "Admin", new { area = "MediaGarden", id = "" }).LocalNav())*/
                    .Add(T("Add Media"), "0", item => item.
                    Action("Sources", "Admin", new { area = "MediaGarden", id = "" }).LocalNav());

             //   menu.Action("List", "Admin", new { area = "Media Garden" });
                // Add child items for each media type (using the stereotype)
                var types = _services.ContentManager.GetContentTypeDefinitions()
                    .Where(d => d.Settings.ContainsKey("MediaStereotype")
                        && d.Parts.Any(p => p.PartDefinition.Name == "MediaPart")
                    ); //.OrderBy(d=>d.DisplayName);
                var stereotypes = types.Select(t => t.Settings["MediaStereotype"]).Distinct().OrderBy(t => t);

                int n = 1;
                foreach (var stereotype in stereotypes)
                {
                    menu.Add(T(stereotype), n.ToString(), submenu => submenu
                         .Action("Sources", "Admin", new { area = "MediaGarden", id = stereotype })
                        .Add(T("Add "+stereotype), "0", item => item
                            // Set sub menu action
                            .Action("Sources", "Admin", new { area = "MediaGarden", id = stereotype }).LocalNav())
                        );
                    n++;
                }
                /* Old version with content type names
                foreach (var contentTypeDefinition in types)
                {
                    menu.Add(T(contentTypeDefinition.DisplayName), n.ToString(), submenu => submenu
                         .Action("Sources", "Admin", new { area = "MediaGarden", id = contentTypeDefinition.Name })
//                        .Action("List", "Admin", new { area = "MediaGarden", id = contentTypeDefinition.Name })
                        .Add(T("Sources"), "0", item => item
                            // Set sub menu action
                            .Action("Sources", "Admin", new { area = "MediaGarden", id = contentTypeDefinition.Name }).LocalNav())                        
                            /*
                        .Add(T("Media"), "1", item=>item
                            // Set sub menu action
                            .Action("List", "Admin", new { area = "Contents", id = contentTypeDefinition.Name }).LocalNav())
                            */
                  /*      );
                    n++;
                }
                */

            });
            // TODO: (Below) ... Create could be a tab in each submenu
                        // TODO: Review: Copied from "New" menu example in Orchard. Really do we need to create a new item just to discover the Create route?
            /*
                        var ci = _services.ContentManager.New(contentTypeDefinition.Name);
                        var cim = _services.ContentManager.GetItemMetadata(ci);
                        var createRouteValues = cim.CreateRouteValues;*/
                        // review: the display name should be a LocalizedString [from "New" menu also]
            
        }
    }
}