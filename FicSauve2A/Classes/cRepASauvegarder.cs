// <copyright file="cRepASauvegarder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FicSauve2A
{
    /// <summary>
    /// Classe CRepASauvegarder.cs.
    /// </summary>
    internal class CRepASauvegarder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CRepASauvegarder"/> class.
        /// Constructeur de la classe CRepASauvegarder.
        /// </summary>
        /// <param name="pPath">Le chemin du répertoire à spécifier.</param>
        /// <param name="pBRecursif">Savoir si le répertoire est récursif ou non.</param>
        public CRepASauvegarder(string pPath, bool pBRecursif)
        {
            this.Path = pPath;
            this.BRecursif = pBRecursif;
        }

        /// <summary>
        /// Gets or Sets Path qui est le chemin du répertoire à sauvegarder sur le serveur.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or Sets BRecursif, booléen qui vérifie si oui ou non le répertoire est récursif.
        /// </summary>
        public bool BRecursif { get; set; }
    }
}
