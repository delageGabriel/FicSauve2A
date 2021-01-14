using System;
using System.Collections.Generic;
using System.IO;
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

        public List<string> lireRepertoire()
        {
            data = parser.ReadFile(chemin);

            bool play = true;
            int i = 1;
            List<string> listRep = new List<string>();
            string fmt = "000.##";


            while (play)
            {

                string resTemp = lireIni("Repertoires", "Rep_" + i.ToString(fmt));


                if (resTemp == "")
                {
                    play = false;
                }
                else
                {
                    listRep.Add(resTemp);
                }

                i++;
            }

            return listRep;

        }

        public void sauvegarderRepertoire()
        {
            data = parser.ReadFile(chemin);

            bool play = true;
            int i = 1;
            string fmt = "000.##";


            while (play)
            {

                string resTemp = lireIni("RepertoiresRecursif", "Rep_" + i.ToString(fmt));

                i++;
            }


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
            catch (Exception e)
            {
                res.bErreur = true;
                res.message = e.Message;
            }

            return res;
        }

        public string test()
        {
            int value = 160934;
            int decimalLength = value.ToString("D").Length + 5;
            return value.ToString("D" + decimalLength.ToString());
            // The example displays the following output:
            //       00000160934
            //       00000274A6
        }
    }
}
