using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class ServerModel
    {
        private List<SocketModel> mList;

        public ServerModel GetInstance()
        {
            if (mList == null)
                mList = new List<SocketModel>();
            return this;
        }

        public ServerModel(List<SocketModel> mList, long numbers)
        {
            this.mList = mList;
        }
        public ServerModel()
        {
        }
        public int GetSocketCounts()
        {
            return mList.Count;
        }

        internal void Add(SocketModel currentSocket)
        {
            mList.Add(currentSocket);
        }

        internal void Remove(SocketModel socket)
        {
            mList.Remove(socket);
        }
    }
}
