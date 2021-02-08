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
            INI ini = new INI(@"FicSauve2A\test.ini");
            cFTP ftp = new cFTP(ini.lireIni("ServeurFTP", "AdresseServeur"), ini.lireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(ini.lireIni("ServeurFTP", "MP")));

#if UPDATE
            string retour = ini.checkVersion(@"FicSauve2A\version.ini", "version.ini");
            MessageBox.Show(retour);
#elif SAUVEGARDE
            //string retour = ini.checkVersion(@"FicSauve2A\version.ini", "version.ini");
            //MessageBox.Show(retour);

            if (args.Length > 0 && args[0] == "silent")
            {
                string retour = ini.checkVersion(@"FicSauve2A\version.ini", "version.ini");
                MessageBox.Show(retour);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Sauvegarde());
                //switch (args[0])
                //{
                //    case "transfert fichier":
                //        cFichier tmp = new cFichier();
                //        tmp.cheminLocal = @"Desktop\version.ini";
                //        tmp.cheminDistant = "version.ini";
                //        List<cFichier> listeTMP = new List<cFichier>();
                //        listeTMP.Add(tmp);
                //        ftp.fichierTransfert(listeTMP, null);
                //        break;
                //    case "transfert dossier ini":
                List<cRepASauvegarder> listRepASauvegarder = ini.getDirectoryToSave();

                foreach (cRepASauvegarder rep in listRepASauvegarder)
                {
                    ftp.dossierRecursifTransfert(rep.path + "\\", null, rep.bRecursif);
                }
                //    break;
                //default:
                //    Console.WriteLine("Cet argument n'est pas pris en charge");
                //    break;

                //}        
                
            }
#endif
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FicSauve2A());

        }
    }
}
