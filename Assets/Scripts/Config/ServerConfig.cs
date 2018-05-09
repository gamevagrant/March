using System;
using System.Collections.Generic;

namespace Core.March.Config
{
    [Serializable]
    public class ServerConfig
    {
        public int CurrentIndex;
        public List<Server> AvailableList;

        public Server Current
        {
            get { return AvailableList[CurrentIndex]; }
        }
    }

    [Serializable]
    public class Server
    {
        public string Name;
        public string Url;
    }
}
