using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace estadisticasapi
{
    internal class RedisDB
    {
        private static Lazy<ConnectionMultiplexer> _lazyConnection;

        private static ConnectionMultiplexer Connection
        {
            get { return _lazyConnection.Value; }
        }
        public static IDatabase PaisDB
        {
            get { return Connection.GetDatabase(1); }        
        }
        public static IDatabase CotizacionDB
        {
            get { return Connection.GetDatabase(2); }
        }
        public static IDatabase IpDB
        {
            get { return Connection.GetDatabase(3); }
        }
        public static IDatabase EstadisticasDB
        {
            get { return Connection.GetDatabase(4); }
        }
        static RedisDB()
        {
           
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    string redisServer = Environment.GetEnvironmentVariable("redisServer");
                    if (redisServer == null)
                        redisServer = "192.168.0.175";

                    return ConnectionMultiplexer.Connect(redisServer);
                }
                
            );
          
        }
    }
}
