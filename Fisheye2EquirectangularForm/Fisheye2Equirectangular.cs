using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fisheye2EquirectangularForm
{
    static class Fisheye2Equirectangular
    {

        private static Point2f findCorrespondingFisheyePoint(int Xe, int Ye, int We, int He, float FOV)
        {
            Point2f fisheyePoint = new Point2f();
            float theta, phi, r;
            Point3f sphericalPoint = new Point3f();

            theta = (float)(Math.PI * (Xe / ((float)We) - 0.5));
            phi = (float)(Math.PI * (Ye / ((float)He) - 0.5));

            sphericalPoint.X = (float)(Math.Cos(phi) * Math.Sin(theta));
            sphericalPoint.Y = (float)(Math.Cos(phi) * Math.Cos(theta));
            sphericalPoint.Z = (float)Math.Sin(phi);

            theta = (float)Math.Atan2(sphericalPoint.Z, sphericalPoint.X);
            phi = (float)Math.Atan2(Math.Sqrt(Math.Pow(sphericalPoint.X, 2) + Math.Pow(sphericalPoint.Z, 2)), sphericalPoint.Y);
            r = ((float)We) * phi / FOV;

            fisheyePoint.X = (int)(0.5 * ((float)We) + r * Math.Cos(theta));
            fisheyePoint.Y = (int)(0.5 * ((float)He) + r * Math.Sin(theta));

            return fisheyePoint;
        }

        /// <summary>
        /// 長方形の画像に余白を加えて、正方形の画像を返す
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static Mat rect2Square(Mat img)
        {
            int h = img.Size().Height;
            int w = img.Size().Width;
            Mat ret = null;
            var iList = new Mat[3];
            iList[1] = img;
            if (h == w)
            {
                return img;
            }
            else if (h > w)
            {
                // 縦長
                var margin = new Mat(new Size((h - w) / 2, h), img.Type());
                ret = new Mat(h, h, img.Type());
                iList[0] = margin;
                iList[2] = margin;
                // 必要なら黒で塗りつぶす
                Cv2.HConcat(iList, ret);

            }
            else
            {
                // 横長
                var margin = new Mat(new Size(w, (w - h) / 2), img.Type());
                ret = new Mat(w, w, img.Type());
                iList[0] = margin;
                iList[2] = margin;
                Cv2.VConcat(iList, ret);
            }

            return ret;
        }

        /// <summary>
        /// 入力魚眼画像を正距円筒画像に変換する。
        /// </summary>
        /// <param name="fisheyeImage"></param>
        /// <returns></returns>
        public static Mat fisheye2equirectangular(Mat fisheyeImage, int angle)
        {
            if (fisheyeImage == null)
            {
                return null;
            }
            Mat equirectangularImage, tImg;
            //int Wf, Hf;
            float FOV;
            //int We, He;

            var squareFisheye = rect2Square(fisheyeImage);
            var sideLen = squareFisheye.Size().Width;

            tImg = new Mat(new Size(sideLen, sideLen), fisheyeImage.Type());
            Cv2.Transpose(squareFisheye, tImg);
            Cv2.Flip(tImg, tImg, FlipMode.X);

            FOV = (angle * (float)Math.PI) / angle;

            equirectangularImage = new Mat();
            equirectangularImage.Create(sideLen, sideLen, fisheyeImage.Type());

            for (int Xe = 0; Xe < sideLen; Xe++)
            {
                for (int Ye = 0; Ye < sideLen; Ye++)
                {
                    Point2f fisheyePoint = findCorrespondingFisheyePoint(Xe, Ye, sideLen, sideLen, FOV);

                    if (fisheyePoint.X >= sideLen || fisheyePoint.Y >= sideLen)
                        continue;

                    if (fisheyePoint.X < 0 || fisheyePoint.Y < 0)
                        continue;

                    equirectangularImage.Set<Vec3b>(Xe, Ye, tImg.At<Vec3b>((int)fisheyePoint.X, (int)fisheyePoint.Y));
                }
            }

            // 回転
            tImg = new Mat(new Size(sideLen, sideLen), fisheyeImage.Type());
            Cv2.Transpose(equirectangularImage, tImg);
            Cv2.Flip(tImg, tImg, FlipMode.Y);

            return tImg;
        }

        public static Mat fisheye2equirectangular(string path, int angle)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            try
            {
                var img = Cv2.ImRead(path);
                return fisheye2equirectangular(img,angle);
            }
            catch (Exception)
            {
                // 例外が発生した場合はnullを返す
                return null;
            }
        }

        /// <summary>
        /// 指定の魚眼画像を変換して、指定のパスに保存する。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="outpath"></param>
        /// <returns></returns>
        public static Mat saveFisheye2equirectangular(string path,int angle, string outpath)
        {
            var equirectangularImage = fisheye2equirectangular(path,angle);
            if (equirectangularImage == null)
            {
                return null;
            }

            try
            {
                Cv2.ImWrite(outpath, equirectangularImage);
            }
            catch (Exception)
            {
                return null;
            }

            return equirectangularImage;
        }
        public static System.Drawing.Bitmap saveFisheye2equirectangularBitmap(string path,int angle, string outpath)
        {
            return mat2bmp(saveFisheye2equirectangular(path,angle, outpath));
        }

        public static System.Drawing.Bitmap mat2bmp(Mat img)
        {
            if (img == null)
            {
                return null;
            }
            var bmp = new System.Drawing.Bitmap(img.Cols, img.Rows, (int)img.Step(), System.Drawing.Imaging.PixelFormat.Format24bppRgb, img.Ptr());

            return bmp;
        }
    }
}
