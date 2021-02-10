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

    /// <summary>
    /// Classe INI.
    /// </summary>
    internal class INI
    {
        ///////////////////////////////////////////////////////////
        // ATTRIBUTS
        ///////////////////////////////////////////////////////////
        private IniData data;
        private FileIniDataParser parser;
        private cFTP ftp;

        ///////////////////////////////////////////////////////////
        // CONSTRUCTEUR
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="INI"/> class.
        /// Constructeur de la classe INI.
        /// </summary>
        /// <param name="chemin">Chemin du fichier ini à utiliser.</param>
        public INI(string chemin)
        {
            this.Chemin = chemin;
            this.parser = new FileIniDataParser();
            this.ftp = new cFTP(this.LireIni("ServeurFTP", "AdresseServeur"), this.LireIni("ServeurFTP", "Utilisateur"), cCryptage.Decrypt(this.LireIni("ServeurFTP", "MP")));
        }

        ///////////////////////////////////////////////////////////
        // ACCESSEUR/MUTATEUR
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets le chemin du fichier.ini à utiliser.
        /// </summary>
        public string Chemin { get; set; }

        ///////////////////////////////////////////////////////////
        // METHODES
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Méthode lireIni qui permet de lire une section et une clé passées en paramètres.
        /// </summary>
        /// <param name="sectionName">Nom de la section à lire.</param>
        /// <param name="keyName">Clé de la section à lire.</param>
        /// <returns>La valeur de la clé de la section passée en paramètre.</returns>
        public string LireIni(string sectionName, string keyName)
        {
            this.data = this.parser.ReadFile(this.Chemin);
            string res = this.data[sectionName][keyName];
            return res;
        }

        /// <summary>
        /// Procédure getDirectoryToSave qui permet de transférer une liste de fichier et répertoires depuis un fichier ini
        /// Et savoir s'il faut également transférer des répertoires récursifs ou non.
        /// </summary>
        /// <returns>La liste des répertoires transférés.</returns>
        public List<CRepASauvegarder> GetDirectoryToSave()
        {
            bool play = true;

            // bool playDeux = true;
            int i = 1;
            List<CRepASauvegarder> listRep = new List<CRepASauvegarder>();
            string fmt = "000.##";
            while (play)
            {
                string path = this.LireIni("Repertoires", "Rep_" + i.ToString(fmt));

                string sRecursif = this.LireIni("RepertoiresRecursif", "Rep_" + i.ToString(fmt));
                bool bRecursif = sRecursif == "1";

                if (path == null)
                {
                    play = false;
                }
                else
                {
                    CRepASauvegarder temp = new CRepASauvegarder(path, bRecursif);
                    listRep.Add(temp);
                }

                i++;
            }

            return listRep;
        }

        /// <summary>
        /// Méthode écrireIni qui permet d'écrire une section et une clé dans le fichier spécifié dans le chemin passé en paramètre.
        /// </summary>
        /// <param name="sectionName">Nom de la section à écrire.</param>
        /// <param name="keyName">Clé de la section à écrire.</param>
        /// <param name="valeur">Valeur à écrire dans le fichier ini.</param>
        /// <returns>Retourne le résultat.</returns>
        public cErreur EcrireIni(string sectionName, string keyName, string valeur)
        {
            cErreur res = new cErreur();
            this.data = this.parser.ReadFile(this.Chemin);
            try
            {
                this.data[sectionName][keyName] = valeur;
                this.parser.WriteFile(this.Chemin, this.data);
                res.bErreur = false;
                res.message = string.Empty;
            }
            catch (Exception e)
            {
                res.bErreur = true;
                res.message = e.Message;
            }

            return res;
        }

        /// <summary>
        /// Méthode qui permet de vérifier si deux fichiers ini sont identiques, et le met à jour
        /// dans le cas contraire.
        /// </summary>
        /// <param name="cheminFichierIniLocal">Chemin local du fichier ini à vérifier.</param>
        /// <param name="cheminFichierIniServeur">Chemin du serveur du fichier ini à vérifier.</param>
        /// <returns>La version du fichier ini en local.</returns>
        public string CheckVersion(string cheminFichierIniLocal, string cheminFichierIniServeur)
        {
            string fichierRecupIniLocal;
            string fichierRecupIniServeur;

            this.Chemin = cheminFichierIniLocal;
            fichierRecupIniLocal = this.LireIni("Version", "Version");

            this.ftp.downloadFile(cheminFichierIniServeur, "version.ini");

            this.Chemin = cheminFichierIniServeur;
            fichierRecupIniServeur = this.Chemin;
            fichierRecupIniServeur = this.LireIni("Version", "Version");

            if (fichierRecupIniLocal != fichierRecupIniServeur)
            {
                MessageBox.Show($"La version n'est pas à jour ! La version du serveur est {fichierRecupIniServeur}");
                this.ftp.downloadFile("Update.exe", "FicSauve2A.exe");
            }
            else
            {
                MessageBox.Show("Les deux versions sont identiques");
            }

            return "Version locale = " + fichierRecupIniLocal;
        }
    }
}
