﻿using System;
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

        private cFTP ftp;
        private INI ini;
        public FicSauve2A()
        {
            InitializeComponent();
            
            ini = new INI(@"FicSauve2A\test.ini");
            ftp = new cFTP(ini.lireIni("ServeurFTP", "AdresseServeur"), ini.lireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(ini.lireIni("ServeurFTP", "MP")));

            
        }



        private void button1_Click(object sender, EventArgs e)
        {

            CFichier tmp = new CFichier();
            tmp.CheminLocal = @"Sauvegarde\Sauvegarde.exe";
            tmp.CheminDistant = "Sauvegarde.exe";
            List<CFichier> listeTMP = new List<CFichier>();
            listeTMP.Add(tmp);

            Task.Run(() => ftp.fichierTransfert(listeTMP, progressBar));           

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
            cErreur retour = ini.ecrireIni("FFFFFFF", "GUID", "ABCDEFG");
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
            cErreur retour = ftp.dossierRecursifTransfert(@"Desktop\Infosftp\", progressBar, true);
            if (retour.bErreur)
            {
                MessageBox.Show(retour.message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<cRepASauvegarder> listRepASauvegarder = ini.getDirectoryToSave();
            foreach (cRepASauvegarder rep in listRepASauvegarder)
            {
                ftp.dossierRecursifTransfert(rep.path + "\\", progressBar, rep.bRecursif);
            }
        }
        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string retour = cCryptage.Encrypt(ini.lireIni("ServeurFTP", "MP"));
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

        private void button11_Click_1(object sender, EventArgs e)
        {
            string retour = ini.checkVersion(@"FicSauve2A\version.ini", "version.ini");
            MessageBox.Show(retour);
        }
    }
}
