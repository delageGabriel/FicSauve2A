namespace FicSauve2A
{
    partial class FicSauve2A
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(96, 59);
            this.progressBar.Maximum = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(248, 23);
            this.progressBar.TabIndex = 9;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // FicSauve2A
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 150);
            this.Controls.Add(this.progressBar);
            this.Name = "FicSauve2A";
            this.Text = "FicSauveA2";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

