using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
namespace Downloader
{
    public partial class Interface : Form
    {
        
        List<Download> downloads;

        public Interface()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            //Validates listbox for items
            if (lstLinks.Items.Count <= 0)
            {
                MessageBox.Show("Provide me a link please!");
                return;
            }

            /*
            *  Note: i'm not commenting anything which involves the Download class, it will be explained later 
            */

            //This list contains all downloads which will be used later
            downloads = new List<Download>();
          
            //defines which value is the maximum one for our progressbar
            progDownload.Maximum = lstLinks.Items.Count;
            
            for (int i = 0; i < lstLinks.Items.Count; i++)
                downloads.Add(new Download(lstLinks.Items[i].ToString()));
            
                

            btnAdd.Enabled = false;
            btnDownload.Enabled = false;

            if(downloads.Count > 0)
                Downloader.RunWorkerAsync(Downloader);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //checks for validity of the url then adds it to listbox
            if (Download.isValidUrl(txtLink.Text))
            {
                lstLinks.Items.Add(txtLink.Text);
            }
            else
            {
                MessageBox.Show("The URL is invalid", "Oops!");
            }
                  
        }

        private void Downloader_DoWork(object sender, DoWorkEventArgs e)
        {
            Downloader.WorkerReportsProgress = true;
            WebClient client = new WebClient();

            for (int i = 0; i < downloads.Count; i++)
            {
                string link = downloads[i].downloadLink;
                string filename = downloads[i].deriveFileName();

                //if something went wrong in getting the filename, the link must not be a downloadlink
                if (filename == null)
                {
                    MessageBox.Show("Invalid download link");
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
                Downloader.ReportProgress((i / downloads.Count)*100, downloads.Count);
                
            }
        
        }

        private void Downloader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //show the progress of the backgroundworker in a progressbar
            Int32 arg = (Int32)e.UserState;
            progDownload.Value = arg;
        }

        private void Downloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnAdd.Enabled = true;
            btnDownload.Enabled = true;
            MessageBox.Show(
                "Downloads are completed",
                "Notification",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
