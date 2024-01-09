using Swos.Domain;
using Swos.Domain.Models;

namespace Swos.CareerMod
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
            var countrySingleNames = new Dictionary<SwosCountry, Dictionary<bool, int>>();

            swosService.SetSwosPath(swosPath + @"\DATA\");

            foreach (var swosFile in new SwosFileType[] { SwosFileType.Club, SwosFileType.NationalTeam }.GetSwosFiles())
            {
                var teamsCount = await swosService.OpenSwosFile(swosFile.Value);
                for (int teamId = 0; teamId < teamsCount; teamId++)
                    for (int playerId = 0; playerId < SwosTeam.PlayersCount; playerId++)
                    {
                        var playerCountry = await swosService.ReadPlayerCountry(teamId, playerId);
                        var playerName = await swosService.ReadPlayerName(teamId, playerId);
                        var names = playerName.Split([' ', '.']);
                        if (names.Length == 1)
                        {
                            countrySingleNames.TryAdd(playerCountry, []);
                            countrySingleNames[playerCountry].TryAdd(true, 0);
                            countrySingleNames[playerCountry][true]++;

                            AddName(countryNames, playerCountry, playerName);
                            AddName(countrySurnames, playerCountry, playerName);
                        }
                        else
                        {
                            countrySingleNames.TryAdd(playerCountry, []);
                            countrySingleNames[playerCountry].TryAdd(false, 0);
                            countrySingleNames[playerCountry][false]++;

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

            if (!Directory.Exists(swosPath + $@"\Country\"))
                Directory.CreateDirectory(swosPath + $@"\Country\");

            foreach (var (country, names) in countryNames)
            {
                await File.WriteAllLinesAsync(swosPath + $@"\Country\{(byte)country:D3}.name", names.Select(x => $"{x.Key}={x.Value}"));
            }

            foreach (var (country, surnames) in countrySurnames)
            {
                await File.WriteAllLinesAsync(swosPath + $@"\Country\{(byte)country:D3}.surname", surnames.Select(x => $"{x.Key}={x.Value}"));
            }

            foreach (var (country, singleNames) in countrySingleNames.Where(x => SwosCountryExtension.CountrySingleName.Contains(x.Key)))
            {
                await File.WriteAllLinesAsync(swosPath + $@"\Country\{(byte)country:D3}.singlename", singleNames.Select(x => $"{x.Key}={x.Value}"));
            }
        }

        private static void AddName(Dictionary<SwosCountry, Dictionary<string, int>> dictionary, SwosCountry country, string name)
        {
            dictionary.TryAdd(country, []);
            dictionary[country].TryAdd(name, 0);
            dictionary[country][name]++;
        }
    }
}