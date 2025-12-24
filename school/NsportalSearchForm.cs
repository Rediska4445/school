using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace school
{
    public partial class NsportalSearchForm : Form
    {
        private WebView2 webView;

        public NsportalSearchForm()
        {
            this.Text = "nsportal.ru";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            SetupWebView("https://nsportal.ru/page/poisk-po-saitu");
        }

        private void SetupWebView(string url)
        {
            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(webView);

            webView.NavigationCompleted += async (s, e) =>
            {
                if (e.IsSuccess)
                {
                    await webView.EnsureCoreWebView2Async();

                    await webView.CoreWebView2.ExecuteScriptAsync(@"
                    var links = document.querySelectorAll('a[href*=""pptx""], a[href*=""pdf""]');
                    links.forEach(l => {
                        l.style.border = '2px solid orange';
                        l.style.background = '#ffebee';
                        l.style.padding = '2px 4px';
                        l.title = '📥 Скачать презентацию';
                    });
                ");
                }
            };

            webView.Source = new Uri(url);
        }
    }

}
