using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FicSauve2A
{
    public partial class Sauvegarde : Form
    {
        private cFTP ftp;
        private INI ini;
        public Sauvegarde()
        {
            InitializeComponent();

            ini = new INI(@"C:\Users\Utilisateur\source\repos\delageGabriel\FicSauve2A\test.ini");
            ftp = new cFTP(ini.LireIni("ServeurFTP", "AdresseServeur"), ini.LireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(ini.LireIni("ServeurFTP", "MP")));

        }

        private void button1_Click(object sender, EventArgs e)
        {

            CFichier tmp = new CFichier();
            tmp.CheminLocal = @"C:\Users\Utilisateur\Desktop\version.ini";
            tmp.CheminDistant = "version.ini";
            List<CFichier> listeTMP = new List<CFichier>();
            listeTMP.Add(tmp);

            Task.Run(() => ftp.fichierTransfert(listeTMP, null));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp._supprimeDossier("test");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.creerDossier("test");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.renommeFichier("test.txt", "test2.txt");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            cErreur retour = ini.EcrireIni("FFFFFFF", "GUID", "ABCDEFG");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string retour = ini.LireIni("FFFFFFF", "GUID");
            MessageBox.Show(retour);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.dossierRecursifTransfert(@"C:\Users\Utilisateur\Desktop\Infosftp\", null, true);
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<CRepASauvegarder> listRepASauvegarder = ini.GetDirectoryToSave();
            foreach (CRepASauvegarder rep in listRepASauvegarder)
            {
                ftp.dossierRecursifTransfert(rep.Path + "\\", null, rep.BRecursif);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string retour = cCryptage.Encrypt(ini.LireIni("ServeurFTP", "MP"));
            MessageBox.Show(retour);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string retour = ini.CheckVersion(@"C:\Users\Utilisateur\source\repos\delageGabriel\FicSauve2A\version.ini", "version.ini");
            MessageBox.Show(retour);
        }
    }
}
