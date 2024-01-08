using Swos.Domain;
using Swos.Domain.Models;

namespace SWOS.CareerMod
{
    public class CountryNamesExtractor
    {
        private readonly ISwosService swosService;

        public CountryNamesExtractor(ISwosService swosService)
        {
            this.swosService = swosService;
        }

        public async Task Extract(string swosPath)
        {
            var countryNames = new Dictionary<SwosCountry, Dictionary<string, int>>();
            var countrySurnames = new Dictionary<SwosCountry, Dictionary<string, int>>();

            swosService.SetSwosPath(swosPath + @"\DATA\");

            foreach (var swosFile in new SwosFileType[] { SwosFileType.Club, SwosFileType.NationalTeam, SwosFileType.CustomTeam }.GetSwosFiles())
            {
                var teamsCount = await swosService.OpenSwosFile(swosFile.Value);
                for (int teamId = 0; teamId < teamsCount; teamId++)
                    for (int playerId = 0; playerId < SwosTeam.PlayersCount; playerId++)
                    {
                        var playerCountry = await swosService.ReadPlayerCountry(teamId, playerId);
                        var playerName = await swosService.ReadPlayerName(teamId, playerId);
                        var names = playerName.Split(' ');
                        if (names.Length == 1)
                        {
                            AddName(countryNames, playerCountry, playerName);
                            AddName(countrySurnames, playerCountry, playerName);
                        }
                        else
                        {
                            foreach (var name in names.Take(names.Length - 1))
                            {
                                AddName(countryNames, playerCountry, name);
                            }
                            for (var i = 2; i < names.Length; i++)
                            {
                                AddName(countryNames, playerCountry, string.Join(' ', names.Take(i)));
                            }
                            for (var i = 1; i < names.Length; i++)
                            {
                                AddName(countrySurnames, playerCountry, string.Join(' ', names.Skip(i)));
                            }
                        }
                    }
                swosService.CloseSwosFile();
            }
            
            if (!Directory.Exists(swosPath + $@"\NAMEFAM\"))
                Directory.CreateDirectory(swosPath + $@"\NAMEFAM\");

            foreach (var (country, names) in countryNames)
            {
                await SaveNamesToFile(swosPath + $@"\NAMEFAM\{(byte)country:D3}.nam", names);                
            }

            foreach (var (country, surnames) in countrySurnames)
            {
                await SaveNamesToFile(swosPath + $@"\NAMEFAM\{(byte)country:D3}.fam", surnames);
            }
        }

        private static void AddName(Dictionary<SwosCountry, Dictionary<string, int>> dictionary, SwosCountry country, string name)
        {
            dictionary.TryAdd(country, []);
            dictionary[country].TryAdd(name, 0);
            dictionary[country][name]++;
        }

        private static async Task SaveNamesToFile(string fileName, Dictionary<string, int> dictionary)
        {
            var names = dictionary.Select(x => $"{x.Key}={x.Value}");
            await File.WriteAllLinesAsync(fileName, names);
        }
    }
}