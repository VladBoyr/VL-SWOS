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
            var countryNames = new Dictionary<SwosCountry, HashSet<string>>();
            var countrySurnames = new Dictionary<SwosCountry, HashSet<string>>();

            swosService.SetSwosPath(swosPath + @"\DATA\");

            foreach (var swosFile in SwosFileType.AnyTeam.GetSwosFiles())
            {
                var teamsCount = await swosService.OpenSwosFile(swosFile.Value);
                for (int teamId = 0; teamId < teamsCount; teamId++)
                    for (int playerId = 0; playerId < SwosTeam.PlayersCount; playerId++)
                    {
                        var playerCountry = await swosService.ReadPlayerCountry(teamId, playerId);
                        var playerName = await swosService.ReadPlayerName(teamId, playerId);
                        var spacePositions = playerName
                            .Select((symbol, index) => new { symbol, index })
                            .Where(x => x.symbol == ' ')
                            .Select(x => x.index)
                            .ToArray();
                        foreach (var spacePos in spacePositions)
                        {
                            AddName(countryNames, playerCountry, playerName[..spacePos]);
                            AddName(countrySurnames, playerCountry, playerName[(spacePos + 1)..]);
                        }
                        if (spacePositions.Length == 0)
                        {
                            AddName(countryNames, playerCountry, playerName);
                            AddName(countrySurnames, playerCountry, playerName);
                        }
                    }
                swosService.CloseSwosFile();
            }
            
            if (!Directory.Exists(swosPath + $@"\NAMEFAM\"))
                Directory.CreateDirectory(swosPath + $@"\NAMEFAM\");

            foreach (var (country, names) in countryNames)
            {
                await SaveNamesToFile(swosPath + $@"\NAMEFAM\{(byte)country}.n", names);                
            }

            foreach (var (country, surnames) in countrySurnames)
            {
                await SaveNamesToFile(swosPath + $@"\NAMEFAM\{(byte)country}.f", surnames);
            }
        }

        private static void AddName(Dictionary<SwosCountry, HashSet<string>> dictionary, SwosCountry country, string name)
        {
            if (!dictionary.ContainsKey(country))
                dictionary.Add(country, []);
            dictionary[country].Add(name);
        }

        private static async Task SaveNamesToFile(string fileName, HashSet<string> names)
        {
            using var namesFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);

            foreach (var name in names)
            {
                if (namesFile.Position != 0)
                {
                    var separator = new byte[1] { 0 };
                    await namesFile.WriteAsync(separator);
                }

                var buffer = NameToByteArray(name);
                await namesFile.WriteAsync(buffer);
            }

            namesFile.Close();
        }

        private static byte[] NameToByteArray(string name)
        {
            var buffer = new byte[name.Length];
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)name[i];
            }
            return buffer;
        }
    }
}