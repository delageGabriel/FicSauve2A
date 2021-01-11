using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FicSauve2A
{
    public partial class Form1 : Form
    {
        private cFTP ftp;
        private INI ini;
        public Form1()
        {
            InitializeComponent();
            ftp = new cFTP("ftp://home.guion.ovh/", "ficsauve2a", "ficsauve2a");
            ini = new INI(@"C:\Users\Dev\Desktop\FicSauve2A\FicSauve2A\test.ini");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.transfertFichier("test.txt", @"C:\Users\Dev\Desktop\Infos ftp.txt");
            if(retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp._supprimeDossier("test");
            if(retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.creerDossier("test");
            if(retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.renommeFichier("test.txt", "test2.txt");
            if(retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cErreur retour = ini.ecrireIni("PECPLUS", "GUID", "123456789");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string retour = ini.lireIni("PECPLUS", "GUID");
            MessageBox.Show(retour);

        }
    }
}
