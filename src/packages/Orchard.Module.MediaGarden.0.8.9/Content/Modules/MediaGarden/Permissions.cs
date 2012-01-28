using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;

namespace MediaGarden
{
    public class Permissions  : IPermissionProvider
    {
        // TODO: More fine-grained media permissions and use DynamicPermissions to generate per-media-type perms
        public static readonly Permission ImportMedia = new Permission { Description = "Import media from sources", Name = "ImportMedia", ImpliedBy = new[] { Orchard.Media.Permissions.ManageMedia } };
        public static readonly Permission ListMedia = new Permission { Description = "List available media", Name = "ListMedia", ImpliedBy = new[] { Orchard.Media.Permissions.ManageMedia, ImportMedia } };

        public Orchard.Environment.Extensions.Models.Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]{
                ImportMedia,
                ListMedia
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]{
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] { ImportMedia }
                },
                new PermissionStereotype {
                    Name = "Moderator",
                    Permissions = new[] { ListMedia }
                },
                new PermissionStereotype {
                    Name = "Contributor",
                    Permissions = new[] { ImportMedia }
                }
            };
        }
    }
}