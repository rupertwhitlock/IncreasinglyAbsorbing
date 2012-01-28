namespace Orchard.jPlayer.Models.Plugins
{
    public abstract class jPlayerPlugin
    {
        public abstract string ToString(string cssSelector);

        public virtual string AdditionalHrefMarkup
        {
            get { return string.Empty; }
        }

        public virtual string jPlayerTemplateName
        {
            get { return "Parts/jPlayer"; }
        }
    }
}