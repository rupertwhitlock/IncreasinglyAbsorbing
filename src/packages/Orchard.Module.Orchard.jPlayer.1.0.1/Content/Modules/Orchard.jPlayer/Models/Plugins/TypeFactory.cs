using Orchard.jPlayer.Models.Plugins.Audio;

namespace Orchard.jPlayer.Models.Plugins {
    public abstract class TypeFactory {
        public static TypeFactory GetFactory(MediaType type) {
            if(type == MediaType.Audio) {
                return new AudioFactory();
            }

            return new Orchard.jPlayer.Models.Plugins.Video.VideoFactory();
        }

        public abstract MediaGalleryType Type { get; }

        public abstract TypeResourceDescriptor TypeResourceDescriptor { get; }
    }
}