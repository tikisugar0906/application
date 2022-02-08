using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DeviceKey
{
    public partial class LockWindow : Form
    {
        delegate void DelegateProcess();    // delegateを宣言

        MyHook myHook = new MyHook();

        public string Pass { get; set; }    // SettingFormから送られてきたパスワード

        public LockWindow()
        {
            InitializeComponent();

            myHook.Hook();  // キーボードフックを開始
        }



        /// <summary>
        /// 画面を開いた時
        /// </summary>
        private void LockWindow_Load(object sender, EventArgs e)
        {
            CheckDeviceFirst(); // 初回の接続デバイス確認処理
        }



        /// <summary>
        /// パスワード判別処理
        /// </summary>
        private void PassMotion()
        {
            if (PassTextBox.Text == string.Format(Pass))    // 入力した値が設定画面で設定したパスワードと一致した時
            {
                this.Invoke(new Action(() => { this.Close(); })); // この画面を閉じる
            }
        }



        /// <summary>
        /// パスワードテキストボックスでキーを押した時
        /// </summary>
        private void PassTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)    // Enterを押した時
            {
                PassMotion();   // パスワード判別
            }
        }



        /// <summary>
        /// OKボタンを押した時
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            PassMotion();   // パスワード判別
        }


        /// <summary>
        /// フォームが閉じる時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            myHook.HookEnd();   //キーボードフックを終了する
        }


        #region/**---------------------------------------- 処理一覧：ここから ----------------------------------------**/

        #region/*-------------------- デバイス取得処理：ここから --------------------*/
        public const int WM_DEVICECHANGE = 0x00000219;  // デバイス変化のWindowsイベントの値
        List<USBDeviceInfo> usbDevicesBefore = new List<USBDeviceInfo>();   // USBデバイスがあるかを確認するためのリスト
        int numBeforeDevices = 0;   // 初期デバイス数は0

        /// <summary>
        /// 初回の接続デバイス確認処理。選択デバイスが接続されている状態でボタンを押してもウィンドウを即座に閉じない。
        /// </summary>
        private void CheckDeviceFirst()
        {
            var usbDevices = GetUSBDevices();   // デバイス取得リストの内容をusbDevicesオブジェクトとする

            // デバイス数が増加（接続）
            if (usbDevices.Count > numBeforeDevices)
            {
                foreach (var usbDevice in usbDevices)
                {
                    // 取得したデバイスの中で、前回の一覧に無いものを検出し、コンボボックスに送る
                    bool bExistDevice = false;  // デバイスがあるか無いかのフラグ

                    // usbDevicesBeforeリストから情報を取得して
                    // usbDeviceBeforeオブジェクトとして使うループ
                    foreach (var usbDeviceBefore in usbDevicesBefore)
                    {
                        if (usbDevice.DeviceID == usbDeviceBefore.DeviceID) //取得したデバイスがリストの中にある時
                        {
                            bExistDevice = true;    // リストに「ある」フラグにして何もしないままブレイク
                            break;
                        }
                    }
                    // リストに「無い」(接続された)フラグの場合
                    if (!bExistDevice)
                    {
                        // 選択したデバイスが接続された時にする動作
                        StreamReader SDloader = new StreamReader("SelectDevice.txt", Encoding.GetEncoding("UTF-8"));  // ファイルを開く、文字コード
                        string SDloadtext = SDloader.ReadToEnd();   // ファイルから読み込み
                        SDloader.Close(); // ファイルを閉じる
                        if (usbDevice.DeviceID == SDloadtext)   // 取得したデバイスが選択したデバイスだった時
                        {
                            // 選択したデバイスが接続していることをSelectDeviceConnect.txtに書き込む
                            StreamWriter SDCwriter = new StreamWriter("SelectDeviceConnect.txt", false, Encoding.GetEncoding("UTF-8")); // ファイルを開く、上書き、文字コード
                            SDCwriter.Write("Yes"); // 書き込み
                            SDCwriter.Close(); // ファイルを閉じる
                        }
                    }
                }
            }

            // デバイス数が減少（取り外し）※ロック画面が表示している時(例：ロックボタンを押した後)に選択デバイスを取り外した場合
            else if (usbDevices.Count < numBeforeDevices)
            {
                foreach (var usbDeviceBefore in usbDevicesBefore)
                {
                    // 前回のデバイスの中で、今回取得した一覧に無いものを検出し、画面から削除
                    bool bExistDevice = false;  // デバイスがあるか無いかのフラグ

                    // usbDevicesリストから情報を取得して
                    // usbDeviceオブジェクトとして使うループ
                    foreach (var usbDevice in usbDevices)
                    {
                        if (usbDevice.DeviceID == usbDeviceBefore.DeviceID) //取得したデバイスがリスト内にある時
                        {
                            bExistDevice = true;     //リストに「ある」フラグにし、何もしないままブレイク
                            break;
                        }
                    }
                    // リストに「無い」(切断された)フラグの場合
                    if (!bExistDevice)
                    {
                        // 選択したデバイスが切断された時にする動作
                        StreamReader SDloader = new StreamReader("SelectDevice.txt", Encoding.GetEncoding("UTF-8"));  // ファイルを開く、文字コード
                        string SDloadtext = SDloader.ReadToEnd();   // ファイルから読み込み
                        SDloader.Close(); // ファイルを閉じる
                        if (usbDeviceBefore.DeviceID == SDloadtext)   // 切断されたデバイスが選択したデバイスだった時
                        {
                            // 選択したデバイスが接続していないことをSelectDeviceConnect.txtに書き込む
                            StreamWriter SDCwriter = new StreamWriter("SelectDeviceConnect.txt", false, Encoding.GetEncoding("UTF-8")); // ファイルを開く、上書き、文字コード
                            SDCwriter.Write("No"); // 書き込み
                            SDCwriter.Close(); // ファイルを閉じる

                            this.Invoke(new Action(() => { this.Close(); })); // この画面を閉じる ※画面が2つ表示されているため1つ閉じる
                        }
                        
                    }
                }
            }
            usbDevicesBefore = usbDevices;  // デバイス一覧を更新
            numBeforeDevices = usbDevices.Count;    // デバイス数をカウント
        }


        /// <summary>
        /// メインの接続デバイス確認処理。選択デバイスが接続された時にウィンドウを閉じる。
        /// </summary>
        private void CheckDevice()
        {
            var usbDevices = GetUSBDevices();   // デバイス取得リストの内容をusbDevicesオブジェクトとする

            // デバイス数が増加（接続）
            if (usbDevices.Count > numBeforeDevices)
            {
                foreach (var usbDevice in usbDevices)
                {
                    // 取得したデバイスの中で、前回の一覧に無いものを検出し、コンボボックスに送る
                    bool bExistDevice = false;  // デバイスがあるか無いかのフラグ

                    // usbDevicesBeforeリストから情報を取得して
                    // usbDeviceBeforeオブジェクトとして使うループ
                    foreach (var usbDeviceBefore in usbDevicesBefore)
                    {
                        if (usbDevice.DeviceID == usbDeviceBefore.DeviceID) //取得したデバイスがリストの中にある時
                        {
                            bExistDevice = true;    // リストに「ある」フラグにして何もしないままブレイク
                            break;
                        }
                    }
                    // リストに「無い」(接続された)フラグの場合
                    if (!bExistDevice)
                    {
                        // 選択したデバイスが接続された時にする動作
                        StreamReader SDloader = new StreamReader("SelectDevice.txt", Encoding.GetEncoding("UTF-8"));  // ファイルを開く、文字コード
                        string SDloadtext = SDloader.ReadToEnd();   // ファイルから読み込み
                        SDloader.Close(); // ファイルを閉じる
                        if (usbDevice.DeviceID == SDloadtext)   // 取得したデバイスが選択したデバイスだった時
                        {
                            // 選択したデバイスが接続していることをSelectDeviceConnect.txtに書き込む
                            StreamWriter SDCwriter = new StreamWriter("SelectDeviceConnect.txt", false, Encoding.GetEncoding("UTF-8")); // ファイルを開く、上書き、文字コード
                            SDCwriter.Write("Yes"); // 書き込み
                            SDCwriter.Close(); // ファイルを閉じる

                            this.Invoke(new Action(() => { this.Close(); })); // この画面を閉じる
                        }
                    }
                }
            }

            // デバイス数が減少（取り外し）※ロック画面が表示している時(例：ロックボタンを押した後)に選択デバイスを取り外した場合
            else if (usbDevices.Count < numBeforeDevices)
            {
                foreach (var usbDeviceBefore in usbDevicesBefore)
                {
                    // 前回のデバイスの中で、今回取得した一覧に無いものを検出し、画面から削除
                    bool bExistDevice = false;  // デバイスがあるか無いかのフラグ

                    // usbDevicesリストから情報を取得して
                    // usbDeviceオブジェクトとして使うループ
                    foreach (var usbDevice in usbDevices)
                    {
                        if (usbDevice.DeviceID == usbDeviceBefore.DeviceID) //取得したデバイスがリスト内にある時
                        {
                            bExistDevice = true;     //リストに「ある」フラグにし、何もしないままブレイク
                            break;
                        }
                    }
                    // リストに「無い」(切断された)フラグの場合
                    if (!bExistDevice)
                    {
                        // 選択したデバイスが切断された時にする動作
                        StreamReader SDloader = new StreamReader("SelectDevice.txt", Encoding.GetEncoding("UTF-8"));  // ファイルを開く、文字コード
                        string SDloadtext = SDloader.ReadToEnd();   // ファイルから読み込み
                        SDloader.Close(); // ファイルを閉じる
                        if (usbDeviceBefore.DeviceID == SDloadtext)   // 切断されたデバイスが選択したデバイスだった時
                        {
                            // 選択したデバイスが接続していないことをSelectDeviceConnect.txtに書き込む
                            StreamWriter SDCwriter = new StreamWriter("SelectDeviceConnect.txt", false, Encoding.GetEncoding("UTF-8")); // ファイルを開く、上書き、文字コード
                            SDCwriter.Write("No"); // 書き込み
                            SDCwriter.Close(); // ファイルを閉じる

                            this.Invoke(new Action(() => { this.Close(); })); // この画面を閉じる ※画面が2つ表示されているため1つ閉じる
                        }

                    }
                }
            }
            usbDevicesBefore = usbDevices;  // デバイス一覧を更新
            numBeforeDevices = usbDevices.Count;    // デバイス数をカウント
        }


        /// <summary>
        /// 接続されているデバイスを取得する処理
        /// </summary>
        /// <returns>デバイス情報のリスト</returns>
        static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();
            ManagementObjectCollection collection;
            // WMIライブラリのWin32_USBHubクラスを使用して管理情報を取得
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            // 取得したUSBHub情報から、ID情報を取り出し、デバイスリストに追加
            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                    (string)device.GetPropertyValue("DeviceID"),    //デバイスID
                    (string)device.GetPropertyValue("Description")  //デバイスの説明文
                    ));
            }
            collection.Dispose();
            return devices;
        }


        /// <summary>
        /// Windows メッセージを処理する関数（オーバーライド）
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_DEVICECHANGE:   //デバイス状況の変化イベント
                    // AddMessage(m.ToString() + Environment.NewLine);
                    Task.Run(() => CheckDevice());      //デバイスをチェック
                    break;
            }
        }


        /// <summary>
        /// デバイス情報を格納するクラス
        /// </summary>
        class USBDeviceInfo
        {
            public USBDeviceInfo(string deviceID, string description)
            {
                this.DeviceID = deviceID;
                this.Description = description;
            }
            public string DeviceID { get; private set; }
            public string Description { get; private set; }
        }
        #endregion/*-------------------- デバイス取得処理：ここまで --------------------*/



        #region/*-------------------- キーボードフック処理：ここから --------------------*/
        /// <summary>
        /// キーボードフックを行うクラス
        /// </summary>
        class MyHook
        {
            delegate int delegateHookCallback(int nCode, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

            static extern IntPtr SetWindowsHookEx(int idHook, delegateHookCallback lpfn, IntPtr hMod, uint dwThreadId);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]

            static extern bool UnhookWindowsHookEx(IntPtr hhk);
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

            static extern IntPtr GetModuleHandle(string lpModuleName);
            IntPtr hookPtr = IntPtr.Zero;

            //フック処理
            public void Hook()
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    // フックを行う
                    hookPtr = SetWindowsHookEx(
                        // 第1引数   フックするイベントの種類。13はキーボードフックを表す
                        13,
                        // 第2引数 フック時のメソッドのアドレス。フックメソッドを登録する
                        HookCallback,
                        // 第3引数   インスタンスハンドル。現在実行中のハンドルを渡す
                        GetModuleHandle(curModule.ModuleName),
                        // 第4引数   スレッドID。0を指定すると、すべてのスレッドでフックされる
                        0
                    );
                }
            }


            /// <summary>
            /// フックした特定のキーを捨てて入力禁止にする
            /// </summary>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns>0で入力、1で捨てる</returns>
            int HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                // フックしたキー
                Keys v = (Keys)(short)Marshal.ReadInt32(lParam);
                if (
                    v == Keys.LWin ||
                    v == Keys.RWin ||
                    v == Keys.LControlKey ||
                    v == Keys.RControlKey ||
                    v == Keys.LMenu ||
                    v == Keys.RMenu ||
                    v == Keys.Escape ||
                    v == Keys.PrintScreen ||
                    v == Keys.F1 ||
                    v == Keys.F2 ||
                    v == Keys.F3 ||
                    v == Keys.F4 ||
                    v == Keys.F5 ||
                    v == Keys.F6 ||
                    v == Keys.F7 ||
                    v == Keys.F8 ||
                    v == Keys.F9 ||
                    v == Keys.F10 ||
                    v == Keys.F11 ||
                    v == Keys.F12
                    )
                {
                    // 1を戻すとフックしたキーが捨てられます
                    return 1;
                }
                else
                {
                    // 0を戻すとフックしたキーが入力される
                    return 0;
                }

            }


            /// <summary>
            /// キーボードフックを終了する処理
            /// </summary>
            public void HookEnd()
            {
                UnhookWindowsHookEx(hookPtr);
                hookPtr = IntPtr.Zero;
            }
        }
        #endregion/*-------------------- キーボードフック処理：ここまで --------------------*/

        #endregion/**---------------------------------------- 処理一覧：ここまで ----------------------------------------**/
    }
}
