using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;

using RestSharp;

namespace ChrisUsher.MoveMate.App.Clients;

public class SavingsClient
{
    public SavingsClient(AppCommon appCommon)
    {
        _appCommon = appCommon;
    }
    
    private readonly string _apiSuffix = @"api/Accounts/{accountId}/Savings";
    private readonly AppCommon _appCommon;

    public async Task<List<SavingsAccount>> GetSavingsAsync(Guid accountId)
    {
        var response = await _appCommon.ApiClient.GetAsync(new RestRequest(_apiSuffix.Replace("{accountId}", accountId.ToString())));

        if (response.IsSuccessful)
        {
            return await JsonSerializer.DeserializeAsync<List<SavingsAccount>>(new MemoryStream(Encoding.UTF8.GetBytes(response.Content)));
        }

        return null;
    }
    
    public async Task<SavingsReport> GetSavingsReportAsync(Guid accountId)
    {
        var response = await _appCommon.ApiClient.GetAsync(new RestRequest($"api/Reports/SavingsReport/{accountId}?caseModel={CaseType.MiddleCase}"));

        if (response.IsSuccessful)
        {
            return await JsonSerializer.DeserializeAsync<SavingsReport>(new MemoryStream(Encoding.UTF8.GetBytes(response.Content)));
        }

        return null;
    }
}