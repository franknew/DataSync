using Chainway.Library.MQ;
using Chainway.Library.SimpleMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.BLL
{
    public class MapperFactory
    {
        public static ISqlMapper CreateMapper(EntityStatusEnum status, ISQLConvert converter)
        {
            ISqlMapper mapper = null;
            switch (status)
            {
                case EntityStatusEnum.Insert:
                    mapper = new InsertMapper(converter);
                    break;
                case EntityStatusEnum.Delete:
                    mapper = new DeleteByIDMapper(converter);
                    break;
                case EntityStatusEnum.Update:
                    mapper = new UpdateByIDMapper(converter);
                    break;
                case EntityStatusEnum.InsertOrUpdate:
                    mapper = new InsertOrUpdateMapper(converter);
                    break;
            }

            return mapper;
        }
    }
}
