using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Renci.SshNet;
using System.Security;
using System.Net;
using System.ComponentModel;
using System.Net.Mime;

namespace OpenRPA.PMTech.SSH
{
    [Designer(typeof(ActivityDesigner))]
    [DisplayName("Run Command")]
    public sealed class RunCommandActivity : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Host { get; set; }
        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> Port { get; set; }
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> User { get; set; }
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Password { get; set; }
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Command { get; set; }

        [Category("Output")]
        public OutArgument<string> Result { get; set; }
        [Category("Output")]
        public OutArgument<int> ExitStatus { get; set; }
        [Category("Output")]
        public OutArgument<string> Error { get; set; }


        protected override void Execute(CodeActivityContext context)
        {

            // 引数を取得
            string host = context.GetValue(this.Host);
            int port = context.GetValue(this.Port);
            string user = context.GetValue(this.User);
            string password = context.GetValue(this.Password);
            string command = context.GetValue(this.Command);

            // 接続情報を作成
            ConnectionInfo con = new ConnectionInfo(host, port, user, ProxyTypes.None, null, 0, null, null,
                new AuthenticationMethod[] {
                        new PasswordAuthenticationMethod(user, password) });

            SshClient ssh = null;
            try
            {
                // 接続
                ssh = new SshClient(con);
                ssh.Connect();

                // コマンドを実行
                SshCommand cmd = ssh.RunCommand("ls -l");
                String result = cmd.Execute();

                // 実行結果を戻り値に設定
                context.SetValue(Result, cmd.Result);
                context.SetValue(ExitStatus, cmd.ExitStatus);
                context.SetValue(Error, cmd.Error);

                // 切断
                ssh.Disconnect();

            }
            catch (Exception e)
            {
                if (ssh != null && ssh.IsConnected)
                {
                    ssh.Disconnect();
                }
                throw e;
            }
            finally
            {
                ssh = null;
            }

        }
    }
}
