using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FicSauve2A
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            INI ini = new INI(@"C:\Users\Utilisateur\source\repos\FicSauve2A\test.ini");
            cFTP ftp = new cFTP(ini.lireIni("ServeurFTP", "AdresseServeur"), ini.lireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(ini.lireIni("ServeurFTP", "MP")));
            
            if (args.Length > 0)
            {
                cFichier tmp = new cFichier();
                tmp.cheminLocal = @"C:\Users\Utilisateur\Desktop\Infos ftp.txt";
                tmp.cheminDistant = "test.txt";
                List<cFichier> listeTMP = new List<cFichier>();
                listeTMP.Add(tmp);
                ftp.fichierTransfert(listeTMP, null);

                List<cRepASauvegarder> listRepASauvegarder = ini.getDirectoryToSave();
                foreach (cRepASauvegarder rep in listRepASauvegarder)
                {
                    ftp.dossierRecursifTransfert(rep.path + "\\", null, rep.bRecursif);
                }


            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
