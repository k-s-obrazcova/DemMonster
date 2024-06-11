using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp27
{
    
    public class Functional
    {
        public Session1_01Entities db = new Session1_01Entities();
        public int CountUsers()
        {
            return db.Users.ToList().Count;
        }
    }
}
