using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Server;
using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public class UserStoreWillReplaceInFuture
    {
        private UserStoreWillReplaceInFuture()
        {
            Init();
        }

        public static readonly UserStoreWillReplaceInFuture instance = new();

        public List<User> Users { get; private set; }

        /// <summary>
        /// This is for test only, please help resolve!
        /// </summary>
        public void Init()
        {
            Users = new List<User>();
        }
    }
}