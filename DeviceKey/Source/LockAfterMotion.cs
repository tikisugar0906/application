using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace DeviceKey
{
    public partial class LockAfterMotion : Form
    {
        public LockAfterMotion()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 画面を読み込んだ時
        /// </summary>
        private void LockAfterMotion_Load(object sender, EventArgs e)
        {
            this.MotionLabel.Text = LoadMotionName() + "まで";  // 設定した動作の名前

            LoadMotionTimer();  // 動作タイマーの時間を読み込む

            // カウントダウンを開始する
            MotionTimer = new Timer();
            MotionTimer.Tick += new EventHandler(CountDown);
            MotionTimer.Interval = 1000;
            MotionTimer.Start();
        }



        /// <summary>
        /// 動作の名前を読み込む処理
        /// </summary>
        /// <returns></returns>
        static string LoadMotionName()
        {
            // 動作ラジオボタンの状態を書き込んだMotionName.txtを読み込む
            StreamReader Mloader = new StreamReader("MotionName.txt", Encoding.GetEncoding("UTF-8"));  // ファイルを開く、文字コード
            string Mloadtext = Mloader.ReadToEnd();   // ファイルから読み込み
            Mloader.Close(); // ファイルを閉じる

            return Mloadtext;
        }



        /// <summary>
        /// 動作タイマーの時間を読み込む処理
        /// </summary>
        /// <returns>stringからintに直した時間テキスト</returns>
        static int LoadMotionTimer()
        {
            // 動作タイマーを何秒にするかを書き込んだMotionTimer.txtを読み込む
            StreamReader MTloader = new StreamReader("MotionTimer.txt", Encoding.GetEncoding("UTF-8"));  // ファイルを開く、文字コード
            string MTloadtext = MTloader.ReadToEnd();   // ファイルから読み込み
            MTloader.Close(); // ファイルを閉じる

            int time = int.Parse(MTloadtext);   // stringテキストをintに直す
            return time;
        }



        private int duration = LoadMotionTimer();   // durationを読み込んだ時間にする



        /// <summary>
        /// カウントダウン処理
        /// </summary>
        private void CountDown(object sender, EventArgs e)
        {
            if (duration == 0)  // durationが0になった時
            {
                MotionTimer.Stop(); // タイマーをストップする

                if (LoadMotionName() == "ロック(Windows)")
                {
                    Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                }
                else if (LoadMotionName() == "スリープ")
                {
                    Application.SetSuspendState(PowerState.Suspend, false, false);
                }
                else if (LoadMotionName() == "ログオフ")
                {
                    Process ShutDownProc = new Process();
                    ShutDownProc.StartInfo.FileName = "shutdown.exe";
                    ShutDownProc.StartInfo.Arguments = "/l";
                    ShutDownProc.Start();
                }
                else if (LoadMotionName() == "シャットダウン")
                {
                    Process ShutDownProc = new Process();
                    ShutDownProc.StartInfo.FileName = "shutdown.exe";
                    ShutDownProc.StartInfo.Arguments = "/s";
                    ShutDownProc.Start();
                }

                this.Close();   // この画面を閉じる
            }
            else if (duration > 0)  // durationが0以外の時
            {
                duration--; // durationを減らしていく
                CountLabel.Text = duration.ToString();  // テキストに今の数値を表示する
            }
        }

    }
}
