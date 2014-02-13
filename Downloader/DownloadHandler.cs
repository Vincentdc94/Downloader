using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;


namespace Downloader
{
    
    public class DownloadHandler
    {
        private List<Download> downloads;
        private Interface intform;
        public Thread downloadThread; 

        public DownloadHandler(Interface form)
        {
            intform = form;
        }

        public void startDownload(int item_count, string[] items)
        {
            //This list contains all downloads which will be used later
            downloads = new List<Download>();

            for (int i = 0; i < item_count; i++)
                downloads.Add(new Download(items[i]));

            if (downloads.Count > 0)
            {
                downloadThread = new Thread(doDownload);
                downloadThread.Start();   
            }
        }

        public void doDownload()
        {
            WebClient client = new WebClient();
            int i;

            for (i = 0; i < downloads.Count; i++)
            {
                string link = downloads[i].downloadLink;
                string filename = downloads[i].deriveFileName();

                //if something went wrong in getting the filename, the link must not be a downloadlink
                if (filename == null)
                {
                    MessageBox.Show("Invalid download link" + filename);
                    break;
                }

                //try downloading the file
                try
                {
                    client.DownloadFile(link, filename);
                }
                catch (Exception)
                {
                    MessageBox.Show("File could not be downloaded for numerous reasons");
                    break;
                }

                //report progress of the backgroundworker

                intform.Invoke(intform.updateProgressDelegate, new object[]{i});

            }
            intform.Invoke(intform.updateProgressDelegate, new object[] {i});
        }
    }
}
