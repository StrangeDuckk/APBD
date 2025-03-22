using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw_2_s30338
{
    public interface IHazardNotifier
    {
        void NiebezpiecznaSytuacja(string nrKontenera);
        void OproznijKontener();
    }
}
