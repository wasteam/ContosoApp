using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.DataProviders
{
    public abstract class DataProviderBase<T> where T : SchemaBase
    {
        public abstract Task<IEnumerable<T>> LoadDataAsync();

        public virtual bool IsLocal
        {
            get
            {
                return false;
            }
        }
    }

    public abstract class DataProviderBase<TConfig, TSchema> : DataProviderBase<TSchema> where TSchema : SchemaBase
    {
        protected TConfig _config;

        protected IParser<TSchema> _parser;

        public DataProviderBase(TConfig config)
        {
            _config = config;
        }

        public DataProviderBase(TConfig config, IParser<TSchema> parser)
        {
            _config = config;
            _parser = parser;
        }
    }
}
