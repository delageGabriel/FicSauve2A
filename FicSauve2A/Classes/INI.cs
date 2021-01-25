using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;

namespace FicSauve2A
{
    class INI
    {
        public string chemin { get; set; }
        private IniData data;
        private FileIniDataParser parser;
        private cFTP ftp;

        public INI(string chemin)
        {
            this.chemin = chemin;
            parser = new FileIniDataParser();
            ftp = new cFTP(lireIni("ServeurFTP", "AdresseServeur"), lireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(lireIni("ServeurFTP", "MP")));
        }

        public string lireIni(string sectionName, string keyName)
        {
            data = parser.ReadFile(chemin);
            string res = data[sectionName][keyName];
            return res;
        }

        public List<cRepASauvegarder> getDirectoryToSave()
        {
            bool play = true;
            int i = 1;
            List<cRepASauvegarder> listRep = new List<cRepASauvegarder>();
            string fmt = "000.##";


            while (play)
            {

                string path = lireIni("Repertoires", "Rep_" + i.ToString(fmt));

                string sRecursif = lireIni("RepertoiresRecursif", "Rep_" + i.ToString(fmt));
                bool bRecursif = (sRecursif == "1");

                if (path == null)
                {
                    play = false;
                }
                else
                {
                    cRepASauvegarder temp = new cRepASauvegarder(path, bRecursif);
                    listRep.Add(temp);
                }

                i++;


            }

            return listRep;

        }
               

        public cErreur ecrireIni(string sectionName, string keyName, string valeur)
        {
            cErreur res = new cErreur();
            data = parser.ReadFile(chemin);
            try
            {
                data[sectionName][keyName] = valeur;
                parser.WriteFile(chemin, data);
                res.bErreur = false;
                res.message = "";
            }
            catch (Exception e)
            {
                res.bErreur = true;
                res.message = e.Message;
            }

            return res;
        }

        public string checkVersion(string cheminFichierIniLocal, string cheminFichierIniServeur)
        {
            string fichierRecupIniLocal;
            string fichierRecupIniServeur;


            chemin = cheminFichierIniLocal;
            fichierRecupIniLocal = lireIni("Version", "Version");

            ftp.downloadFile("version.ini", "version.ini");

            chemin = cheminFichierIniServeur;
            fichierRecupIniServeur = chemin;
            fichierRecupIniServeur = lireIni("Version", "Version");


            if (fichierRecupIniLocal != fichierRecupIniServeur)
            {
                MessageBox.Show("La version n'est pas à jour !");
            }
            else
            {
                MessageBox.Show("Les deux versions sont identiques");
            }

            return fichierRecupIniLocal;
        }
    }
}
