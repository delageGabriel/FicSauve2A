using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FicSauve2A
{
    public class cFTP
    {

        private Uri target;
        private NetworkCredential cred;

        public cFTP(string pUrl, string pUser, string pPassword)
        {
            target = new Uri(pUrl);
            cred = new NetworkCredential(pUser, pPassword);
        }



        /// <summary>
        /// Méthode qui va supprimer le dossier avec le nom passé en paramètres
        /// </summary>
        /// <param name="pDossier"></param>
        /// <returns></returns>
        public cErreur _supprimeDossier(string pDossier)
        {
            // Instanciation de l'objet Res de la classe cErreur
            cErreur Res = new cErreur();

            // Instanciation de l'objet request de la classe FtpWebRequest, avec la requête qui contiendra l'URI + le nom du dossier à supprimer
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + pDossier + "/");

            // La méthode de la classe WebRequest à utiliser est « RemoveDirectory »
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;

            // Récupère l'identifiant et le mot de passe du serveur FTP
            request.Credentials = cred;

            // Le serveur FTP envoie une réponse
            FtpWebResponse response;

            try
            {
                response = (FtpWebResponse)request.GetResponse();

                // L'accesseur de l'objet Res est se voit assigné la valeur « false » ; il n'y aura donc pas de message d'erreur
                Res.bErreur = false;
                Res.message = response.StatusDescription;

                // Petit message pour confirmer que le fichier a bien été supprimé
                MessageBox.Show($"Le dossier {pDossier} a bien été supprimé !");
                response.Close();

            }
            catch (Exception e)
            {
                Res.bErreur = true;
                Res.message = e.Message;
            }

            // Retourne le résultat sous la forme d'une chaîne de caractère
            return Res;
        }

        /// <summary>
        /// méthode qui va créer un dossier avec le nom passé en paramètres
        /// </summary>
        /// <param name="pDossier"></param>
        /// <returns></returns>
        public cErreur creerDossier(string pDossier)
        {
            // Instanciation de l'objet Res de la classe cErreur
            cErreur Res = new cErreur();

            // Instanciation de l'objet request de la classe FtpWebRequest, avec la requête qui contiendra l'URI + le nom du dossier à créer
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + pDossier + "/");

            // La méthode de la classe WebRequest à utiliser est « MakeDirectory »
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            // Récupère l'identifiant et le mot de passe du serveur FTP
            request.Credentials = cred;

            // Le serveur FTP envoie une réponse
            FtpWebResponse response;

            try
            {
                response = (FtpWebResponse)request.GetResponse();

                // L'accesseur de l'objet Res est se voit assigné la valeur « false » ; il n'y aura donc pas de message d'erreur
                Res.bErreur = false;
                Res.message = response.StatusDescription;

                // Petit message pour confirmer que le fichier a bien été créé
                //MessageBox.Show($"Le dossier {pDossier} a bien été créé !");
                response.Close();
            }
            catch (Exception e)
            {
                Res.bErreur = true;
                Res.message = e.Message;
            }

            // Retourne le résultat sous la forme d'une chaîne de caractère
            return Res;
        }

        /// <summary>
        /// Méthode qui va renommer le fichier avec le nom passé dans le paramètre nouveauNomFichier,
        /// le nom du fichier à modifier est quant à lui à passer dans le premier paramètre
        /// </summary>
        /// <param name="pFichier"></param>
        /// <param name="nouveauNomFichier"></param>
        /// <returns></returns>
        public cErreur renommeFichier(string pFichier, string nouveauNomFichier)
        {
            // Instanciation de l'objet Res de la classe cErreur
            cErreur Res = new cErreur();

            // Instanciation de l'objet request de la classe FtpWebRequest, avec la requête qui contiendra l'URI + le nom du fichier à modifier
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + pFichier);

            // La méthode de la classe WebRequest à utiliser est « Rename »
            request.Method = WebRequestMethods.Ftp.Rename;

            // Récupère l'identifiant et le mot de passe du serveur FTP
            request.Credentials = cred;

            // Renomme le fichier avec le nom passé en paramètre
            request.RenameTo = nouveauNomFichier;

            // Le serveur FTP envoie une réponse
            FtpWebResponse response;

            try
            {
                response = (FtpWebResponse)request.GetResponse();

                // L'accesseur de l'objet Res est se voit assigné la valeur « false » ; il n'y aura donc pas de message d'erreur
                Res.bErreur = false;

                Res.message = response.StatusDescription;

                // Petit message pour confirmer que le fichier a bien été renommé
                MessageBox.Show($"Le fichier {pFichier} a bien été renommé en {nouveauNomFichier} !");

                response.Close();
            }
            catch (Exception e)
            {
                Res.bErreur = true;
                Res.message = e.Message;
            }

            // Retourne le résultat sous la forme d'une chaîne de caractère
            return Res;
        }

        /// <summary>
        /// Méthode qui va transférer le fichier avec le nom passé en paramètre dans pFichier. Le chemin du fichier est à placer
        /// à la place du deuxième paramètre
        /// </summary>
        /// <param name="pFichier"></param>
        /// <param name="cheminDuFichier"></param>
        /// <returns></returns>
        public cErreur fichierTransfert(List<cFichier> fichiers, ProgressBar progressBar, int TailleMax = 0)
        {
            // Instanciation de l'objet Res de la classe cErreur
            cErreur Res = new cErreur();
            FtpState state = new FtpState();

            int valeurEnCoursPB = 0;

            foreach (cFichier fichier in fichiers)
            {

                //ManualResetEvent waitObject;

                // Instanciation de l'objet request de la classe FtpWebRequest, avec la requête qui contiendra l'URI + le nom du fichier à transférer
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + fichier.cheminDistant);

                // La méthode de la classe WebRequest à utiliser est « UploadFile »
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // Récupère l'identifiant et le mot de passe du serveur FTP
                request.Credentials = cred;
                state.Request = request;
                state.FileName = fichier.cheminLocal;
                //waitObject = state.OperationComplete;
                //request.BeginGetRequestStream(
                //    new AsyncCallback(AsynchronousFtpUpLoader.EndGetStreamCallback),
                //    state
                //);

                using (Stream fileStream = File.OpenRead(fichier.cheminLocal))
                using (Stream ftpStream = request.GetRequestStream())
                {

                    if (TailleMax == 0)
                    {
                        TailleMax = (int)fileStream.Length;
                    }
                    else
                    {
                        fichier.taille = (int)fileStream.Length;
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

                //waitObject.WaitOne();

                valeurEnCoursPB += fichier.taille;

                if (state.OperationException != null)
                {
                    Res.bErreur = true;
                    Res.message = state.OperationException.Message;
                }
                else
                {
                    // L'accesseur de l'objet Res est se voit assigné la valeur « false » ; il n'y aura donc pas de message d'erreur
                    Res.bErreur = false;
                    Res.message = state.StatusDescription;
                }
            }
            // Retourne le résultat sous la forme d'une chaîne de caractère
            return Res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cheminDuDossier"></param>
        /// <param name="pDossier"></param>
        /// <returns></returns>
        public cErreur dossierRecursifTransfert(string cheminDuDossier, ProgressBar progressBar, bool bRecursif = false)
        {
            // Instanciation de l'objet Res de la classe cErreur
            cErreur Res = new cErreur();

            int tailleMax = 0;
            List<cFichier> fichiersATransferer = new List<cFichier>();

            string dossierRacine = Path.GetDirectoryName(cheminDuDossier);
            dossierRacine = dossierRacine.Split('\\').Last<string>();

            creerDossier(dossierRacine);

            IEnumerable<string> fichiers = Directory.EnumerateFiles(cheminDuDossier);
            foreach (string fichier in fichiers)
            {
                string cheminDestination = dossierRacine + "/";

                cFichier tmp = new cFichier();
                tmp.cheminLocal = cheminDuDossier + Path.GetFileName(fichier);
                tmp.cheminDistant = cheminDestination + Path.GetFileName(fichier);

                using (Stream fileStream = File.OpenRead(tmp.cheminLocal))
                {
                    tmp.taille = (int)fileStream.Length;
                    tailleMax += tmp.taille;
                }

                fichiersATransferer.Add(tmp);

            }


            if (bRecursif)
            {
                ////////////////
                IEnumerable<string> dossiers = Directory.EnumerateDirectories(cheminDuDossier);
                foreach (string dossier in dossiers)
                {
                    string name = dossier.Split('\\').Last<string>();

                    creerDossier(dossierRacine + "/" + name);

                    IEnumerable<string> fichiersDeux = Directory.EnumerateFiles(dossier);
                    foreach (string fichier in fichiersDeux)
                    {
                        string cheminDestination = dossierRacine + "/" + name + "/";

                        cFichier tmp = new cFichier();
                        tmp.cheminLocal = fichier;
                        tmp.cheminDistant = cheminDestination + Path.GetFileName(fichier);

                        using (Stream fileStream = File.OpenRead(tmp.cheminLocal))
                        {
                            tmp.taille = (int)fileStream.Length;
                            tailleMax += tmp.taille;
                        }

                        fichiersATransferer.Add(tmp);
                    }

                }
            }

            try
            {
                Task.Run(() => fichierTransfert(fichiersATransferer, progressBar, tailleMax));
            }
            catch (Exception e)
            {
                Res.bErreur = true;
                Res.message = e.Message;
            }

            return Res;
        }

        public cErreur downloadFile(string cheminLocalTelechargement, string nomFichier)
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
