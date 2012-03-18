using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using AirWin;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        AirWin.Airwin air= new Airwin();

        static string URL = "http://www.bitsdelocos.es/donate.php";

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
           
            int longitud = air.GetNumberOfIntefaces();
            if (longitud != 0)
            {
                air.SetInterfaz(0);
                for (int i = 0; i < longitud; i++)
                {
                    listBox1.Items.Add(air.GetNameOfInterface(i));
                }

            }
            else scan.Enabled = false;
        }

        private void actualiza_progress()
        {
           progressBar1.Maximum=air.lineas;
           progressBar1.Minimum = 0;
           progressBar1.Value = (air.TestedKeys + air.SkipedKeys);
           progressBar1.Update();

        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            air.SetInterfaz( listBox1.SelectedIndex);
        }
    
        private void actualizar()
        {
            
            air.Scan();
            air.preparaAvaiable();
            air.preparaBssList();
            listView1.Items.Clear();
            int longitud = air.GetLenghtBss();

            
            int indice_red;
            for (int i = 0; i < longitud; i++)
            {
               
                indice_red = air.GetNetIndex(air.Bss_Ssid_struct(i));


                ListViewItem item1 = new ListViewItem(air.Bss_Ssid(i));
                item1.SubItems.Add(air.Bss_Bssid(i));
                try
                {
                    item1.SubItems.Add(air.GetAuthAlgorithm(indice_red));
                    item1.SubItems.Add(air.GetCipherAlgorithm(indice_red));
                    item1.SubItems.Add(air.GetConectable(indice_red));
                    item1.SubItems.Add(air.GetSignalQuality(indice_red));
                    item1.SubItems.Add(air.Bss_rssi(i));
                }
                catch { }

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
                int index = listView1.SelectedIndices[0];
                air.SetBss(index);
                int indice_red = air.GetNetIndex(air.Bss_Ssid_struct(index));
                textBox1.Text =air.Bss_Ssid(index);
                textBox3.Text = air.GetAuthAlgorithm(indice_red);
                textBox2.Text = air.GetCipherAlgorithm(indice_red);
            }
            catch { }



        }


 


       


        private void button1_Click(object sender, EventArgs e)
        {
            label9.Visible = false;



            

            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                timer2.Enabled = true;
                timer2.Start();

                string cifrado = air.auth(textBox2.Text);
                string autenticacion = air.cif(textBox3.Text);
                if (cifrado == "" || autenticacion == "")
                {
                    label8.Text = "No implementado aún";
                    label8.Visible = true;
                }else{

                label8.Visible = false;
                label5.Visible = true;
                progressBar1.Visible = true;
                timer1.Enabled = false;
                string key = "";
               // Process oProceso = new Process();
                //oProceso.
                //backgroundWorker1.
                bool exito=air.ataque_dicc(textBox1.Text, cifrado, textBox4.Text, autenticacion, (int)(numericUpDown1.Value),key);
                if (exito)
                {
                    label8.Text = "La clave es:" + air.Resultat;
                    label8.Visible = true;
                }
                else
                {
                    label8.Text = "No se ha encontrado clave";
                    label8.Visible = true;
                }

                timer2.Stop();
                timer2.Enabled = false;
                timer1.Enabled = true;
                label5.Visible = false;
                progressBar1.Visible = false;
            }
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

        private void timer2_Tick(object sender, EventArgs e)
        {
            actualiza_progress();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }




    
    }
}