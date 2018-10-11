using Accord.Video.FFMPEG;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlExDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        public int selectedDeviceIndex = 0;

        private VideoFileWriter videoWriter = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            GetDevices();
            VideoConnect(0);
            videoSourcePlayer.VideoSource = videoSource;
            videoSourcePlayer.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            videoSourcePlayer.Stop();
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            GrabBitmap(Directory.GetCurrentDirectory());
        }


        public FilterInfoCollection GetDevices()
        {
            try
            {
                //枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count != 0)
                {
                    //LogClass.WriteFile("已找到视频设备.");
                    return videoDevices;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                //LogClass.WriteFile("error:没有找到视频设备！具体原因：" + ex.Message);
                return null;
            }
        }

        /// </p><summary>
        /// 连接视频摄像头
        /// </summary>
        /// <param name="deviceIndex">
        /// <param name="resolutionIndex">
        /// <returns></returns>
        public VideoCaptureDevice VideoConnect(int deviceIndex = 0, int resolutionIndex = 0)
        {
            if (videoDevices.Count <= 0)
                return null;
            selectedDeviceIndex = deviceIndex;
            videoSource = new VideoCaptureDevice(videoDevices[deviceIndex].MonikerString);
            return videoSource;
        }

        //抓图，拍照，单帧
        public void GrabBitmap(string path)
        {
            //if (videoSource == null)
            // return;
            //g_Path = path;

            stopREC = false;
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            videoSourcePlayer.NewFrame += VideoSourcePlayer_NewFrame;
        }

        private bool stopREC = false;
        private bool createNewFile = true;
        private string videoFileFullPath = string.Empty; //视频文件全路径
        private string imageFileFullPath = string.Empty; //图像文件全路径
        private string videoPath = @"E:\video\"; //视频文件路径
        private string imagePath = @"E:\video\images\"; //图像文件路径
        private string videoFileName = string.Empty; //视频文件名
        private string imageFileName = string.Empty; //图像文件名
        private string drawDate = string.Empty;
        int frameRate = 20; //默认帧率

        private void VideoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            if (stopREC)
            {
                stopREC = true;
                createNewFile = true;  //这里要设置为true表示要创建新文件
                if (videoWriter != null)
                    videoWriter.Close();
            }
            else
            {
                //开始录像
                if (createNewFile)
                {
                    if (!Directory.Exists(videoPath))
                        Directory.CreateDirectory(videoPath);

                    videoFileName = DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss") + ".avi";
                    videoFileFullPath = videoPath + videoFileName;
                    createNewFile = false;
                    if (videoWriter != null)
                    {
                        videoWriter.Close();
                        videoWriter.Dispose();
                    }
                    videoWriter = new VideoFileWriter();
                    //这里必须是全路径，否则会默认保存到程序运行根据录下了
                    videoWriter.Open(videoFileFullPath, image.Width, image.Height, frameRate, VideoCodec.MPEG4);
                    videoWriter.WriteVideoFrame(image);
                }
                else
                {
                    videoWriter.WriteVideoFrame(image);
                }
            }

            //videoSourcePlayer.NewFrame -= new AForge.Controls.VideoSourcePlayer.NewFrameHandler(VideoSourcePlayer_NewFrame);
        }

        void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
            string fullPath = Directory.GetCurrentDirectory() + "\\" + "temp\\";
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            string img = fullPath + DateTime.Now.ToString("yyyyMMdd hhmmss") + ".bmp";
            bmp.Save(img);

            videoSource.NewFrame -= new NewFrameEventHandler(videoSource_NewFrame);
        }
    }
}
