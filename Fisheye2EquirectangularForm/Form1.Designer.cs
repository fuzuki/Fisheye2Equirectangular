namespace Fisheye2EquirectangularForm
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.fisheyeImg = new System.Windows.Forms.PictureBox();
            this.equirectImg = new System.Windows.Forms.PictureBox();
            this.openFisheyeButton = new System.Windows.Forms.Button();
            this.saveEquirectButton = new System.Windows.Forms.Button();
            this.openFisheyeFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveEquirectFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.angleNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.fisheyeImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.equirectImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.angleNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // fisheyeImg
            // 
            this.fisheyeImg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.fisheyeImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fisheyeImg.Location = new System.Drawing.Point(13, 48);
            this.fisheyeImg.Name = "fisheyeImg";
            this.fisheyeImg.Size = new System.Drawing.Size(390, 390);
            this.fisheyeImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.fisheyeImg.TabIndex = 0;
            this.fisheyeImg.TabStop = false;
            // 
            // equirectImg
            // 
            this.equirectImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.equirectImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.equirectImg.Location = new System.Drawing.Point(418, 48);
            this.equirectImg.Name = "equirectImg";
            this.equirectImg.Size = new System.Drawing.Size(390, 390);
            this.equirectImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.equirectImg.TabIndex = 1;
            this.equirectImg.TabStop = false;
            // 
            // openFisheyeButton
            // 
            this.openFisheyeButton.Location = new System.Drawing.Point(12, 12);
            this.openFisheyeButton.Name = "openFisheyeButton";
            this.openFisheyeButton.Size = new System.Drawing.Size(113, 23);
            this.openFisheyeButton.TabIndex = 2;
            this.openFisheyeButton.Text = "open Fisheye";
            this.openFisheyeButton.UseVisualStyleBackColor = true;
            this.openFisheyeButton.Click += new System.EventHandler(this.openFisheyeButton_Click);
            // 
            // saveEquirectButton
            // 
            this.saveEquirectButton.Location = new System.Drawing.Point(132, 11);
            this.saveEquirectButton.Name = "saveEquirectButton";
            this.saveEquirectButton.Size = new System.Drawing.Size(141, 23);
            this.saveEquirectButton.TabIndex = 3;
            this.saveEquirectButton.Text = "save Equirectangular";
            this.saveEquirectButton.UseVisualStyleBackColor = true;
            this.saveEquirectButton.Click += new System.EventHandler(this.saveEquirectButton_Click);
            // 
            // openFisheyeFileDialog
            // 
            this.openFisheyeFileDialog.Filter = "魚眼jpegファイル|*.jpg";
            // 
            // saveEquirectFileDialog
            // 
            this.saveEquirectFileDialog.Filter = "jpegファイル|*.jpg";
            // 
            // angleNumericUpDown
            // 
            this.angleNumericUpDown.Location = new System.Drawing.Point(743, 15);
            this.angleNumericUpDown.Maximum = new decimal(new int[] {
            230,
            0,
            0,
            0});
            this.angleNumericUpDown.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.angleNumericUpDown.Name = "angleNumericUpDown";
            this.angleNumericUpDown.Size = new System.Drawing.Size(65, 19);
            this.angleNumericUpDown.TabIndex = 5;
            this.angleNumericUpDown.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 450);
            this.Controls.Add(this.angleNumericUpDown);
            this.Controls.Add(this.saveEquirectButton);
            this.Controls.Add(this.openFisheyeButton);
            this.Controls.Add(this.equirectImg);
            this.Controls.Add(this.fisheyeImg);
            this.MinimumSize = new System.Drawing.Size(836, 489);
            this.Name = "MainForm";
            this.Text = "Fisheye2Equirectangular";
            ((System.ComponentModel.ISupportInitialize)(this.fisheyeImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.equirectImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.angleNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox fisheyeImg;
        private System.Windows.Forms.PictureBox equirectImg;
        private System.Windows.Forms.Button openFisheyeButton;
        private System.Windows.Forms.Button saveEquirectButton;
        private System.Windows.Forms.OpenFileDialog openFisheyeFileDialog;
        private System.Windows.Forms.SaveFileDialog saveEquirectFileDialog;
        private System.Windows.Forms.NumericUpDown angleNumericUpDown;
    }
}

