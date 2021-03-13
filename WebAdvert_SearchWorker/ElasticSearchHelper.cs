using System;
using Nest;

namespace WedbAdvert_SearchWorker
{
    public static class ElasticSearchHelper
    {
        private static IElasticClient _client;

        public static IElasticClient GetInstance()
        {
            if (_client == null)
            {
                string url = Environment.GetEnvironmentVariable("ES_URL");
                string userName = Environment.GetEnvironmentVariable("USER_NAME");
                string password = Environment.GetEnvironmentVariable("PASSWORD");

                var settings = new ConnectionSettings(new Uri(url))
                    .BasicAuthentication(userName, password)
                    .DefaultIndex("adverts")
                    .DefaultMappingFor<AdvertType>(d => d.IndexName("advert"))
                    .DefaultMappingFor<AdvertType>(m => m.IdProperty(x => x.Id));

                _client = new ElasticClient(settings);
            }

            return _client;
        }
    }
}
