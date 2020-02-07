using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using UCExtend.Helpers;
using UCExtend.VideoTraining;

//Passing script between WinForms app and embeded browser::
////https://stackoverflow.com/questions/14172273/youtube-embedded-player-event-when-video-has-ended
////https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/implement-two-way-com-between-dhtml-and-client?redirectedfrom=MSDN
////https://www.htmlgoodies.com/beyond/video/respond-to-embedded-youtube-video-events.html
//CefSharp
////https://www.telerik.com/support/kb/winforms/details/how-to-embed-chrome-browser-in-a-winforms-application
////https://ourcodeworld.com/articles/read/173/how-to-use-cefsharp-chromium-embedded-framework-csharp-in-a-winforms-application
////https://www.codeproject.com/Articles/990346/Using-HTML-as-UI-Elements-in-a-WinForms-Applicatio

namespace UCExtend
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class VideoPlayer : Form
    {
        private readonly string[] resourceFiles = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        public ChromiumWebBrowser browser;

        public VideoPlayer()
        {
            InitializeComponent();
            InitBrowser();
            //LoadHtml("Z25ibgtwQa0");
        }

        public VideoPlayer(string VideoId)
        {
            InitializeComponent();
            InitBrowser();
            LoadHtml(VideoId);
        }

        private void VideoPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        public void VideoPlayer_Load(object sender, EventArgs e)
        {
            var data = DataFactory.GetVideos();
            foreach (var item in data)
            {
                DataFactory.Add(item);
            }
            
            comboBox1.DataSource = DataFactory.GetPlayList();
        }

        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            CefSharpSettings.LegacyJavascriptBindingEnabled = true; //Need to figure out new way - https://github.com/cefsharp/CefSharp/issues/2246
            browser = new ChromiumWebBrowser(string.Empty)
            {
                Location = new Point(0, 0),
                Dock = DockStyle.Fill
            };
            //this.Controls.Add(browser);
            panelVideo.Controls.Add(browser);
            ChromeDevToolsSystemMenu.CreateSysMenu(this);
            browser.RegisterJsObject("winformObj", new JsInteractionHandler());
            //LoadHtml("Z25ibgtwQa0");


        }

        public void LoadHtml(string VideoId)
        {
            string html = ScriptReader("YouTubeEmbed.html");
            var htmlFormated = html.Replace("**VideoId**", VideoId);
            browser.LoadHtml(htmlFormated);
        }

        public void btnGo_Click(object sender, EventArgs e)
        {
            var selectedVideo = comboBox1.SelectedValue.ToString();
            var selectedVideoId = selectedVideo.Split('=')[1];
            LoadHtml(selectedVideoId);

//            webBrowser1.AllowWebBrowserDrop = false;
//            webBrowser1.IsWebBrowserContextMenuEnabled = false;
//            webBrowser1.WebBrowserShortcutsEnabled = false;
//            webBrowser1.ObjectForScripting = this;
//            //webBrowser1.ObjectForScripting = new ScriptInterface();
//            // Uncomment the following line when you are finished debugging.
//            //webBrowser1.ScriptErrorsSuppressed = true;

            //            var html = ScriptReader("YouTubeEmbed.html");
            //            webBrowser1.DocumentText = @"<html>
            //            <head>
            //            <meta content='IE=Edge' http-equiv='X-UA-Compatible'/>
            //            <script>function popWebMessageBox(message) { alert(message); }</script>
            //            </head>
            //  <body>
            //<button onclick = ""window.external.PopWinFormsMessageBox('Called from the embeded webpage!')"" > call client code from script code</button>
            //  <iframe id=""existing - iframe - example""
            //        width = ""640"" height = ""360""
            //        src = ""https://www.youtube.com/embed/M7lc1UVf-VE?enablejsapi=1""
            //        frameborder = ""0""
            //        style = ""border: solid 4px #37474F"" ></ iframe >
            //    < script>
            //        window.onerror = function(message, url, lineNumber) 
            //        { 
            //          window.external.errorHandler(message, url, lineNumber);
            //        }
            //        var tag = document.createElement('script');
            //            tag.id = 'iframe-demo';
            //            tag.src = 'https://www.youtube.com/iframe_api';
            //            var firstScriptTag = document.getElementsByTagName('script')[0];
            //            firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

            //            var player;
            //            function onYouTubeIframeAPIReady()
            //            {
            //                player = new YT.Player('existing-iframe-example', {
            //        events: {
            //                'onReady': onPlayerReady,
            //          'onStateChange': onPlayerStateChange
            //        }
            //        });
            //  }
            //    function onPlayerReady(event)
            //    {
            //        document.getElementById('existing-iframe-example').style.borderColor = '#FF6D00';
            //    }
            //    function changeBorderColor(playerStatus)
            //    {
            //        var color;
            //        if (playerStatus == -1)
            //        {
            //            color = ""#37474F""; // unstarted = gray
            //        }
            //        else if (playerStatus == 0)
            //        {
            //            color = ""#FFFF00""; // ended = yellow
            //        }
            //        else if (playerStatus == 1)
            //        {
            //            color = ""#33691E""; // playing = green
            //        }
            //        else if (playerStatus == 2)
            //        {
            //            color = ""#DD2C00""; // paused = red
            //        }
            //        else if (playerStatus == 3)
            //        {
            //            color = ""#AA00FF""; // buffering = purple
            //        }
            //        else if (playerStatus == 5)
            //        {
            //            color = ""#FF6DOO""; // video cued = orange
            //        }
            //        if (color)
            //        {
            //            document.getElementById('existing-iframe-example').style.borderColor = color;
            //        }
            //    }
            //    function onPlayerStateChange(event)
            //    {
            //        changeBorderColor(event.data);
            //    }
            //</script>
            //  </body>
            //</html>";


        }

        //This WinForms button when clicked invokes a script in the embeded webpages "popWebMessageBox" function that pops a message box
        public void button1_Click(object sender, EventArgs e)
        {
            //webBrowser1.Document.InvokeScript("popWebMessageBox",
            //new String[] { "Called from WinForms app!" });
        }

        //The script code in the embeded web page calls this method using "onclick=\"window.external.PopWinFormsMessageBox('Called from the embeded webpage!')\">" 
        public void PopWinFormsMessageBox(String message)
        {
            MessageBox.Show(message, "WinForms Message Box");
        }

        private string ScriptReader(string fileName)
        {
            string script = null;
            string resourcePath = "UCExtend.HTML." + fileName;

            if (resourceFiles.Contains(resourcePath))
            {
                //Resource resource file as preference
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))

                using (StreamReader reader = new StreamReader(stream))
                {
                    script = reader.ReadToEnd();
                }
            }

            return script;
        }

        [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        public class ScriptInterface
        {
            void errorHandler(string message, string url, string lineNumber)
            {
                MessageBox.Show(string.Format("Message: {0}, URL: {1}, Line: {2}"));
            }
        }

        private void btnDevTools_Click(object sender, EventArgs e)
        {
           browser.ShowDevTools();
        }

        //Dev tools menu option overide
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if ((m.Msg == ChromeDevToolsSystemMenu.WM_SYSCOMMAND) && ((int)m.WParam == ChromeDevToolsSystemMenu.SYSMENU_CHROME_DEV_TOOLS))
            {
                browser.ShowDevTools();
            }
        }

        private void btnCtoJ_Click(object sender, EventArgs e)
        {
            //var script = "document.body.style.backgroundColor = 'red';";
            var script = "playFullscreen();";//"document.body.playFullscreen()";
            browser.ExecuteScriptAsync(script);
        }
    }

   
}




//webBrowser1.DocumentText =
//            @"<html>
//            <head>
//            <meta content='IE=Edge' http-equiv='X-UA-Compatible'/>
//            <script>function popWebMessageBox(message) { alert(message); }</script>
//            </head>
//            <body>
//            <script type = 'text/javascript' src='http://www.youtube.com/player_api'></script>
//            <iframe id='video' src='http://www.youtube.com/embed/Z25ibgtwQa0?autoplay=1&controls=1&enablejsapi=1' width='100%' height='650' frameborder='0' allowfullscreen='allowfullscreen'></iframe>
//            <script type = 'text/javascript' >
//            var player;
//            function onYouTubeIframeAPIReady()
//            {
//                alert('test 1');
//                player = new YT.Player('player', 
//                {
//                    videoId: 'VIDEO_ID', events: 
//                    {
//                        'onStateChange': function(evt)
//                        {
//                            if (evt.data == 0)
//                            {
                                
//                                alert('test 2');
//                                window.external.PopWinFormsMessageBox('Video finished!!');
//                            }
//                        }
//                    }
//                });
//            }
//            </script>
//            </body></html>";

//string html = "<html><head>";
//html += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
//html += "<iframe id='video' src='https://www.youtube.com/embed/{0}' width='100%' height='650' frameborder='0' allowfullscreen='allowfullscreen'></iframe>";
//html += "</body></html>";
//?rel=0&autoplay=1
//rel='0' autoplay='1'
//allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture'
//html += @"< iframe width = '560' height = '315' src = 'https://www.youtube.com/embed/{0}' frameborder = '0' allow = 'accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen ></ iframe >";



//webBrowser1.DocumentText = string.Format(html, selectedVideoId);

//This creates an HTML button that when clicked executes javascript which triggers the "PopWinFormsMessageBox" method in C#
//webBrowser1.DocumentText =
//"<html><head><script>" +
//"function popWebMessageBox(message) { alert(message); }" +
//"</script></head><body><button " +
//"onclick=\"window.external.PopWinFormsMessageBox('Called from the embeded webpage!')\">" +
//"call client code from script code</button>" +
//"</body></html>";

//            <iframe id = 'player' width='100%' height='100%' src='http://www.youtube.com/embed/Zgq7y2Bvb9U?autoplay=1&controls=0&enablejsapi=1' frameborder='0' allowfullscreen></iframe>


//var embed = "<html><head>" +
//            "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
//            "</head><body>" +
//            "<iframe width=\"300\" src='https://www.youtube.com/embed/{0}'" +
//            "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
//            "</body></html>";
//this.webBrowser1.DocumentText = string.Format(embed, txtUrl.Text.Split('=')[1]);


//string html = "https://www.youtube.com/embed/{0}";
// this.webBrowser1.Url = string.Format(html, txtUrl.Text.Split('=')[1]);

//webBrowser1.DocumentText = String.Format("<html><head>" +
//        "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
//        "</head><body style=\"margin: 0px; padding: 0px; overflow: hidden\">" +
//        "<iframe style = \"overflow: hidden; height: 100%; width: 100%\" width=\"100%\" height=\"100%\" src=\"https://www.youtube.com/embed/{0}?rel=0&autoplay=1&controls=1&autohide=2\"" +
//        "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
//        "</body></html>", "Qf0RdWIWtm8");


//https://www.youtube.com/embed/qRv7G7WpOoU?controls=1&autohide=2
