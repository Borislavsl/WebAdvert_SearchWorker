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
                var url = Environment.GetEnvironmentVariable("ES_URL"); ;
                var settings = new ConnectionSettings(new Uri(url))
                    .DefaultIndex("adverts")
                    .DefaultMappingFor<AdvertType>(d => d.IndexName("advert"))
                    .DefaultMappingFor<AdvertType>(m => m.IdProperty(x => x.Id));

                _client = new ElasticClient(settings);
            }

            return _client;
        }
    }
}
