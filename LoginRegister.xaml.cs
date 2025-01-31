﻿using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.SmallBasic.Library;


namespace CStudy
{
    /// <summary>
    /// LoginRegister.xaml の相互作用ロジック
    /// </summary>
    /// <summary>
    /// Page1.xaml の相互作用ロジック
    /// </summary>
    /// 
    public partial class LoginRegister : Page
    {

        public LoginRegister()
        {
            InitializeComponent();//おまじない
            Sound.Play(@"C:\Users\CStudy\MUSIC\LoginRegister.mp3");//ログイン画面のBGMを再生
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            string Which_Select = ((Button)sender).Name.ToString();//ログインか登録どちらのボタンが押されたか。ボタンの名前をWhich_Selectに代入
            switch (Which_Select)///Which_Slectの値が以下なら
            {
                case "Button_Select_Login"://ログインボタンが押されたら
                    Label_UserID.Visibility = Visibility.Visible;//ログインのGUIアイテムを表示----------------------------------------------↓
                    TextBox_UserID.Visibility = Visibility.Visible;
                    Label_Password.Visibility = Visibility.Visible;
                    PasswordBox_Password.Visibility = Visibility.Visible;
                    Label_PasswordConfirm.Visibility = Visibility.Hidden;
                    PasswordBox_PasswordConfirm.Visibility = Visibility.Hidden;
                    Button_Login.Visibility = Visibility.Visible;
                    Button_Register.Visibility = Visibility.Hidden;
                    break;//--------------------------------------------------------------------------------------------------------------↑
                case "Button_Select_Register"://登録ボタンが押されたら
                    Label_UserID.Visibility = Visibility.Visible;//登録のGUIアイテムを表示--------------------------------------------------↓
                    TextBox_UserID.Visibility = Visibility.Visible;
                    Label_Password.Visibility = Visibility.Visible;
                    PasswordBox_Password.Visibility = Visibility.Visible;
                    Label_PasswordConfirm.Visibility = Visibility.Visible;
                    PasswordBox_PasswordConfirm.Visibility = Visibility.Visible;
                    Button_Login.Visibility = Visibility.Hidden;
                    Button_Register.Visibility = Visibility.Visible;
                    break;//---------------------------------------------------------------------------------------------------------------↑
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)////Loginボタンが押されたら
        {
            string UserID = TextBox_UserID.Text;//入力されたIDを変数に代入
            string Password = PasswordBox_Password.Password;//入力されたパスワードを変数に代入
            string Path_Userdata = @"./data\user\" + UserID;//UserIDからデータベースの保存パスを作成
            if (UserID != "")///ユーザーIDが入力されていたら
            {
                if (new DirectoryInfo(Path_Userdata).Exists)///ディレクトリデータ(ユーザデータ).存在するか　真：↑　偽：↓
                {
                    Path_Userdata += @"\password.CStudy";//passwordファイルへのアクセスパスを作成
                    StreamReader ReadPassword = new StreamReader(Path_Userdata, Encoding.GetEncoding("Shift_JIS"));//パスワードファイルパスと、入力形式をReadPasswordに定義
                    string Check_Password = ReadPassword.ReadLine();//Cjeck_PassWordにパスワードを読み込んで代入。
                    if (Password == Check_Password)///入力されたパスワードと保存されていたパスワードが一致したら　真：↑　偽：↓
                    {
                        Method_Nowuser(UserID);//ログインした人のIDをファイルに保存する(Method_Nowuser)
                        NavigationService.Navigate(new ModeSelect());//モード選択画面に遷移
                    }
                    else
                    {
                        MessageBox.Show("不正なログイン情報です。");//ログインエラーを出力
                    }
                }
                else
                {
                    MessageBox.Show("不正なログイン情報です。");//ログインエラーを出力
                }
            }
            else
            {
                MessageBox.Show("不正なログイン情報です。");//ログインエラーを出力
            }
        }


        private void RegisterButton_Click(object sender, RoutedEventArgs e)////Registerボタンが押されたら
        {
            string UserID = TextBox_UserID.Text;//入力されたIDを変数に代入
            string Password = PasswordBox_Password.Password.ToString();//入力されたパスワードを変数に代入
            string PasswordConfirm = PasswordBox_PasswordConfirm.Password.ToString();//入力された再度入力パスワードを変数に代入
            if (UserID == "")//UserIDが入力されていなかったら
            {
                MessageBox.Show("UserIDが入力されていません。");//登録エラーを表示
            }
            if (Password == "")///パスワードが入力されていなかったら
            {
                MessageBox.Show("Passwordが入力されていません。");//登録エラーを表示
            }
            else
            {
                if (Password == PasswordConfirm)///入力されたパスワードが一致していたら　真：↑　偽：↓
                {
                    string Path_Savedata = @"./data\user\" + UserID + @"\save.CStudy";//セーブデータ保存ファイルのファイルパスを定義
                    string Path_Userdata = @"./data\user\" + UserID;//ユーザディレクトリ作成用ファイルパスを定義
                    if (new DirectoryInfo(Path_Userdata).Exists)///すでにユーザーディレクトリが存在していたら
                    {
                        MessageBox.Show("すでに存在するUserIDです。");//既存ユーザー在りのエラーを出力
                    }
                    ///すでにユーザディレクトリが存在していたら　偽：↓
                    else
                    {
                        Directory.CreateDirectory(Path_Userdata);//UserIDと同値のディレクリを作成
                        Path_Userdata += (@"\password.CStudy");//パスワードファイルのパスを定義
                        System.IO.File.AppendAllText(Path_Userdata, Password);//パスワードファイルにパスワードを保存
                        System.IO.File.AppendAllText(Path_Savedata, "0");
                        Method_Nowuser(UserID);//登録されたユーザーのUserIDを外部ファイルに保存(Method_Nowuser)
                        NavigationService.Navigate(new ModeSelect());//モード選択画面に遷移
                    }
                }
                else///パスワードが不一致だったら
                {
                    MessageBox.Show("パスワードが一致していません。");//パスワード不一致を表示
                }
            }
        }

        public void Method_Nowuser(string UserID)////メソッドNowuser
        {
            string Path_NowUser = @"./data\NowUser.CStudy";//NowUserのファイルパスを定義
            System.IO.File.Delete(Path_NowUser);//NowUserファイルを削除
            System.IO.File.AppendAllText(Path_NowUser, UserID);//新しくNowUserファイルを作成し、ログイン・登録されたUserIDを保存
        }

        private void Exit_Click(object sender, RoutedEventArgs e)////Exitボタンが押されたら
        {
            Application.Current.Shutdown();//アプリケーションを終了する。
        }
    }
}
