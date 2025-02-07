using Quartz;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Contracts.Query;
using Microsoft.Extensions.DependencyInjection;
using CryptoGateway.Service.Models.ContractAccount;
using CryptoGateway.Domain.Entities.ContractAccount;
using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi;

namespace CryptoGateway.Service.Quartz.Jobs.Payment;

[DisallowConcurrentExecution]
public class KeepServerAliveJob : IJob, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public KeepServerAliveJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            FileLoger.Message($"KeepServerAliveJob is started: {DateTime.Now.ToString("HH:mm:ss")}");
            using var scope = _serviceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetService<DbConnection>();
            var tronWeb = scope.ServiceProvider.GetService<ITronWeb>();

            var request = new ContractAccountQueryModels.GetContractAccountList();
            if (connection != null && tronWeb != null)
            {
                var contractAccounts = await connection.Query(request);

                var rnd = new Random();
                var rndIndex = rnd.Next(0, contractAccounts.ToList().Count() - 1);
                var contractAccount = contractAccounts.ToList()[rndIndex];
                var balance = await tronWeb.GetAccountBalanceAsync(contractAccount.AddressBase58);
                FileLoger.Message($"Balance: {balance.SerializeToJson()}");

                //foreach (var contractAccount in contractAccounts.ToList())
                //{
                //    var rnd = new Random();
                //    var rndTime = rnd.Next(1, 1000);
                //    Thread.Sleep(rndTime);
                //    FileLoger.Message($"Random time: {rndTime}");
                //    var balance = await tronWeb.GetAccountBalanceAsync(contractAccount.AddressBase58);
                //    FileLoger.Message($"Balance: {balance.SerializeToJson()}");
                //}
                FileLoger.Message($"KeepServerAliveJob is ended: {DateTime.Now.ToString("HH:mm:ss")}");
            }
        }
        catch (Exception e)
        {
            FileLoger.CriticalError(e);
            if (e.InnerException != null) FileLoger.CriticalError(e.InnerException);
            //var executionException = new JobExecutionException(e)
            //{
            //    RefireImmediately = true
            //};
            //throw executionException;
        }
    }

    public void Dispose() => GC.SuppressFinalize(this);
}