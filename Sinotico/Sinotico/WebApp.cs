using System;
using System.Windows.Forms;

namespace Sinotico
    {
    public partial class WebApp : Form
        {
        public WebApp()
            {
            InitializeComponent();
            }

        private void WebApp_Load(object sender, EventArgs e)
            {
            LoadingInfo.InfoText = "Waiting for response from http://loknitting.olimpias.it/Sinotico.aspx";
            LoadingInfo.ShowLoading();
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://loknitting.olimpias.it/Sinotico.aspx");   
            }

        private void button3_Click(object sender, EventArgs e)
            {
            LoadingInfo.InfoText = "Waiting for response from " + txtLink.Text;
            LoadingInfo.ShowLoading();
            webBrowser1.Navigate(txtLink.Text);
            }

        private void btnBack_Click(object sender, EventArgs e)
            {
            webBrowser1.GoBack();
            }

        private void btnForw_Click(object sender, EventArgs e)
            {
            webBrowser1.GoForward();
            }

        private void btnHome_Click(object sender, EventArgs e)
            {
            LoadingInfo.InfoText = "Waiting for response from http://loknitting.olimpias.it/Sinotico.aspx";
            LoadingInfo.ShowLoading();
            webBrowser1.Navigate("http://loknitting.olimpias.it/Sinotico.aspx");
            }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
            {
            txtLink.Text = e.Url.ToString();
            LoadingInfo.CloseLoading();
            }
        }
    }
