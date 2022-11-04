using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WepSerApp.Model;

namespace WepSerApp.ViewModel
{
    class NewsAppViewModel : Bindable
    {
        private string groupsNameFromFavorit;

        public string GroupsNameFromFavorit
        {
            get { return groupsNameFromFavorit; }
            set { groupsNameFromFavorit = value;
                PropertyIsChanged();
            }
        }

        ServerCommunication serverCommunication;

        public ServerCommunication ServerCom
        {
            get { return serverCommunication; }
            set
            {
                serverCommunication = value;
                PropertyIsChanged();
            }
        }
        public ServerCommand AddServer { get; set; }
        public ServerCommand RemoveServer { get; set; }
        public ServerCommand ConnectToServer { get; set; }
        public ServerCommand SendMessage { get; set; }
        public ServerCommand GroupList { get; set; }
        public ServerCommand SeverHelpList { get; set; }
        public ServerCommand CleanWindow { get; set; }
        public ServerCommand AddToFavoritGroup { get; set; }
        public ServerCommand GetListArticles { get; set; }

        private MyFavoriteGroup favoriteGroup;
        public MyFavoriteGroup FavoriteGroup
        {
            get { return favoriteGroup; }
            set { favoriteGroup = value; 
                    PropertyIsChanged(); 
            }
        }

        private MyOverview overview;
        public MyOverview Overview
        {
            get { return overview; }
            set { overview = value;
                PropertyIsChanged();
            }
        }

        private MyServerList myServer;
        public MyServerList MyServer
        {
            get { return myServer; }
            set { myServer = value;
                PropertyIsChanged();
            }
        }

        private ObservableCollection<MyServerList> listOfServer = new ObservableCollection<MyServerList>();
        public ObservableCollection<MyServerList> ListOfServers
        {
            get { return listOfServer; }
            set { listOfServer = value;
                PropertyIsChanged();
            }
        }

        private ObservableCollection<MyOverview> overviewList = new ObservableCollection<MyOverview>();
        public ObservableCollection<MyOverview> OverviewList
        {
            get { return overviewList; }
            set { overviewList = value;
                PropertyIsChanged();
            }
        }

        private ObservableCollection<MyFavoriteGroup> favoriteGroupsList = new ObservableCollection<MyFavoriteGroup>();
        public ObservableCollection<MyFavoriteGroup> FavoriteGroupsList
        {
            get { return favoriteGroupsList; }
            set { favoriteGroupsList = value;
                PropertyIsChanged();
            }
        }

        public NewsAppViewModel()
        {
            myServer = new MyServerList();
            overview = new MyOverview();
            serverCommunication = new ServerCommunication();
            favoriteGroup = new MyFavoriteGroup();

            ListOfServers.Add(new MyServerList
            {
                Servername = "news.dotsrc.org",
                Port = 119,
                Username = "janx802y@easv365.dk",
                Password = "8d2d43"
            });

            ConnectToServer = new ServerCommand(CreateConnectToServer);
            SendMessage = new ServerCommand(SendInputToServer);
            AddServer = new ServerCommand(AddToServerList);
            GroupList = new ServerCommand(GetGroupList);
            SeverHelpList = new ServerCommand(GetHelpList);
            CleanWindow = new ServerCommand(CleanChatWindow);
            AddToFavoritGroup = new ServerCommand(AddGroup);
            GetListArticles = new ServerCommand(GetArticelList);
        }

        public void CreateConnectToServer(object parmeter)
        {
            serverCommunication.Connect(new MyServerList
            {
                Servername = MyServer.Servername,
                Port = MyServer.Port,
                Username = MyServer.Username,
                Password = MyServer.Password
            });
        }

        public void SendInputToServer(object parameter)
        {
            serverCommunication.SendTextToServer(new MyOverview { NamesInList = Overview.NamesInList });
        }

        public void AddToServerList(object parameter)
        {
            ListOfServers.Add(new MyServerList
            {
                Servername = MyServer.Servername,
                Port = MyServer.Port,
                Username = MyServer.Username,
                Password = MyServer.Password
            });
        }

        public void GetGroupList(object parameter)
        {
            OverviewList.Clear();
            OverviewList = new ObservableCollection<MyOverview>(serverCommunication.GetServerGroups());
        }

        public void GetHelpList(object parameter)
        {
            OverviewList.Clear();
            OverviewList = new ObservableCollection<MyOverview>(serverCommunication.GetServerCommands());
        }

        public void CleanChatWindow(object parameter)
        {
            serverCommunication.Clean();
        }

        public void AddGroup(object parameter)
        {
            Console.WriteLine(FavoriteGroup.GroupName);
            FavoriteGroupsList.Add(new MyFavoriteGroup { GroupName = Overview.NamesInList });
        }

        public void GetArticelList(object parameter)
        {
            GroupsNameFromFavorit = FavoriteGroup.GroupName;
            OverviewList = new ObservableCollection<MyOverview>(serverCommunication.ArticelInSelectedGroup(GroupsNameFromFavorit));
        }
    }
}
