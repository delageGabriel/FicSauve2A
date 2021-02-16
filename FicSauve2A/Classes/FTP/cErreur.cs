// <copyright file="cErreur.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FicSauve2A
{
    /// <summary>
    /// Classe cErreur.
    /// </summary>
    public class cErreur
    {
        ///////////////////////////////////////////////////////////
        // ATTRIBUTS
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets a value indicating whether gets or Sets BErreur, booléen.
        /// </summary>
        public bool BErreur { get; set; }

        /// <summary>
        /// Gets or sets Message qui retournera l'erreur.
        /// </summary>
        public string Message { get; set; }

        ///////////////////////////////////////////////////////////
        // CONSTRUCTEUR
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="cErreur"/> class.
        /// Constructeur de la classe cErreur.
        /// </summary>
        public cErreur()
        {

        }
    }
}
