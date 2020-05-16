using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fisheye2EquirectangularForm
{
    public partial class MainForm : Form
    {
        static int count = 0;
        public MainForm()
        {
            InitializeComponent();
        }

        private void openFisheyeButton_Click(object sender, EventArgs e)
        {
            if(openFisheyeFileDialog.ShowDialog() == DialogResult.OK)
            {
                fisheyeImg.ImageLocation = openFisheyeFileDialog.FileName;
            }
        }

        private void saveEquirectButton_Click(object sender, EventArgs e)
        {
            if (openFisheyeFileDialog.FileName.Length == 0 || !File.Exists(openFisheyeFileDialog.FileName))
            {
                MessageBox.Show("No File Selected !");
                return;
            }

            if (saveEquirectFileDialog.ShowDialog() == DialogResult.OK)
            {
                // TODO
                FisheyeUtil.saveFisheye2equirectangular(openFisheyeFileDialog.FileName, Decimal.ToInt32(angleNumericUpDown.Value), saveEquirectFileDialog.FileName, mode360CheckBox.Checked);
//                Fisheye2Equirectangular.saveFisheye2equirectangular(openFisheyeFileDialog.FileName, Decimal.ToInt32(angleNumericUpDown.Value), saveEquirectFileDialog.FileName, mode360CheckBox.Checked);
                equirectImg.ImageLocation = saveEquirectFileDialog.FileName;
            }
        }

        private void convertDirectoryButton_Click(object sender, EventArgs e)
        {
            if(fisheyeFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var files = Directory.GetFiles(fisheyeFolderBrowserDialog.SelectedPath,"*.jpg");
                if (files.Length > 0)
                {
                    var outdir = fisheyeFolderBrowserDialog.SelectedPath + "\\VR";
                    if (!Directory.Exists(outdir))
                    {
                        Directory.CreateDirectory(outdir);
                    }
                    count = 0;
                    foreach (var item in files)
                    {
                        var outfile = string.Format("{0}\\{1}", outdir,Path.GetFileName(item));
                        Task.Run(() => {
                            FisheyeUtil.saveFisheye2equirectangular(item, Decimal.ToInt32(angleNumericUpDown.Value), outfile, mode360CheckBox.Checked);
                            count++;
                            convertCountToolStripStatusLabel.Text = string.Format("{0}/{1}",count, files.Length);
                        });
                    }
                }

//                var str = files.Length > 0 ? files[0] : "";
//                convertCountToolStripStatusLabel.Text = string.Format("-/{0}:{1}", files.Length,str);
//                MessageBox.Show(fisheyeFolderBrowserDialog.SelectedPath);
            }
        }

        private void convertMovieButton_Click(object sender, EventArgs e)
        {
            if (openMovieFileDialog.ShowDialog() == DialogResult.OK)
            {
                var dir = Path.GetDirectoryName(openMovieFileDialog.FileName);
                var outdir = dir + "\\VR";
                if (!Directory.Exists(outdir))
                {
                    Directory.CreateDirectory(outdir);
                }
                var outfile = string.Format("{0}\\{1}", outdir, Path.GetFileName(openMovieFileDialog.FileName));
                Task.Run(() => {
                    var video = new VideoCapture(openMovieFileDialog.FileName);
                    var size = new OpenCvSharp.Size();
                    var len = video.FrameHeight > video.FrameWidth ? video.FrameHeight : video.FrameWidth;
                    size.Height = len;
                    size.Width = mode360CheckBox.Checked ? len * 2 : len;
                    var writer = new VideoWriter(outfile, FourCC.DIVX, video.Fps, size);
//                    var writer = new VideoWriter(outfile, FourCC.DIVX, video.Fps, new OpenCvSharp.Size(video.FrameWidth,video.FrameHeight));
                    var total = video.FrameCount;
                    var cnt = 0;
                    for (int i = 0; i < video.FrameCount; i++)
                    {
                        var frame = new Mat();
                        if (video.Read(frame))
                        {
                            var f = Fisheye2Equirectangular.fisheye2equirectangular(frame, Decimal.ToInt32(angleNumericUpDown.Value), mode360CheckBox.Checked);
                            // 変換して書き込み
                            writer.Write(f);
                            //writer.Write(frame);
                            //Cv2.ImWrite(writer.FileName+".jpg", frame);
                            //break;
                            cnt++;
                            convertCountToolStripStatusLabel.Text = string.Format("{0}/{1}:{2}", cnt, total, writer.FileName);
                        }
                        else
                        {
                            break;
                        }
                        frame.Dispose();
                    }
                    /*
                    while (video.IsOpened())
                    {
                        var frame = new Mat();
                        if (video.Read(frame))
                        {
                            var f = Fisheye2Equirectangular.fisheye2equirectangular(frame, Decimal.ToInt32(angleNumericUpDown.Value), mode360CheckBox.Checked);
                            // 変換して書き込み
                            writer.Write(f);
                            cnt++;
                            convertCountToolStripStatusLabel.Text = string.Format("{0}/{1}:{2}",cnt,total,outfile);
                        }
                        else
                        {
                            break;
                        }
                        frame.Dispose();
                    }
                    */
                    video.Dispose();
                    writer.Dispose();
                    //Fisheye2Equirectangular.saveFisheyeMovie2equirectangular(openMovieFileDialog.FileName, Decimal.ToInt32(angleNumericUpDown.Value), outfile, mode360CheckBox.Checked);
                });
                //fisheyeImg.ImageLocation = openFisheyeFileDialog.FileName;
            }
        }
    }
}
