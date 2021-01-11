using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;

namespace FicSauve2A
{
    class INI
    {
        public string chemin { get; set; }
        private IniData data;
        private FileIniDataParser parser;
        public INI(string chemin)
        {
            this.chemin = chemin;
            parser = new FileIniDataParser();
        }

        public string lireIni(string sectionName, string keyName)
        {
            data = parser.ReadFile(chemin);
            string res = data[sectionName][keyName];
            return res;
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
            catch(Exception e)
            {
                res.bErreur = true;
                res.message = e.Message;
            }

            return res;
        }
    }
}
