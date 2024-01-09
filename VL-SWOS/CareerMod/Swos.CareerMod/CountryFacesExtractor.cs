using Swos.Domain;
using Swos.Domain.Models;

namespace Swos.CareerMod
{
    public class CountryFacesExtractor
    {
        private readonly ISwosService swosService;

        public CountryFacesExtractor(ISwosService swosService)
        {
            this.swosService = swosService;
        }

        public async Task Extract(string swosPath)
        {
            var countryFaces = new Dictionary<SwosCountry, Dictionary<SwosFace, int>>();

            swosService.SetSwosPath(swosPath + @"\DATA\");

            foreach (var swosFile in new SwosFileType[] { SwosFileType.Club, SwosFileType.NationalTeam }.GetSwosFiles())
            {
                var teamsCount = await swosService.OpenSwosFile(swosFile.Value);
                for (int teamId = 0; teamId < teamsCount; teamId++)
                    for (int playerId = 0; playerId < SwosTeam.PlayersCount; playerId++)
                    {
                        var playerCountry = await swosService.ReadPlayerCountry(teamId, playerId);
                        var (_, playerFace) = await swosService.ReadPlayerPositionAndFace(teamId, playerId);
                        countryFaces.TryAdd(playerCountry, []);
                        countryFaces[playerCountry].TryAdd(playerFace, 0);
                        countryFaces[playerCountry][playerFace]++;
                    }
                swosService.CloseSwosFile();
            }

            if (!Directory.Exists(swosPath + $@"\Country\"))
                Directory.CreateDirectory(swosPath + $@"\Country\");

            foreach (var (country, faces) in countryFaces)
            {
                await File.WriteAllLinesAsync(swosPath + $@"\Country\{(byte)country:D3}.face", faces.Select(x => $"{x.Key}={x.Value}"));
            }
        }
    }
}