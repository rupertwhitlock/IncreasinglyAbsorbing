namespace Orchard.jPlayer.Models.Plugins.Video {
    public class Video : MediaGalleryType {
        public override string MediaGalleryTemplateName {
            get { return "Parts/Video"; }
        }

        protected override MediaType MediaType {
            get {
                return MediaType.Audio;
            }
        }
    }
}