/// <summary>
// <copyright file="CFTP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>"
/// </summary>


namespace FicSauve2A
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Classe cFTP.
    /// </summary>
    public class CFTP
    {

        ///////////////////////////////////////////////////////////
        // ATTRIBUTS
        ///////////////////////////////////////////////////////////
        private Uri target;
        private NetworkCredential cred;

        ///////////////////////////////////////////////////////////
        // CONSTRUCTEUR
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CFTP"/> class.
        /// Constructeur de la classe cFTP.
        /// </summary>
        /// <param name="pUrl">URI du serveur ftp.</param>
        /// <param name="pUser">Identifiant du serveur ftp.</param>
        /// <param name="pPassword">Mot de passe du serveur ftp.</param>
        public CFTP(string pUrl, string pUser, string pPassword)
        {
            target = new Uri(pUrl);
            cred = new NetworkCredential(pUser, pPassword);
        }

        /// <summary>
        /// Méthode qui va supprimer le dossier portant le même nom
        /// que celui passé en paramètre.
        /// </summary>
        /// <param name="pDossier">Nom du dossier à supprimer.</param>
        /// <returns>Retourne Res</returns>
        public cErreur SupprimeDossier(string pDossier)
        {

            cErreur Res = new cErreur();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + pDossier + "/");
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            request.Credentials = cred;
            FtpWebResponse response;

            try
            {
                response = (FtpWebResponse)request.GetResponse();
                Res.BErreur = false;
                Res.Message = response.StatusDescription;
                MessageBox.Show($"Le dossier {pDossier} a bien été supprimé !");
                response.Close();
            }
            catch (Exception e)
            {
                Res.BErreur = true;
                Res.Message = e.Message;
            }
            return Res;
        }

        /// <summary>
        /// Méthode qui va créer un dossier avec le même nom 
        /// que celui passé en paramètre.
        /// </summary>
        /// <param name="pDossier">Nom du dossier à créer.</param>
        /// <returns>Retourne Res.</returns>
        public cErreur CreerDossier(string pDossier)
        {
            cErreur Res = new cErreur();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + pDossier + "/");
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = cred;
            FtpWebResponse response;

            try
            {
                response = (FtpWebResponse)request.GetResponse();
                Res.BErreur = false;
                Res.Message = response.StatusDescription;
                response.Close();
            }
            catch (Exception e)
            {
                Res.BErreur = true;
                Res.Message = e.Message;
            }
            return Res;
        }

        /// <summary>
        /// Méthode qui va renommer le fichier dont le nom est passé en premier paramètre,
        /// le nouveau nom est quant à lui à passer en second paramètre.
        /// </summary>
        /// <param name="pFichier">Nom du fichier à modifier.</param>
        /// <param name="nouveauNomFichier">Nouveau nom pour le fichier.</param>
        /// <returns>Retourne Res.</returns>
        public cErreur RenommeFichier(string pFichier, string nouveauNomFichier)
        {
            cErreur Res = new cErreur();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + pFichier);
            request.Method = WebRequestMethods.Ftp.Rename;
            request.Credentials = cred;
            request.RenameTo = nouveauNomFichier;
            FtpWebResponse response;

            try
            {
                response = (FtpWebResponse)request.GetResponse();
                Res.BErreur = false;
                Res.Message = response.StatusDescription;
                MessageBox.Show($"Le fichier {pFichier} a bien été renommé en {nouveauNomFichier} !");
                response.Close();
            }
            catch (Exception e)
            {
                Res.BErreur = true;
                Res.Message = e.Message;
            }
            return Res;
        }

        /// <summary>
        /// Méthode qui permet de transférer une liste d'objets de la classe CFichier vers le serveur ftp,
        /// avec, ou sans progressBar.
        /// </summary>
        /// <param name="fichiers">Liste contenant des objets de la classe CFichier.</param>
        /// <param name="progressBar">Barre de progression permettant de voir l'avancée du transfert.</param>
        /// <param name="TailleMax">Taille max des fichiers à transférer.</param>
        /// <returns>Retourne Res.</returns>
        public cErreur FichierTransfert(List<CFichier> fichiers, ProgressBar progressBar, int TailleMax = 0)
        {
            cErreur Res = new cErreur();
            FtpState state = new FtpState();
            int valeurEnCoursPB = 0;

            foreach (CFichier fichier in fichiers)
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + fichier.CheminDistant);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = cred;
                state.Request = request;
                state.FileName = fichier.CheminLocal;

                using (Stream fileStream = File.OpenRead(fichier.CheminLocal))
                using (Stream ftpStream = request.GetRequestStream())
                {

                    if (TailleMax == 0)
                    {
                        TailleMax = (int)fileStream.Length;
                    }
                    else
                    {
                        fichier.Taille = (int)fileStream.Length;
                    }

                    if (progressBar != null)
                    {
                        progressBar.Invoke(
                            (MethodInvoker)delegate { progressBar.Maximum = TailleMax; });
                    }

                    byte[] buffer = new byte[42867898];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ftpStream.Write(buffer, 0, read);

                        if (progressBar != null)
                        {
                            progressBar.Invoke(
                               (MethodInvoker)delegate
                               {
                                   progressBar.Value = valeurEnCoursPB + (int)fileStream.Position;
                               });
                        }
                    }
                }

                valeurEnCoursPB += fichier.Taille;
                if (state.OperationException != null)
                {
                    Res.BErreur = true;
                    Res.Message = state.OperationException.Message;
                }
                else
                {                    
                    Res.BErreur = false;
                    Res.Message = state.StatusDescription;
                }
            }
            return Res;
        }


        /// <summary>
        /// Méthode qui permet le transfert de fichiers et de répertoires de manière récursive.
        /// </summary>
        /// <param name="cheminDuDossier">Chemin du dossier à transférer, ainsi que son contenu.</param>
        /// <param name="progressBar">Barre de progression.</param>
        /// <param name="bRecursif">Savoir si le répertoir est récursif ou non.</param>
        /// <returns>Retourne res.</returns>
        public cErreur DossierRecursifTransfert(string cheminDuDossier, ProgressBar progressBar, bool bRecursif = false)
        {
            cErreur Res = new cErreur();
            int tailleMax = 0;
            List<CFichier> fichiersATransferer = new List<CFichier>();
            string dossierRacine = Path.GetDirectoryName(cheminDuDossier);
            dossierRacine = dossierRacine.Split('\\').Last<string>();
            CreerDossier(dossierRacine);
            IEnumerable<string> fichiers = Directory.EnumerateFiles(cheminDuDossier);

            foreach (string fichier in fichiers)
            {
                string cheminDestination = dossierRacine + "/";

                CFichier tmp = new CFichier();
                tmp.CheminLocal = cheminDuDossier + Path.GetFileName(fichier);
                tmp.CheminDistant = cheminDestination + Path.GetFileName(fichier);

                using (Stream fileStream = File.OpenRead(tmp.CheminLocal))
                {
                    tmp.Taille = (int)fileStream.Length;
                    tailleMax += tmp.Taille;
                }
                fichiersATransferer.Add(tmp);
            }


            if (bRecursif)
            {
                IEnumerable<string> dossiers = Directory.EnumerateDirectories(cheminDuDossier);
                foreach (string dossier in dossiers)
                {
                    string name = dossier.Split('\\').Last<string>();
                    CreerDossier(dossierRacine + "/" + name);
                    IEnumerable<string> fichiersDeux = Directory.EnumerateFiles(dossier);

                    foreach (string fichier in fichiersDeux)
                    {
                        string cheminDestination = dossierRacine + "/" + name + "/";
                        CFichier tmp = new CFichier();
                        tmp.CheminLocal = fichier;
                        tmp.CheminDistant = cheminDestination + Path.GetFileName(fichier);

                        using (Stream fileStream = File.OpenRead(tmp.CheminLocal))
                        {
                            tmp.Taille = (int)fileStream.Length;
                            tailleMax += tmp.Taille;
                        }
                        fichiersATransferer.Add(tmp);
                    }
                }
            }

            try
            {
                Task.Run(() => FichierTransfert(fichiersATransferer, progressBar, tailleMax));
            }
            catch (Exception e)
            {
                Res.BErreur = true;
                Res.Message = e.Message;
            }
            return Res;
        }

        /// <summary>
        /// Méthode qui permet télécharger un fichier depuis le serveur ftp.
        /// </summary>
        /// <param name="cheminLocalTelechargement">Chemin local où le fichier sera téléchargé.</param>
        /// <param name="nomFichier">Le nom du fichier une fois qu'il sera téléchargé.</param>
        /// <returns>Retourne Res.</returns>
        public cErreur DownloadFile(string cheminLocalTelechargement, string nomFichier)
        {
            cErreur Res = new cErreur();
            WebClient client = new WebClient();
            client.Credentials = cred;
            client.DownloadFile(
                target + nomFichier, cheminLocalTelechargement);

            return Res;
        }
    }
}
