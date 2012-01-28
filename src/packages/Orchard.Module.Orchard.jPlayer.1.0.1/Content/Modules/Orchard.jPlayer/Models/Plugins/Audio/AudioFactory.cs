namespace Orchard.jPlayer.Models.Plugins.Audio {
    public sealed class AudioFactory : TypeFactory {
        public AudioFactory() {
            _typeResourceDescriptor = new AudioResourceDescriptor();
            _type = new Audio();
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