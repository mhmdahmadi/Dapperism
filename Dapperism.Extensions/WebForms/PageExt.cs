using System.Web.UI;

namespace Dapperism.Extensions.WebForms
{
    public static class PageExt
    {
        public static void ForceJavaScriptEnabled(this Page page, string path, int redirectionTime = 0)
        {
            var p = page.ResolveUrl(path);
            var address = string.Format(
                @"<noscript><meta http-equiv='refresh' content='{0}; url={1}' /></noscript>", redirectionTime,
                p);
            page.Controls.Add(new LiteralControl(address));
        }

        public static void SetAutoRedirection(this Page page, string path, int redirectionTime = 0)
        {
            var p = page.ResolveUrl(path);
            var address = string.Format(
                @"<meta http-equiv='refresh' content='{0}; url={1}' />", redirectionTime,
                p);
            page.Controls.Add(new LiteralControl(address));
        }

        public static void ForceModernIE(this Page page, string path)
        {
            var p = page.ResolveUrl(path);
            var js =
                @"var e=-1;if(navigator.appName==""Microsoft Internet Explorer""){var t=navigator.userAgent;var n=new RegExp(""MSIE ([0-9]{1,}[.0-9]{0,})"");if(n.exec(t)!=null){e=parseFloat(RegExp.$1)}}var r=e;if(r>-1){if(r<9){window.location.href=""" +
                p + @"""}}";
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "ModernIE", js, true);
            page.ClientScript.RegisterStartupScript(page.GetType(), "ModernIE", js, true);
        }
    }
}
