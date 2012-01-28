using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.UI.Navigation;

namespace ThereAndBackAgain.WantedRecord
{
    public class AdminMenu  : INavigationProvider
    {
            public string MenuName {
                get { return "admin"; }
            }

            public void GetNavigation(NavigationBuilder builder) {
                builder.AddImageSet("page");
            }
    }
}
