using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WepSerApp.Model
{
    class MyOverview : Bindable
    {
        private string overview;

        public string NamesInList
        {
            get { return overview; }
            set { overview = value;
                PropertyIsChanged();
            }
        }

    }
}
