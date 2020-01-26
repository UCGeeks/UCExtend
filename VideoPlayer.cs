using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Passing script between WinForms app and embeded browser::
////https://stackoverflow.com/questions/14172273/youtube-embedded-player-event-when-video-has-ended
////https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/implement-two-way-com-between-dhtml-and-client?redirectedfrom=MSDN
////https://www.htmlgoodies.com/beyond/video/respond-to-embedded-youtube-video-events.html

namespace UCExtend
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class VideoPlayer : Form
    {
        public VideoPlayer()
        {
            InitializeComponent();
        }

        //Video settings
        public static string settingsFolderBase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Application.CompanyName + @"\" + Application.ProductName;
        public static string pathVideoPlaylist = settingsFolderBase + @"\VideoPlayList.txt";
        public static string pathVideoPlayWatchList = settingsFolderBase + @"\VideoWatchList.txt";
        //static string settingsTemplateFilePath = Application.StartupPath + @"\SettingsTemplate.xml";

        //List<List<string>> videoPlaylist = new List<List<string>>();
        //List<List<string>> videoWatchlist = new List<List<string>>();
        List<string> videoPlaylist = new List<string>();
        List<string> videoWatchlist = new List<string>();

        public void btnGo_Click(object sender, EventArgs e)
        {
            var selectedVideo = comboBox1.SelectedValue.ToString();
            var selectedVideoId = selectedVideo.Split('=')[1];

            webBrowser1.AllowWebBrowserDrop = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;
            webBrowser1.ObjectForScripting = this;
            // Uncomment the following line when you are finished debugging.
            //webBrowser1.ScriptErrorsSuppressed = true;

           
            webBrowser1.DocumentText =
            @"<html>
            <head>
            <meta content='IE=Edge' http-equiv='X-UA-Compatible'/>
            <script>function popWebMessageBox(message) { alert(message); }</script>
            </head>
            <body>
            <script type = 'text/javascript' src='http://www.youtube.com/player_api'></script>
            <iframe id='video' src='http://www.youtube.com/embed/Z25ibgtwQa0?autoplay=1&controls=1&enablejsapi=1' width='100%' height='650' frameborder='0' allowfullscreen='allowfullscreen'></iframe>
            <script type = 'text/javascript' >
            var player;
            function onYouTubeIframeAPIReady()
            {
                alert('test 1');
                player = new YT.Player('player', 
                {
                    videoId: 'VIDEO_ID', events: 
                    {
                        'onStateChange': function(evt)
                        {
                            if (evt.data == 0)
                            {
                                
                                alert('test 2');
                                window.external.PopWinFormsMessageBox('Video finished!!');
                            }
                        }
                    }
                });
            }
            </script>
            </body></html>";

        }

        public void VideoPlayer_Load(object sender, EventArgs e)
        {
            if (File.Exists(pathVideoPlaylist))
            {
                var lines = System.IO.File.ReadAllLines(pathVideoPlaylist);
                foreach (var line in lines)
                {
                    var split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    //videoPlaylist.Add(split.ToList());
                    videoPlaylist.Add(line);
                }
                comboBox1.DataSource = videoPlaylist;
                //comboBox1.DisplayMember = 

                if (File.Exists(pathVideoPlayWatchList))
                {
                    lines = System.IO.File.ReadAllLines(pathVideoPlayWatchList);
                    foreach (var line in lines)
                    {
                        var split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        //videoWatchlist.Add(split.ToList());
                        videoWatchlist.Add(line);
                    }
                }
                else
                {
                    File.Create(pathVideoPlayWatchList);
                }
            }
            else
            {
                MessageBox.Show("No video playlist found!");
            }
        }

        //This WinForms button when clicked invokes a script in the embeded webpages "popWebMessageBox" function that pops a message box
        public void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("popWebMessageBox",
            new String[] { "Called from WinForms app!" });
        }

        //The script code in the embeded web page calls this method using "onclick=\"window.external.PopWinFormsMessageBox('Called from the embeded webpage!')\">" 
        public void PopWinFormsMessageBox(String message)
        {
            MessageBox.Show(message, "WinForms Message Box");
        }
    }
}



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
