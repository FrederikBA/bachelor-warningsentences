using System.Text.Json;
using Microsoft.Extensions.Logging;
using Shared.Integration.Authorization;
using Shared.Integration.Configuration;
using Shared.Integration.Models.Dtos;
using Shared.Integration.Models.Dtos.Sync;
using WS.Core.Entities.WSAggregate;
using WS.Core.Exceptions;
using WS.Core.Interfaces.DomainServices;
using WS.Core.Interfaces.Integration;
using WS.Core.Interfaces.Repositories;
using WS.Core.Models.Dtos;
using WS.Core.Specifications;

namespace WS.Core.Services.DomainServices;

public class WarningSentenceService : IWarningSentenceService
{
    private readonly IReadRepository<WarningSentence> _warningSentenceReadRepository;
    private readonly IRepository<WarningSentence> _warningSentenceRepository;
    private readonly ISyncProducer _syncProducer;
    private readonly HttpClient _httpClient;
    private readonly ILogger<WarningSentenceService> _logger;

    public WarningSentenceService(IReadRepository<WarningSentence> warningSentenceReadRepository,
        IRepository<WarningSentence> warningSentenceRepository, ISyncProducer syncProducer,
        ILogger<WarningSentenceService> logger)
    {
        _warningSentenceReadRepository = warningSentenceReadRepository;
        _warningSentenceRepository = warningSentenceRepository;
        _syncProducer = syncProducer;
        _logger = logger;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization",
            "Bearer " + IntegrationAuthService.GetIntegrationToken());
    }

    public async Task<List<WarningSentence>> GetAllWarningSentencesAsync()
    {
        var warningSentences = await _warningSentenceReadRepository.ListAsync(new GetWarningSentencesFullSpec());

        if (warningSentences == null || warningSentences.Count == 0)
        {
            throw new WarningSentencesNotFoundException();
        }

        return warningSentences;
    }

    public async Task<WarningSentence> GetWarningSentenceByIdAsync(int id)
    {
        var warningSentence =
            await _warningSentenceReadRepository.FirstOrDefaultAsync(new GetWarningSentenceByIdFullSpec(id));

        if (warningSentence == null)
        {
            throw new WarningSentenceNotFoundException(id);
        }

        return warningSentence;
    }

    public async Task<WarningSentence> AddWarningSentenceAsync(WarningSentenceDto warningSentenceDto)
    {
        var warningSentence = new WarningSentence
        {
            Code = warningSentenceDto.Code,
            Text = warningSentenceDto.Text,
            WarningCategoryId = warningSentenceDto.WarningCategoryId,
            WarningSignalWordId = warningSentenceDto.WarningSignalWordId,
            WarningPictogramId = warningSentenceDto.WarningPictogramId
        };

        var result = await _warningSentenceRepository.AddAsync(warningSentence);

        //Sync warning sentence with SEA database
        await _syncProducer.ProduceAsync(Config.Kafka.Topics.SyncAddWs,
            new SyncWarningSentenceDto { WarningSentenceId = result.Id, Code = result.Code });

        _logger.LogInformation($"Syncing new warning sentence with SEA database. Id: {result.Id}, Code: {result.Code}");

        return result;
    }

    public async Task<IEnumerable<WarningSentence>> CloneWarningSentenceAsync(List<int> ids)
    {
        //Get the warning sentence that needs to be cloned
        var warningSentences = await _warningSentenceReadRepository.ListAsync();

        //Filter the warning sentences that needs to be cloned (ids)
        warningSentences = warningSentences.Where(w => ids.Contains(w.Id)).ToList();

        if (warningSentences == null || warningSentences.Count == 0)
        {
            throw new WarningSentencesNotFoundException();
        }

        var clonedWarningSentences = new List<WarningSentence>();

        foreach (var warningSentence in warningSentences)
        {
            //Clone the warning sentence
            var clonedWarningSentence = new WarningSentence
            {
                Code = warningSentence.Code + " (Copy)", //Affix (Copy) to the code to indicate that it is a copy
                Text = warningSentence.Text,
                WarningCategoryId = warningSentence.WarningCategoryId,
                WarningSignalWordId = warningSentence.WarningSignalWordId,
                WarningPictogramId = warningSentence.WarningPictogramId
            };

            //Add the cloned warning sentence to the list
            clonedWarningSentences.Add(clonedWarningSentence);
        }


        var result = await _warningSentenceRepository.AddRangeAsync(clonedWarningSentences);
        var cloneWarningSentenceAsync = result.ToList();

        //Sync warning sentence with SEA database
        foreach (var ws in cloneWarningSentenceAsync)
        {
            await _syncProducer.ProduceAsync(Config.Kafka.Topics.SyncAddWs,
                new SyncWarningSentenceDto
                    { WarningSentenceId = ws.Id, Code = ws.Code });

            _logger.LogInformation($"Syncing new warning sentence with SEA database. Id: {ws.Id}, Code: {ws.Code}");
        }


        return cloneWarningSentenceAsync;
    }

    public async Task<WarningSentence> UpdateWarningSentenceAsync(int id, WarningSentenceDto warningSentenceDto)
    {
        var warningSentence = await _warningSentenceReadRepository.GetByIdAsync(id);

        if (warningSentence == null)
        {
            throw new WarningSentenceNotFoundException(id);
        }

        //Update the warning sentence
        warningSentence.Code = warningSentenceDto.Code;
        warningSentence.Text = warningSentenceDto.Text;
        warningSentence.WarningCategoryId = warningSentenceDto.WarningCategoryId;
        warningSentence.WarningSignalWordId = warningSentenceDto.WarningSignalWordId;
        warningSentence.WarningPictogramId = warningSentenceDto.WarningPictogramId;

        await _warningSentenceRepository.UpdateAsync(warningSentence);

        //Sync warning sentence with SEA database on update
        await _syncProducer.ProduceAsync(Config.Kafka.Topics.SyncUpdateWs,
            new SyncWarningSentenceDto { WarningSentenceId = warningSentence.Id, Code = warningSentence.Code });

        _logger.LogInformation(
            $"Syncing updated warning sentence with SEA database. Id: {warningSentence.Id}, Code: {warningSentence.Code}");

        return warningSentence;
    }

    public async Task<WarningSentence> DeleteWarningSentenceAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync(Config.IntegrationEndpoints.ActiveWarningSentencesIntegration);
            var content = await response.Content.ReadAsStringAsync();
            var activeWarningSentences = JsonSerializer.Deserialize<SharedProductWsDto>(content);

            //Check if given warning sentence is in use
            var itemInUse = activeWarningSentences!.WarningSentenceIds.Contains(id);

            //Throw exception if warning sentence is in use
            if (itemInUse)
            {
                throw new WarningSentenceIsInUseException(id);
            }

            //Delete the warning sentence
            var warningSentence = await _warningSentenceReadRepository.GetByIdAsync(id);

            if (warningSentence == null)
            {
                throw new WarningSentenceNotFoundException(id);
            }

            await _warningSentenceRepository.DeleteAsync(warningSentence);

            //Sync warning sentence with SEA database
            await _syncProducer.ProduceAsync(Config.Kafka.Topics.SyncDeleteWs,
                new SyncWarningSentenceDto { WarningSentenceId = warningSentence.Id, Code = warningSentence.Code });

            return warningSentence;
        }
        catch (Exception e)
        {
            _logger.LogError($"Warning sentence with id {id} is in use and cannot be deleted.");
            throw;
        }
    }
}