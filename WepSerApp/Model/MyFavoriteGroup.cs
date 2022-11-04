using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WepSerApp.Model
{
    class MyFavoriteGroup : Bindable
    {
        private string groupName;

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value;
                PropertyIsChanged();
            }
        }
    }
}
