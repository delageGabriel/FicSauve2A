using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FicSauve2A
{
    class cRepASauvegarder
    {
        public string path { get; set; }
        public bool bRecursif { get; set; }

        public cRepASauvegarder(string pPath, bool pBRecursif)
        {
            path = pPath;
            bRecursif = pBRecursif;
        }

    }
}
