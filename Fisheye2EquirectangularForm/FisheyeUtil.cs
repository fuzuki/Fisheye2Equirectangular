using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fisheye2EquirectangularForm
{
    static class FisheyeUtil
    {
        private static System.Drawing.Point findCorrespondingFisheyePoint(int Xe, int Ye, int We, int He, float FOV)
        {
            var fisheyePoint = new System.Drawing.Point();
            float theta, phi, r;
            //Point3f sphericalPoint = new Point3f();

            theta = (float)(Math.PI * (Xe / ((float)We) - 0.5));
            phi = (float)(Math.PI * (Ye / ((float)He) - 0.5));

            var sphericalPointX = Math.Cos(phi) * Math.Sin(theta);
            var sphericalPointY = Math.Cos(phi) * Math.Cos(theta);
            var sphericalPointZ = Math.Sin(phi);

            theta = (float)Math.Atan2(sphericalPointZ, sphericalPointX);
            phi = (float)Math.Atan2(Math.Sqrt(Math.Pow(sphericalPointX, 2) + Math.Pow(sphericalPointZ, 2)), sphericalPointY);
            r = ((float)We) * phi / FOV;

            fisheyePoint.X = (int)(0.5 * ((float)We) + r * Math.Cos(theta));
            fisheyePoint.Y = (int)(0.5 * ((float)He) + r * Math.Sin(theta));

            return fisheyePoint;
        }

        /// <summary>
        /// 入力画像を正方形の中心に配置して返す
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private static Bitmap rect2Square(System.Drawing.Bitmap src)
        {
            if (src.Height == src.Width)
            {
                return src;
            }
            var len = src.Height > src.Width ? src.Height : src.Width;

            var ret = new Bitmap(len, len, PixelFormat.Format24bppRgb);
            var dstRect = new Rectangle();
            if (src.Height > src.Width)
            {
                dstRect.X = (src.Height - src.Width) / 2;
                dstRect.Y = 0;
            }
            else
            {
                dstRect.X = 0;
                dstRect.Y = (src.Width - src.Height) / 2;
            }


            using (var g = Graphics.FromImage(ret))
            {
                dstRect.Width = src.Width;
                dstRect.Height = src.Height;
                g.DrawImage(src, dstRect, 0, 0, src.Width, src.Height, GraphicsUnit.Pixel);
            }
            return ret;
        }

        /// <summary>
        /// 入力魚眼画像を正距円筒画像に変換する。
        /// </summary>
        /// <param name="fisheyeImage"></param>
        /// <returns></returns>
        public static Bitmap fisheye2equirectangular(Bitmap fisheyeImage, int angle)
        {
            if (fisheyeImage == null)
            {
                return null;
            }
            float FOV;

            var squareFisheye = rect2Square(fisheyeImage);
            var sideLen = squareFisheye.Width;

            var equirectangularImage = new Bitmap(sideLen, sideLen,PixelFormat.Format24bppRgb);

            FOV = (angle * (float)Math.PI) / 180F;

            for (int Xe = 0; Xe < sideLen; Xe++)
            {
                for (int Ye = 0; Ye < sideLen; Ye++)
                {
                    var fisheyePoint = findCorrespondingFisheyePoint(Xe, Ye, sideLen, sideLen, FOV);

                    if (fisheyePoint.X >= sideLen || fisheyePoint.Y >= sideLen)
                        continue;

                    if (fisheyePoint.X < 0 || fisheyePoint.Y < 0)
                        continue;
                    equirectangularImage.SetPixel(Xe, Ye, squareFisheye.GetPixel(fisheyePoint.X, fisheyePoint.Y));
                }
            }

            return equirectangularImage;
        }
        public static Bitmap fisheye2equirectangular(string path, int angle)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            try
            {
                var img = new Bitmap(path);
                return fisheye2equirectangular(img, angle);
            }
            catch (Exception e)
            {
                // 例外が発生した場合はnullを返す
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 指定の魚眼画像を変換して、指定のパスに保存する。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="outpath"></param>
        public static void saveFisheye2equirectangular(string path, int angle, string outpath, bool mode360)
        {
            var equirectangularImage = fisheye2equirectangular(path, angle);
            if (equirectangularImage == null)
            {
                return;
            }
            try
            {
                if (mode360)
                {
                    //equirectangularImageは正方形
                    var len = equirectangularImage.Width;
                    var ret = new Bitmap(len*2, len, PixelFormat.Format24bppRgb);
                    var dstRect = new Rectangle();

                    using (var g = Graphics.FromImage(ret))
                    {
                        dstRect.X = len / 2;
                        dstRect.Width = equirectangularImage.Width;
                        dstRect.Height = equirectangularImage.Height;
                        g.DrawImage(equirectangularImage, dstRect, 0, 0, equirectangularImage.Width, equirectangularImage.Height, GraphicsUnit.Pixel);
                    }
                    equirectangularImage = ret;

                    // TODO メタデータも設定するとよい
                    //https://www.facebook.com/notes/eric-cheng/editing-360-photos-injecting-metadata/10156930564975277/
                }
                equirectangularImage.Save(outpath, ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //return null;
            }

            return;

        }
    }
}
