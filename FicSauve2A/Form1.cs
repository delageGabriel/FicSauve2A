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
            ini = new INI(@"C:\Users\Utilisateur\source\repos\FicSauve2A\test.ini");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.fichierTransfert("test.txt", @"C:\Users\Utilisateur\Desktop\Infos ftp.txt");
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
            cErreur retour = ftp.creerDossier("test", "ftp://home.guion.ovh/");
            if (retour.bErreur)
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
            cErreur retour = ini.ecrireIni("FFFFFFF", "GUID", "123456789");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string retour = ini.lireIni("FFFFFFF", "GUID");
            MessageBox.Show(retour);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.dossierRecursifTransfert(@"C:\Users\Utilisateur\Desktop\Infosftp\", "Infosftp", @"C:\Users\Utilisateur\Desktop\Infosftp\Infosftp.txt");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }
    }
}
