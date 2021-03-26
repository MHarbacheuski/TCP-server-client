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
using System.Reflection;

namespace TCPServer01
{
    public partial class Form1 : Form
    {
        
        TcpListener mTCPListener;//класс для прослушивания клиентов
        TcpClient mTCPClient;//TcpClient предоставляет клиентские подключения для сетевых служб протокола TCP
        byte[] mRx; 
        int count = 0;

        private List<ClientNode> mlClientSocks;//список подключенных клиентов


        public Form1()
        {
            InitializeComponent();
            //startServer();
            
            mlClientSocks = new List<ClientNode>(2);

            CheckForIllegalCrossThreadCalls = false;

        }
       
        //функция получения собственного ip адреса
        IPAddress findMyIPV4Address()
        {
            string strThisHostName = string.Empty;
            IPHostEntry thisHostDNSEntry = null;//IPHostEntry предоставляет класс контейнеров для сведений об адресе веб-узла.
            IPAddress[] allIPsOfThisHost = null;
            IPAddress ipv4Ret = null;

            try
            {
                strThisHostName = System.Net.Dns.GetHostName();//возвращает имя узла локального компьютера
                thisHostDNSEntry = System.Net.Dns.GetHostEntry(strThisHostName);
                allIPsOfThisHost = thisHostDNSEntry.AddressList;

                for (int idx = allIPsOfThisHost.Length - 1; idx >= 0; idx--)
                {
                    if (allIPsOfThisHost[idx].AddressFamily == AddressFamily.InterNetwork)//Если это ipv4-адресс
                    {
                        ipv4Ret = allIPsOfThisHost[idx];
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            return ipv4Ret;
        }
        private void startServer()
        {
            try
            {
                IPAddress ipaddr;//предоставление ip адреса
                int nPort;//порт для прослушивания 

                if (!int.TryParse(tbPort.Text, out nPort))
                {
                    nPort = 23000;
                }
                if (!IPAddress.TryParse(tbIPAddress.Text, out ipaddr))
                {
                    MessageBox.Show("Invalid IP address supplied.");
                    return;
                }

                mTCPListener = new TcpListener(ipaddr, nPort);

                mTCPListener.Start();//начинаем прослушивание

                mTCPListener.BeginAcceptTcpClient(onCompleteAcceptTcpClient, mTCPListener);//начинает асинхронную операция, чтобы принять попытку входящего подключения.
            }catch(Exception p) { p.ToString(); }
        }
        
        //функция принятия входящих подключений
        void onCompleteAcceptTcpClient(IAsyncResult iar)
        {
            
            TcpListener tcpl = (TcpListener)iar.AsyncState;//получаем слушателя, который обрабатывает запрос клиента
            TcpClient tclient = null;
            ClientNode cNode = null;//используется для работы с клиентами

            try
            {
                tclient = tcpl.EndAcceptTcpClient(iar);//принятие входящей попытки подлючения

                printLine("Client Connected...");

                tcpl.BeginAcceptTcpClient(onCompleteAcceptTcpClient, tcpl);

                lock (mlClientSocks)
                {
                    mlClientSocks.Add((cNode = new ClientNode(tclient, new byte[100000], new byte[100000], tclient.Client.RemoteEndPoint.ToString())));//создание нового клиента
                    lbClients.Items.Add(cNode.ToString());//добавление нового клиента в список всех клиентов               
                }

                tclient.GetStream().BeginRead(cNode.Rx, 0, cNode.Rx.Length, onCompleteReadFromTCPClientStream, tclient);//начинаем чтения данных с клиентов

                //mRx = new byte[512];
                //mTCPClient.GetStream().BeginRead(mRx, 0, mRx.Length, onCompleteReadFromTCPClientStream, mTCPClient);


            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ReceiveTCP(int portN)
        {
            TcpListener Listener = null;

            try
            {
                Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), portN);
                Listener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            int BufferSize = 4096;
            byte[] RecData = new byte[BufferSize];
            int RecBytes;

            for (; ; )
            {
                TcpClient client = null;
                NetworkStream netstream = null;
                //Status = string.Empty;
                try
                {
                    string message = "Accept the Incoming File ";
                    string caption = "Incoming Connection";
                    //MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    //DialogResult result;


                    if (Listener.Pending())
                    {
                        client = Listener.AcceptTcpClient();
                        netstream = client.GetStream();
                        //Status = "Connected to a client\n";
                        //result = MessageBox.Show(message, caption, buttons);
                        tbConsoleOutput.Invoke(new Action(() => tbConsoleOutput.Text = "Connected to a sender"));
                        string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string SaveFileName = dirPath + "\\" + string.Format("RecvdFile-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                        if (SaveFileName != string.Empty)
                        {
                            int totalrecbytes = 0;
                            FileStream Fs = new FileStream(SaveFileName, FileMode.OpenOrCreate, FileAccess.Write);
                            while ((RecBytes = netstream.Read(RecData, 0, RecData.Length)) > 0)
                            {
                                Fs.Write(RecData, 0, RecBytes);
                                totalrecbytes += RecBytes;
                            }
                            Fs.Close();
                        }
                        netstream.Close();
                        client.Close();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //netstream.Close();
                }
            }
        }

        //функция чтения данных с клиентов
        void onCompleteReadFromTCPClientStream(IAsyncResult iar)
        {
            TcpClient tcpc;
            int nCountReadBytes = 0;//Количество полученных байт
            //string strRecv;
            ClientNode cn = null;//используется для работы с клиентами
            
            try
            {
                lock(mlClientSocks)
                {
                    
                    tcpc = (TcpClient)iar.AsyncState;
                    
                    cn = mlClientSocks.Find(x => x.strId == tcpc.Client.RemoteEndPoint.ToString());//выбор определённого клиента для отправки сообщения

                    nCountReadBytes = tcpc.GetStream().EndRead(iar);

                    if (nCountReadBytes == 0)
                    {
                        MessageBox.Show("Client disconnected.");
                        mlClientSocks.Remove(cn);
                        lbClients.Items.Remove(cn.ToString());
                        return;
                    }


                        
                        printLine(" Text : " + Encoding.UTF8.GetString(cn.Rx, 0, nCountReadBytes).ToString());
                        
                        

                        if(Encoding.UTF8.GetString(cn.Rx, 0, nCountReadBytes).ToString()== "TIME")
                        {
                            cn.Tx = Encoding.UTF8.GetBytes(DateTime.Now.ToString()); //преобразование Text в byte 

                            cn.tclient.GetStream().BeginWrite(cn.Tx, 0, cn.Tx.Length, onCompleteWriteToClientStream, cn.tclient);//отправка сообщения клиенту
                        }
                        else if(Encoding.UTF8.GetString(cn.Rx, 0, nCountReadBytes).ToString() == "CLOSE")
                        {                                                         
                                mlClientSocks.Remove(cn);
                                lbClients.Items.Remove(cn.ToString());                                                        
                        }
                        else if(Encoding.UTF8.GetString(cn.Rx, 0, 8).ToString() == "DOWNLOAD")
                        {
                            int nameSize = BitConverter.ToInt32(cn.Rx,8);
                            string filename = Encoding.UTF8.GetString(cn.Rx, 12, nameSize);
                            int fileSize = BitConverter.ToInt32(cn.Rx, 12 + nameSize);
                            byte[] data = new byte[100000];
                            //cn.Rx.CopyTo(data, 12+4+fileSize);
                            Array.Copy(cn.Rx, 12 + 4, data, 0, fileSize);
                            File.WriteAllBytes("titylnik.docx", data);
                        }

                        
                        Logger.WriteLine(cn.ToString() + ": "+ "Text: " + Encoding.UTF8.GetString(cn.Rx, 0, nCountReadBytes).ToString());//запись информации в файл
                        cn.Rx = new byte[100000];
                    
                    


                    tcpc.GetStream().BeginRead(cn.Rx, 0, cn.Rx.Length, onCompleteReadFromTCPClientStream, tcpc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lock (mlClientSocks)
                {
                    printLine("Client disconnected: " + cn.ToString());
                    mlClientSocks.Remove(cn);
                    lbClients.Items.Remove(cn.ToString());
                }

            }
        }

        private void printHexLine(object p)
        {
            throw new NotImplementedException();
        }

        public void printLine(string _strPrint)
        {
            tbConsoleOutput.Invoke(new Action<string>(doInvoke), _strPrint);
        }

        public void printHexLine(string _strPrint)
        {
            textBox1.Invoke(new Action<string>(doInvokeHex), _strPrint);
        }

        public void doInvoke(string _strPrint)
        {
            tbConsoleOutput.Text = _strPrint + Environment.NewLine + tbConsoleOutput.Text;
        }
        public void doInvokeHex(string _strPrint)
        {
            textBox1.Text = _strPrint + Environment.NewLine + textBox1.Text;
        }

        //функция отправления сообщения клиенту
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbClients.Items.Count <= 0) return;
                if (string.IsNullOrEmpty(tbPayload.Text)) return;
                ClientNode cn = null;
                cn = mlClientSocks.Find(x => x.strId == lbClients.SelectedItem.ToString());//выбор определённого клиента для отправки сообщения
                cn.Tx = new byte[100000];//буффер для отправки                 
                
                
                 lock (mlClientSocks)//lock используется для того,чтобы синхронизировать потоки и ограничить доступ к разделяемым ресурсам на время их использования каким-нибудь потоком.
                 {
                    

                    try
                    {
                        if (cn != null)
                        {
                            if (cn.tclient != null)
                            {
                                if (cn.tclient.Client.Connected)
                                {
                                    cn.Tx = Encoding.UTF8.GetBytes(tbPayload.Text); //преобразование Text в byte 
                                    
                                        cn.tclient.GetStream().BeginWrite(cn.Tx, 0, cn.Tx.Length, onCompleteWriteToClientStream, cn.tclient);//отправка сообщения клиенту
                                     
                                        MessageBox.Show("Сообщение отправлено!");
                                }
                            }
                        }
                        tbPayload.Text = "";
                    }
                       
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                       
                 }
                    
                
            }
            catch(Exception p) { MessageBox.Show("Выберите получателя для отправки сообщения");}
        }

       //функция отправки данных клиенту
        private void onCompleteWriteToClientStream(IAsyncResult iar)
        {
            try
            {
                TcpClient tcpc = (TcpClient)iar.AsyncState;
                tcpc.GetStream().EndWrite(iar);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnFindIPv4IP_Click_1(object sender, EventArgs e)
        {
            
            IPAddress ipa = null;

            ipa = findMyIPV4Address();
            if (ipa != null)
            {
                tbIPAddress.Text = ipa.ToString();
            }
            startServer();
            //ReceiveTCP(23000);
            tbConsoleOutput.Invoke(new Action(() => tbConsoleOutput.Text = "Server is running"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
        
        //функция отправления массива байт клиенту
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ClientNode cn = null;               
                if (lbClients.Items.Count <= 0) return;

                lock (mlClientSocks)
                {
                    
                    cn = mlClientSocks.Find(x => x.strId == lbClients.SelectedItem.ToString());//выбор определённого клиента для отправки сообщения
                    cn.Tx = new byte[100000];//буффер для отправки
                    
                    try
                    {
                        if (cn != null)
                        {
                            if (cn.tclient != null)
                            {
                                if (cn.tclient.Client.Connected)
                                {
                                    
                                    for (int i = Convert.ToInt32(num1.Text), j = 0; j <= Convert.ToInt32(num3.Text) + 1; i++, j++)
                                    {
                                        if (i > 255)//если значение i больше чем 255, то находим остаток от деления, и приравниваем этот остаток к i
                                        {
                                            i %= 256;
                                        }
                                        if (j == 0)//заполняем наш первый элемен массива символом *(42)
                                        {
                                            cn.Tx[0] = 42;
                                            i = Convert.ToInt32(num1.Text) - 1;
                                            continue;
                                        }
                                        cn.Tx[j] = (byte)(i);//заполняем массив байтов различными значениями
                                        i += Convert.ToInt32(num2.Text) - 1;//увеличиваем шаг

                                    }

                                    for (int i = 0; i < cn.Tx.Length; i++)
                                    {
                                        if (cn.Tx[i] == 0 && cn.Tx[i + 1] == 0 && cn.Tx[i + 2] == 0)// если 3 подряд значения массива байтов равна 0, то удаляем находим общее количество этих нулей
                                        {
                                            count = cn.Tx.Length - i;
                                            break;
                                        }
                                    }
                                    Array.Resize(ref cn.Tx, cn.Tx.Length - count);//обрезаем нули, которые нам не нужны
                                    count = 0;                                                                   
                                        cn.tclient.GetStream().BeginWrite(cn.Tx, 0, cn.Tx.Length, onCompleteWriteToClientStream, cn.tclient);//отправка сообщения клиенту                                                                         
                                    MessageBox.Show("Сообщение отправлено!");
                                }
                            }
                        }                        
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception p) { MessageBox.Show("Выберите получателя для отправки сообщения"); }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbClients.Items.Count <= 0) return;
               
                ClientNode cn = null;
                cn = mlClientSocks.Find(x => x.strId == lbClients.SelectedItem.ToString());//выбор определённого клиента для отправки сообщения
                cn.Tx = new byte[Convert.ToInt32(textBox2.Text)];//буффер для отправки


                int k = 0;
                while (k < Convert.ToInt32(textBox3.Text))
                {
                    lock (mlClientSocks)//lock используется для того,чтобы синхронизировать потоки и ограничить доступ к разделяемым ресурсам на время их использования каким-нибудь потоком.
                    {


                        try
                        {
                            if (cn != null)
                            {
                                if (cn.tclient != null)
                                {
                                    if (cn.tclient.Client.Connected)
                                    {
                                        string message = "";
                                        for(int i=0;i<Convert.ToInt32(textBox2.Text);i++)
                                        {
                                            message += "q";
                                        }
                                        cn.Tx = Encoding.UTF8.GetBytes(message); //преобразование Text в byte 

                                        cn.tclient.GetStream().BeginWrite(cn.Tx, 0, cn.Tx.Length, onCompleteWriteToClientStream, cn.tclient);//отправка сообщения клиенту
                                        //System.Threading.Thread.Sleep(100);
                                        // MessageBox.Show("Сообщение отправлено!");
                                    }
                                }
                            }
                        }

                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        k++;
                    }
                    //tbPayload.Text = "";
                }
            }
            catch (Exception p) { MessageBox.Show("Выберите получателя для отправки сообщения"); }
        }

        private void tbConsoleOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
    }
    static public class Logger
    {
        //----------------------------------------------------------
        // Статический метод записи строки в файл без переноса
        //----------------------------------------------------------
        public static void Write(string text)
        {
            using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data.log", true))
            {
                sw.Write(text);
            }
        }

        //---------------------------------------------------------
        // Статический метод записи строки в файл с переносом
        //---------------------------------------------------------
        public static void WriteLine(string message)
        {
            using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data.log", true))
            {
                sw.WriteLine(String.Format("{0} {1}", DateTime.Now.ToString() + ":", message));
            }
        }
    }


}
