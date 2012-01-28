using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Orchard.jPlayer {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }

        public AdminMenu() {
            T = NullLocalizer.Instance;
        }

        public string MenuName {
            get { return "admin"; }
        }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("jPlayer"), "8",
                        menu => menu.Add(T("jPlayer"), "0", item => item.Action("Index", "Admin", new { area = "Orchard.jPlayer" })));
        }
    }
}