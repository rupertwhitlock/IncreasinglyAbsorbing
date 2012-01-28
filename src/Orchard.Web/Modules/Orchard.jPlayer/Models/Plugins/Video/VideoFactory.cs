namespace Orchard.jPlayer.Models.Plugins.Video {
    public sealed class VideoFactory : TypeFactory {
        public VideoFactory() {
            _typeResourceDescriptor = new VideoResourceDescriptor();
            _type = new Video();
        }

        private readonly MediaGalleryType _type;

        private readonly TypeResourceDescriptor _typeResourceDescriptor;

        public override MediaGalleryType Type {
            get { return _type; }
        }

        public override TypeResourceDescriptor TypeResourceDescriptor {
            get { return _typeResourceDescriptor; }
        }
    }
}