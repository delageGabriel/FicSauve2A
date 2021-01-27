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
            ftp = new cFTP(ini.lireIni("ServeurFTP", "AdresseServeur"), ini.lireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(ini.lireIni("ServeurFTP", "MP")));

        }

        private void button1_Click(object sender, EventArgs e)
        {

            cFichier tmp = new cFichier();
            tmp.cheminLocal = @"C:\Users\Utilisateur\source\repos\delageGabriel\FicSauve2A\FicSauve2A\bin\Sauvegarde";
            tmp.cheminDistant = "Sauvegarde.exe";
            List<cFichier> listeTMP = new List<cFichier>();
            listeTMP.Add(tmp);

            Task.Run(() => ftp.fichierTransfert(listeTMP, null));
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp._supprimeDossier("test");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
                Application.Exit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.creerDossier("test");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
                Application.Exit();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.renommeFichier("test.txt", "test2.txt");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
                Application.Exit();
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            cErreur retour = ini.ecrireIni("FFFFFFF", "GUID", "ABCDEFG");
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
                Application.Exit();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string retour = ini.lireIni("FFFFFFF", "GUID");
            MessageBox.Show(retour);
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cErreur retour = ftp.dossierRecursifTransfert(@"C:\Users\Utilisateur\Desktop\Infosftp\", null, true);
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
                Application.Exit();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<cRepASauvegarder> listRepASauvegarder = ini.getDirectoryToSave();
            foreach (cRepASauvegarder rep in listRepASauvegarder)
            {
                ftp.dossierRecursifTransfert(rep.path + "\\", null, rep.bRecursif);
            }
            Application.Exit();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string retour = cCryptage.Encrypt(ini.lireIni("ServeurFTP", "MP"));
            MessageBox.Show(retour);
            Application.Exit();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string retour = ini.checkVersion(@"C:\Users\Utilisateur\source\repos\delageGabriel\FicSauve2A\version.ini", "version.ini");
            MessageBox.Show(retour);
        }
    }
}
