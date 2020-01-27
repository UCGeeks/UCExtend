using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCExtend.VideoTraining
{
    class JsInteractionHandler
    {
        public void ShowWinFormsMessageBox(string msg)
        {
            MessageBox.Show(msg);
        }

        public void PlayerStateChanged(int obj)
        {
            if (obj == -1)
            {
                // unstarted = gray
            }
            else if (obj == 0)
            {
                PlaybackEnded(); // ended = yellow
            }
            else if (obj == 1)
            {
                // playing = green
            }
            else if (obj == 2)
            {
                 // paused = red
            }
            else if (obj == 3)
            {
                // buffering = purple
            }
            else if (obj == 5)
            {
                 // video cued = orange
            }
        }

        public void PlaybackEnded()
        {
            MessageBox.Show("Congrats, you've completed a training video!");
        }
    }
}
