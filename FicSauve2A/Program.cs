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
            INI ini = new INI("test.ini");
            CFTP ftp = new CFTP(ini.LireIni("ServeurFTP", "AdresseServeur"), ini.LireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(ini.LireIni("ServeurFTP", "MP")));

        #if UPDATE
                    string retour = ini.checkVersion(@"FicSauve2A\version.ini", "version.ini");
                    MessageBox.Show(retour);
        #elif SAUVEGARDE
                    //string retour = ini.checkVersion(@"FicSauve2A\version.ini", "version.ini");
                    //MessageBox.Show(retour);

                    if (args.Length > 0 && args[0] == "silent")
                    {
                        string retour = ini.CheckVersion(@"FicSauve2A\version.ini", "version.ini");
                        MessageBox.Show(retour);

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Sauvegarde());
                        List<CRepASauvegarder> listRepASauvegarder = ini.GetDirectoryToSave();

                        foreach (CRepASauvegarder rep in listRepASauvegarder)
                        {
                            ftp.DossierRecursifTransfert(rep.Path + "\\", null, rep.BRecursif);
                        }                         
            }
        #endif
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FicSauve2A());

        }
    }
}
