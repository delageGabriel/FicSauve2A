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
    public partial class FicSauve2A : Form
    {

        private CFTP ftp;
        private INI ini;
        public FicSauve2A()
        {
            InitializeComponent();

            ini = new INI(@"C:\Users\Dev\Desktop\FicSauve2A\FicSauve2A\FicSauve2A\bin\CheckVersionIni\test.ini");
            ftp = new CFTP(ini.LireIni("ServeurFTP", "AdresseServeur"), ini.LireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(ini.LireIni("ServeurFTP", "MP")));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CFichier tmp = new CFichier();
            tmp.CheminLocal = @"C: \Users\Dev\Desktop\version.ini";
            tmp.CheminDistant = "version.ini";
            List<CFichier> listeTMP = new List<CFichier>();
            listeTMP.Add(tmp);
            ftp.FichierTransfert(listeTMP, progressBar);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.SupprimeDossier("test");
            if (retour.BErreur)
            {
                MessageBox.Show(retour.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.CreerDossier("test");
            if (retour.BErreur)
            {
                MessageBox.Show(retour.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            cErreur retour = ini.EcrireIni("FFFFFFF", "GUID", "ABCDEFG");
            if (retour.BErreur)
            {
                MessageBox.Show(retour.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string retour = ini.LireIni("FFFFFFF", "GUID");
            MessageBox.Show(retour);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.DossierRecursifTransfert(@"Desktop\Infosftp\", progressBar, true);
            if (retour.BErreur)
            {
                MessageBox.Show(retour.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<CRepASauvegarder> listRepASauvegarder = ini.GetDirectoryToSave();
            foreach (CRepASauvegarder rep in listRepASauvegarder)
            {
                ftp.DossierRecursifTransfert(rep.Path + "\\", progressBar, rep.BRecursif);
            }
        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string retour = cCryptage.Encrypt(ini.LireIni("ServeurFTP", "MP"));
            MessageBox.Show(retour);
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }
    }
}
