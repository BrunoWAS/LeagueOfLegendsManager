using ApiLeague.Data.Repository.Interfaces;
using Newtonsoft.Json.Linq;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Services
{
    public class SyncService : ISyncService
    {
        private readonly HttpClient _httpClient;
        private readonly IClasseRepository _classeRepository;
        private readonly ICampeaoRepository _campeaoRepository;
        private readonly IConfiguration _configuration;

        public SyncService(HttpClient httpClient, IClasseRepository classeRepository, ICampeaoRepository campeaoRepository, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _classeRepository = classeRepository;
            _campeaoRepository = campeaoRepository;
            _configuration = configuration;
        }

        public async Task<string> SynchronizeDataAsync()
        {
            string apiUrl = _configuration["ApiLol-campeos"];
            var response = await _httpClient.GetStringAsync(apiUrl);
            var data = JObject.Parse(response)["data"];
            var champions = data.Children().Select(c => c.First).ToList();

            var existingClasses = (await _classeRepository.GetAllAsync()).ToDictionary(c => c.Nome, c => c.Id);
            var existingChampions = (await _campeaoRepository.GetAllAsync()).ToDictionary(c => c.Nome, c => c.Id);

            var newClasses = new List<Classe>();
            var newChampions = new List<Campeao>();
            var campeaoClasses = new List<CampeaoClasse>();

            foreach (var champion in champions)
            {
                string championName = champion["name"].ToString();
                string championTitle = champion["title"].ToString();
                var tags = champion["tags"].Select(t => t.ToString()).ToList();

                foreach (var tag in tags)
                {
                    if (!existingClasses.ContainsKey(tag))
                    {
                        var newClass = new Classe { Nome = tag };
                        newClasses.Add(newClass);
                    }
                }

                if (!existingChampions.ContainsKey(championName))
                {
                    var newChampion = new Campeao { Nome = championName, Title = championTitle };
                    newChampions.Add(newChampion);
                }
            }

            // Insert new classes and update existingClasses dictionary
            foreach (var newClass in newClasses)
            {
                await _classeRepository.AddAsync(newClass);
                existingClasses[newClass.Nome] = newClass.Id;
            }

            // Insert new champions and update existingChampions dictionary
            foreach (var newChampion in newChampions)
            {
                await _campeaoRepository.AddAsync(newChampion);
                existingChampions[newChampion.Nome] = newChampion.Id;
            }

            // Create relationships for CampeaoClasse table
            foreach (var champion in champions)
            {
                string championName = champion["name"].ToString();
                int championId = existingChampions[championName];
                var tags = champion["tags"].Select(t => t.ToString()).ToList();

                foreach (var tag in tags)
                {
                    int classId = existingClasses[tag];
                    campeaoClasses.Add(new CampeaoClasse { CampeaoId = championId, ClasseId = classId });
                }
            }

            // Insert CampeaoClasse relationships
            foreach (var campeaoClasse in campeaoClasses)
            {
                await _campeaoRepository.AddCampeaoClasseAsync(campeaoClasse);
            }

            return "Synchronization completed successfully.";
        }
    }
}
