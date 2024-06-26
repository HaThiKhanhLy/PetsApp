using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.ViewModel
{
   
    class UserVM
    {
        public UserModels users { get; set; }
       public UserVM() {
            this.users = new UserModels();
                }
      
    }
}
