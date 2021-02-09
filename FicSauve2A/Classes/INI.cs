// <copyright file="INI.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FicSauve2A
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using IniParser;
    using IniParser.Model;

    internal class INI
    {
        ///////////////////////////////////////////////////////////
        // ATTRIBUTS
        ///////////////////////////////////////////////////////////

        public string Chemin { get; set; }

        private IniData data;
        private FileIniDataParser parser;
        private cFTP ftp;

        ///////////////////////////////////////////////////////////
        // CONSTRUCTEUR
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="INI"/> class.
        /// Constructeur de la classe INI, qui a comme paramètre le chemin à parcourir pour trouver le fichier ini
        /// à lire.
        /// </summary>
        /// <param name="chemin"></param>
        public INI(string chemin)
        {
            this.Chemin = chemin;
            parser = new FileIniDataParser();
            ftp = new cFTP(lireIni("ServeurFTP", "AdresseServeur"), lireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(lireIni("ServeurFTP", "MP")));
        }

        /////////////////////////////////////////////////////////// 
        ///                    METHODES
        /////////////////////////////////////////////////////////// 
        
        /// <summary>
        /// Méthode lireIni qui permet de lire une section et une clé passés en paramètres
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string lireIni(string sectionName, string keyName)
        {
            data = parser.ReadFile(Chemin);
            string res = data[sectionName][keyName];
            return res;
        }

        /// <summary>
        /// Procédure getDirectoryToSave qui permet de transférer une liste de fichier et répertoires depuis un fichier ini
        /// </summary>
        /// <returns></returns>
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
               
        /// <summary>
        /// méthode écrireIni qui permet d'écrire une section et une clé dans le fichier spécifié dans l'attribut chemin 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <param name="valeur"></param>
        /// <returns></returns>
        public cErreur ecrireIni(string sectionName, string keyName, string valeur)
        {
            cErreur res = new cErreur();
            data = parser.ReadFile(Chemin);
            try
            {
                data[sectionName][keyName] = valeur;
                parser.WriteFile(Chemin, data);
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

        /// <summary>
        /// méthode qui permet de vérifier si deux fichiers ini sont identiques, et le met à jour
        /// dans le cas contraire
        /// </summary>
        /// <param name="cheminFichierIniLocal"></param>
        /// <param name="cheminFichierIniServeur"></param>
        /// <returns></returns>
        public string checkVersion(string cheminFichierIniLocal, string cheminFichierIniServeur)
        {
            string fichierRecupIniLocal;
            string fichierRecupIniServeur;


            Chemin = cheminFichierIniLocal;
            fichierRecupIniLocal = lireIni("Version", "Version");

            ftp.downloadFile(cheminFichierIniServeur, "version.ini");

            Chemin = cheminFichierIniServeur;
            fichierRecupIniServeur = Chemin;
            fichierRecupIniServeur = lireIni("Version", "Version");


            if (fichierRecupIniLocal != fichierRecupIniServeur)
            {
                MessageBox.Show($"La version n'est pas à jour ! La version du serveur est {fichierRecupIniServeur}");              
                ftp.downloadFile("Update.exe", "FicSauve2A.exe");
                
            }
            else
            {
                MessageBox.Show("Les deux versions sont identiques");
            }

            

            return "Version locale = " + fichierRecupIniLocal;
        }
        
    }
}
