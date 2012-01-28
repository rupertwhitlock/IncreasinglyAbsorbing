namespace Orchard.jPlayer.Utils {
    public class JavaScriptHelper {
        public static string AddScriptTag(string script) {
            return string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", script);
        }
    }
}