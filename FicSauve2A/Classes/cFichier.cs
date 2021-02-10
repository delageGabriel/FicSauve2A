// <copyright file="cFichier.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FicSauve2A
{
    /// <summary>
    /// Classe public CFichier.
    /// </summary>
    public class CFichier
    {
        ///////////////////////////////////////////////////////////
        // ACCESSEUR/MUTATEUR
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets CheminLocal qui est le chemin local du fichier.
        /// </summary>
        public string CheminLocal { get; set; }

        /// <summary>
        /// Gets or sets CheminDistant qui est le chemin sur le serveur ftp.
        /// </summary>
        public string CheminDistant { get; set; }

        /// <summary>
        /// Gets or sets Taille qui est le poids du fichier (en octets).
        /// </summary>
        public int Taille { get; set; }
    }
}
