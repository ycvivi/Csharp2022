using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace myApplication1
{
    public partial class MainForm : Form
    {
        static string[] DelStr = new string[] { "a", "bb", "cccc" };
        public MainForm()
        {
            InitializeComponent();
        }
        /// <summary>
        ///直接删除指定目录下的所有文件及文件夹(保留目录)
        /// </summary>
        /// <param name="strPath">文件夹路径</param>
        /// <returns>执行结果</returns>

        public static void DeleteDir(string file,int iDelFlag)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {

                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {

                        if (File.Exists(f))
                        {
                            Thread.Sleep(2);
                            //如果有子文件删除文件
                            if(iDelFlag==1)
                                File.Delete(f);
                            //Console.WriteLine(f);
                        }
                        else
                        {
                            for (int i = 0; i < DelStr.GetLength(0);i++)
                                if (f.IndexOf(DelStr[i]) != -1)
                                {
                                     //循环递归删除子文件夹
                                    DeleteDir(f,1);
                                    //Console.WriteLine("字符串中含有@，其出现的位置是{0}", str.IndexOf("@") + 1);
                                }
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(file);

                }

            }
            catch (Exception ex) // 异常处理
            {
                Console.WriteLine(ex.Message.ToString());// 异常信息
            }

        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            //textBox1.Text.ToString();
            DeleteDir(textBox1.Text.ToString(),0);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            textBox1.Text = path; //将获取到的完整路径赋值到textBox1
        }
        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
	     	//拖拽的文件路径
            string fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            MessageBox.Show("接收到文件 {fileName}");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;//允许窗体Drop事件 注意默认是false(也就是不能进行拖文件进入操作）
        }
    }
}
