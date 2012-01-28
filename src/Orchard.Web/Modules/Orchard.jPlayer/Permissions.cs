using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace Orchard.jPlayer {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageMediaGallery = new Permission { Description = "Managing Media Gallery", Name = "ManageMediaGallery" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[]
                   {
                       ManageMediaGallery,
                   };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[]
                   {
                       new PermissionStereotype
                       {
                           Name = "Administrator",
                           Permissions = new[] {ManageMediaGallery}
                       },
                       new PermissionStereotype
                       {
                           Name = "Editor",
                           Permissions = new[] {ManageMediaGallery}
                       },
                       new PermissionStereotype
                       {
                           Name = "Moderator",
                       },
                       new PermissionStereotype
                       {
                           Name = "Author",
                           Permissions = new[] {ManageMediaGallery}
                       },
                       new PermissionStereotype
                       {
                           Name = "Contributor",
                       },
                   };
        }
    }
}