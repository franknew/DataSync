
using Chainway.Library.SimpleMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Entity
{
    public abstract class BaseEntity
    {
        [Ingore]
        public EntityStatusEnum Status { get; set; }

        public void ConvertStatus(string status)
        {
            switch (status.ToLower())
            {
                case "insert":
                    Status = EntityStatusEnum.Insert;
                    break;
                case "update":
                    Status = EntityStatusEnum.Update;
                    break;
                case "delete":
                    Status = EntityStatusEnum.Delete;
                    break;
                case "insertorupdate":
                    Status = EntityStatusEnum.InsertOrUpdate;
                    break;
                default:
                    Status = EntityStatusEnum.Insert;
                    break;
            }
        }
    }

    public enum EntityStatusEnum
    {
        Insert,
        Update,
        Delete,
        InsertOrUpdate,

    }
}