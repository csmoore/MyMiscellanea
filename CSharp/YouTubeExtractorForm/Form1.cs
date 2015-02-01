using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExtractor;

namespace YouTubeExtractorForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            labelStatus.Text = "Select URL";
        }

        private void buttonPasteFromClipboard_Click(object sender, EventArgs e)
        {
            editUrl.Text = Clipboard.GetText().Trim();
            if (String.IsNullOrEmpty(editUrl.Text))
                editUrl.Focus();
        }

        private string getFileName(string defaultFileName)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            dialog.FileName = defaultFileName;

            if (dialog.ShowDialog() == DialogResult.OK)            
                return dialog.FileName;            
            else
                return null;
        }

        // Rest of file adapted from example at:
        // From: https://github.com/flagbug/YoutubeExtractor/blob/master/YoutubeExtractor/ExampleApplication/Program.cs

        private static string removeIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private void downloadAudio(IEnumerable<VideoInfo> videoInfos)
        {
            /*
             * We want the first extractable video with the highest audio quality.
             */
            VideoInfo video = videoInfos
                .Where(info => info.CanExtractAudio)
                .OrderByDescending(info => info.AudioBitrate)
                .First();

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the audio downloader.
             * The first argument is the video where the audio should be extracted from.
             * The second argument is the path to save the audio file.
             */

            string defaultFileName = removeIllegalPathCharacters(video.Title) + video.AudioExtension;
            string fileName = getFileName(defaultFileName);

            var audioDownloader = new AudioDownloader(video, fileName);

            progressBar.Value = 0;

            labelStatus.Text = "Downloading - " + video.Title;
            labelStatus.Refresh();

            // Register the progress events. We treat the download progress as 85% of the progress
                // and the extraction progress only as 15% of the progress, because the download will
                // take much longer than the audio extraction.
            audioDownloader.DownloadProgressChanged += (sender, args) => { progressBar.Value = (int)(args.ProgressPercentage * 0.85); };
            audioDownloader.AudioExtractionProgressChanged += (sender, args) => { progressBar.Value = (85 + (int)(args.ProgressPercentage * 0.15)); };

            /*
             * Execute the audio downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            audioDownloader.Execute();

            progressBar.Value = 0;
            labelStatus.Text = "Complete.";
        }

        private void buttonDownloadAudio_Click(object sender, EventArgs e)
        {
            // const string link = "https://www.youtube.com/watch?v=B0jMPI_pUec"; //  http://www.youtube.com/watch?v=O3UBOOZw-FE";

            labelStatus.Text = "Starting...";

            string link = editUrl.Text.Trim();

            if (string.IsNullOrEmpty(link))
            {
                MessageBox.Show("Please Enter URL", "Missing URL");
                return;
            }

            /*
             * Get the available video formats.
             * We'll work with them in the video and audio download examples.
             */
            try
            {
                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link, false);
                downloadAudio(videoInfos);          
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could note contact URL", "Bad URL");
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
