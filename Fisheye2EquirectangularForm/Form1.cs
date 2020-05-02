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
                Fisheye2Equirectangular.saveFisheye2equirectangular(openFisheyeFileDialog.FileName, Decimal.ToInt32(angleNumericUpDown.Value), saveEquirectFileDialog.FileName);
                equirectImg.ImageLocation = saveEquirectFileDialog.FileName;
            }
        }
    }
}
