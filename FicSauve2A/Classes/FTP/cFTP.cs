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
        public cErreur fichierTransfert(string pFichier, string cheminDuFichier, ProgressBar progressBar, BackgroundWorker worker)
        {
            // Instanciation de l'objet Res de la classe cErreur
            cErreur Res = new cErreur();

            ManualResetEvent waitObject;
            FtpState state = new FtpState();

            // Instanciation de l'objet request de la classe FtpWebRequest, avec la requête qui contiendra l'URI + le nom du fichier à transférer
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target + pFichier);

            // La méthode de la classe WebRequest à utiliser est « UploadFile »
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Récupère l'identifiant et le mot de passe du serveur FTP
            request.Credentials = cred;
            state.Request = request;
            state.FileName = cheminDuFichier;
            waitObject = state.OperationComplete;
            //request.BeginGetRequestStream(
            //    new AsyncCallback(AsynchronousFtpUpLoader.EndGetStreamCallback),
            //    state
            //);

            using (Stream fileStream = File.OpenRead(cheminDuFichier))
            using (Stream ftpStream = request.GetRequestStream())
            {
                if (progressBar != null)
                    progressBar.Invoke(
                        (MethodInvoker)delegate { progressBar.Maximum = (int)fileStream.Length; });

                byte[] buffer = new byte[10240];
                int read;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ftpStream.Write(buffer, 0, read);
                    worker.ReportProgress((int)(fileStream.Position * 100 / fileStream.Length)); 
                }
                fileStream.Close();
                ftpStream.Close();
            }
            
           waitObject.WaitOne();

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

            // Retourne le résultat sous la forme d'une chaîne de caractère
            return Res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cheminDuDossier"></param>
        /// <param name="pDossier"></param>
        /// <returns></returns>
        public cErreur dossierRecursifTransfert(string cheminDuDossier, ProgressBar progressBar, BackgroundWorker worker, bool bRecursif = false)
        {
            // Instanciation de l'objet Res de la classe cErreur
            cErreur Res = new cErreur();

            string dossierRacine = Path.GetDirectoryName(cheminDuDossier);
            dossierRacine = dossierRacine.Split('\\').Last<string>();

            try
            {

                creerDossier(dossierRacine);
                

                IEnumerable<string> fichiers = Directory.EnumerateFiles(cheminDuDossier);
                foreach (string fichier in fichiers)
                {

                    //MessageBox.Show($"Transfert de: {fichier}");
                    string cheminDestination = dossierRacine + "/";
                    Task.Run(() => fichierTransfert(cheminDestination + Path.GetFileName(fichier), cheminDuDossier + Path.GetFileName(fichier),progressBar, worker));

                }
            }
            catch (Exception e)
            {
                Res.bErreur = true;
                Res.message = e.Message;
            }


            if (bRecursif)
            {
                ////////////////
                IEnumerable<string> dossiers = Directory.EnumerateDirectories(cheminDuDossier);
                foreach (string dossier in dossiers)
                {
                    string name = dossier.Split('\\').Last<string>();


                    try
                    {

                        creerDossier(dossierRacine + "/" + name);

                        IEnumerable<string> fichiersDeux = Directory.EnumerateFiles(dossier);
                        foreach (string fichier in fichiersDeux)
                        {
                            //MessageBox.Show($"Transfert de: {fichier}");
                            string cheminDestination = dossierRacine + "/" + name + "/";
                            Task.Run(() => fichierTransfert(cheminDestination + Path.GetFileName(fichier), fichier, progressBar, worker));
                        }

                    }
                    catch (Exception e)
                    {
                        Res.bErreur = true;
                        Res.message = e.Message;
                    }
                }
            }

            return Res;
        }

       
    }
}
