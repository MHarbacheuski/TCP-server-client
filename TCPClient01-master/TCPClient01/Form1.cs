using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCPClient01
{
    public partial class Form1 : Form
    {
        TcpClient mTcpClient;
        byte[] mRx =new byte[1024 * 8];//буффер
        byte[] tx = new byte[1024 * 8];
        int count = 0;

        public Form1()
        {
            InitializeComponent();
            progressBar1.Value = 0;
        }

        //функция для соединения с сервером
        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress ipa;//предоставление ip адреса
            int nPort;//порт для прослушивания 

            try
            {
                if (string.IsNullOrEmpty(tbServerIP.Text) || string.IsNullOrEmpty(tbServerPort.Text))
                    return;
                if (!IPAddress.TryParse(tbServerIP.Text, out ipa))
                {
                    MessageBox.Show("Please supply an IP Address.");
                    return;
                }

                if (!int.TryParse(tbServerPort.Text, out nPort))
                {
                    nPort = 23000;
                }

                mTcpClient = new TcpClient();
                mTcpClient.BeginConnect(ipa, nPort, onCompleteConnect, mTcpClient);//Начинает асинхронный запрос на подключение к удаленному хосту.             
                tbConsole.Invoke(new Action(() => tbConsole.Text = "Connected"));               
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        void onCompleteConnect(IAsyncResult iar)
        {
            TcpClient tcpc;            
            try
            {
                tcpc = (TcpClient)iar.AsyncState;
                tcpc.EndConnect(iar);//Завершает ожидающую асинхронную попытку на подключение
                mRx = new byte[1024 * 8];
                tcpc.GetStream().BeginRead(mRx, 0, mRx.Length, onCompleteReadFromServerStream, tcpc);//начинается чтение данных

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        //функция чтения данных полученных с сервера
        void onCompleteReadFromServerStream(IAsyncResult iar)
        {
            TcpClient tcpc=new TcpClient();
            int nCountBytesReceivedFromServer;//Количество байтов, полученных от сервера

            try
            {
                tcpc.NoDelay = true;
                tcpc = (TcpClient)iar.AsyncState;
                nCountBytesReceivedFromServer = tcpc.GetStream().EndRead(iar);//обработка завершения асинхронного чтения

                if (nCountBytesReceivedFromServer == 0)
                {
                    MessageBox.Show("Connection broken.");
                    return;
                }


                if (mRx[0] == 42)//если 1-ый байт является байтом со значением 42, то работаем с этим массивом как с массивом байтов
                {                 
                    for (int i = 0; i < mRx.Length; i++)
                    {                     
                        if (mRx[i] == 0 && mRx[i + 1] == 0 && mRx[i + 2] == 0)// если 3 подряд значения массива байтов равна 0, то удаляем находим общее колличество этих нулей
                        {
                            count = mRx.Length - i;
                            break;
                        }
                    }
                    Array.Resize(ref mRx, mRx.Length - count);//обрезаем нули, которые нам не нужны
                    int[] newmassiv = new int[mRx.Length];// формируем новый массив значений
                    count = 0;//обнуляем счётчик
                    string message = "";
                    
                    for (int i = 0; i < mRx.Length - 1; i++)//записываем значение каждого элемента массива в строку через пробел, пропуская первый элемент массива
                    {
                        newmassiv[i] = mRx[i+1];
                        message += newmassiv[i] + " ";
                    }
                    
                    printLine(" Bytes : " + message);//вывод информации на экран
                    mRx = new byte[1024 * 8];
                }
                else
                {
                    
                    printLine(" Text : " + Encoding.UTF8.GetString(mRx, 0, nCountBytesReceivedFromServer).ToString());
                    mRx = new byte[1024 * 8];
                }
                
                tcpc.GetStream().BeginRead(mRx, 0, mRx.Length, onCompleteReadFromServerStream, tcpc);
               

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void printLine(string _strPrint)
        {
            tbConsole.Invoke(new Action<string>(doInvoke), _strPrint);
        }

        public void doInvoke(string _strPrint)
        {
            tbConsole.Text = _strPrint + Environment.NewLine + tbConsole.Text;
        }

        //функция отправления сообщения серверу
        private void tbSend_Click(object sender, EventArgs e)
        {
            byte[] tx;//буффер для отправки
            
            if (string.IsNullOrEmpty(tbPayload.Text)) return;

            try
            {
                tx = Encoding.UTF8.GetBytes(tbPayload.Text);//преобразование Text в byte

                if (mTcpClient != null)
                {
                    if (mTcpClient.Client.Connected)
                    {
                        mTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToServer, mTcpClient);//отправляем сообщение
                        MessageBox.Show("Сообщение отправлено!");
                    }
                }
                tbPayload.Text = "";
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        //функция для отправки информации серверу
        void onCompleteWriteToServer(IAsyncResult iar)
        {
            TcpClient tcpc;

            try
            {
                tcpc = (TcpClient)iar.AsyncState;
                tcpc.GetStream().EndWrite(iar);//обработка завершения асинхронной записи
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        //функция отправления массива байт серверу
        private void tbSendBytes_Click(object sender, EventArgs e)
        {
          
            try
            {
              
                if (mTcpClient != null)
                {
                    if (mTcpClient.Client.Connected)
                    {
                        for (int i = Convert.ToInt32(num1.Text), j = 0; j <= Convert.ToInt32(num3.Text) + 1; i++, j++)
                        {
                            if (i > 255)//если значение i больше чем 255, то находим остаток от деления, и приравниваем этот остаток к i
                            {
                                i %= 256;
                            }
                            if (j == 0)//заполняем наш первый элемен массива символом *(42)
                            {
                                tx[0] = 42;
                                i = Convert.ToInt32(num1.Text) - 1;
                                continue;
                            }
                            tx[j] = (byte)(i);//заполняем массив байтов различными значениями
                            i += Convert.ToInt32(num2.Text) - 1;//увеличиваем шаг

                        }

                        for (int i = 0; i < tx.Length; i++)
                        {
                            if (tx[i] == 0 && tx[i + 1] == 0 && tx[i + 2] == 0)// если 3 подряд значения массива байтов равна 0, то удаляем находим общее количество этих нулей
                            {
                                count = tx.Length - i;
                                break;
                            }
                        }
                        Array.Resize(ref tx, tx.Length - count);//обрезаем нули, которые нам не нужны
                        count = 0;
                        mTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToServer, mTcpClient);//отправляем сообщение
                        tx = new byte[1024 * 8];
                        MessageBox.Show("Сообщение отправлено!");
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            byte[] tx;//буффер для отправки

           // if (string.IsNullOrEmpty(tbPayload.Text)) return;

            try
            {
                tx = Encoding.UTF8.GetBytes("TIME");//преобразование Text в byte

                if (mTcpClient != null)
                {
                    if (mTcpClient.Client.Connected)
                    {
                        mTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToServer, mTcpClient);//отправляем сообщение
                        MessageBox.Show("Сообщение отправлено!");
                    }
                }
                tbPayload.Text = "";
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }
        string fileName = "";
        int fileNameSize = 0;
        string filePath = "";
        long size = 0;
        private  void SendFile_Click(object sender, EventArgs e)
        {
            byte[] packege = new byte[100000];
            
            string command = "DOWNLOAD";

            byte[] comand = Encoding.UTF8.GetBytes(command);
            comand.CopyTo(packege, 0);
            //Array.Copy(packege, 0, comand, 0, fileNameSize);
            BitConverter.GetBytes(fileNameSize).CopyTo(packege, 8);
            Array.Copy(Encoding.UTF8.GetBytes(fileName), 0, packege, 12, Encoding.UTF8.GetBytes(fileName).Length);

            BitConverter.GetBytes(size).CopyTo(packege, 12 + fileNameSize);

            byte[] data = File.ReadAllBytes(filePath);
            data.CopyTo(packege, 12 + fileNameSize + 4);
            //Array.Copy(packege, 12 + 4, data, 0, fileNameSize);
            mTcpClient.GetStream().BeginWrite(packege, 0, packege.Length, onCompleteWriteToServer, mTcpClient);//отправляем сообщение





        }
    

        private void Browse_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            //openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            //openFileDialog1.FilterIndex = 1;

            //openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == DialogResult.OK)
            {
                tbConsole.Text = openFileDialog1.FileName;
                fileName = Path.GetFileName(openFileDialog1.FileName);
                fileNameSize = fileName.Length;
                filePath = openFileDialog1.FileName;

                size = new FileInfo(openFileDialog1.FileName).Length;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] tx;//буффер для отправки

            // if (string.IsNullOrEmpty(tbPayload.Text)) return;

            try
            {
                tx = Encoding.UTF8.GetBytes("CLOSE");//преобразование Text в byte

                if (mTcpClient != null)
                {
                    if (mTcpClient.Client.Connected)
                    {
                        mTcpClient.GetStream().BeginWrite(tx, 0, tx.Length, onCompleteWriteToServer, mTcpClient);//отправляем сообщение
                        MessageBox.Show("Сообщение отправлено!");
                    }
                }
                tbPayload.Text = "";
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
