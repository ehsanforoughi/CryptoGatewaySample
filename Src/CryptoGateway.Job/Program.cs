using Quartz;
using Quartz.AspNetCore;
using System.Data.Common;
using CryptoGateway.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CryptoGateway.Job.Infrastructure;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.PayIn;
using CryptoGateway.DomainService.Payment;
using CryptoGateway.Domain.Entities.Payout;
using CryptoGateway.DataAccess.UnitOfWorks;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.DataAccess.Repositories;
using CryptoGateway.Service.Quartz.Jobs.PayIn;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Service.Quartz.Jobs.Payment;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Service.Quartz.Jobs.Contract;
using CryptoGateway.Service.Quartz.Jobs.Messaging;
using CryptoGateway.Domain.Entities.CustodyAccount;
using CryptoGateway.Domain.Entities.ContractAccount;
using CryptoGateway.Service.ExternalWebServices.RamzPlus;
using CryptoGateway.Service.ExternalWebServices.NodeJsApi;
using CryptoGateway.Service.ExternalWebServices.SunCentre;
using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi;
using CryptoGateway.DomainService.ExternalWebServices.RamzPlus;
using CryptoGateway.DomainService.ExternalWebServices.SunCentre;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(configuration.GetConnectionString("AppDbContext")));

builder.Services.AddScoped<DbConnection>(c => 
    new SqlConnection(configuration.GetConnectionString("AppDbContext")));
builder.Services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPayInRepository, PayInRepository>();
builder.Services.AddScoped<IPayoutRepository, PayoutRepository>();
builder.Services.AddScoped<IContractAccountRepository, ContractAccountRepository>();
builder.Services.AddScoped<ICustodyAccRepository, CustodyAccRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPayInDomainService, PayInDomainService>();
builder.Services.AddScoped<IPaymentDomainService, PaymentDomainService>();
builder.Services.AddScoped<IBlockedCredit, FixedBlockedCredit>();
builder.Services.AddScoped<ITronWeb, TronWeb>();
builder.Services.AddScoped<IRamzPlusPublicApi, RamzPlusPublicApi>();
builder.Services.AddScoped<ISunCentreECommerceApi, SunCentreECommerceApi>();
builder.Services.AddSingleton<InquiryPayInStatusJob>();
//builder.Services.AddSingleton<InquiryPaymentStatusJob>();
builder.Services.AddSingleton<SendingNotificationJob>();
builder.Services.AddSingleton<KeepServerAliveJob>();

builder.Services.AddQuartz(q =>
{
    if (bool.Parse(configuration["QuartzSettings:IsActiveInquiryPayInStatusJob"]!))
    {
        var payInJobKey = new JobKey("InquiryPayInStatusJob");
        q.AddJob<InquiryPayInStatusJob>(opts => opts.WithIdentity(payInJobKey));
        q.AddTrigger(opts => opts
            .ForJob(payInJobKey)
            .WithIdentity("InquiryPayInStatusJob-trigger")
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(1)
                .RepeatForever()
                .WithMisfireHandlingInstructionNextWithExistingCount())
            //.WithCronSchedule(configuration["QuartzSettings:InquiryPayInStatusJobCron"]!)
            .StartNow());
    }

    //if (bool.Parse(configuration["QuartzSettings:IsActiveInquiryPaymentStatusJob"]!))
    //{
    //    var paymentJobKey = new JobKey("InquiryPaymentStatusJob");
    //    q.AddJob<InquiryPaymentStatusJob>(opts => opts.WithIdentity(paymentJobKey));
    //    q.AddTrigger(opts => opts
    //        .ForJob(paymentJobKey)
    //        .WithIdentity("InquiryPaymentStatusJob-trigger")
    //        .WithSimpleSchedule(x => x
    //            .WithIntervalInMinutes(1)
    //            .RepeatForever()
    //            .WithMisfireHandlingInstructionNextWithExistingCount())
    //        //.WithCronSchedule(configuration["QuartzSettings:InquiryPaymentStatusJobCron"]!)
    //        .StartNow());
    //}

    if (bool.Parse(configuration["QuartzSettings:IsActiveSendNotificationJob"]!))
    {
        var notifJobKey = new JobKey("SendingNotificationJob");
        q.AddJob<SendingNotificationJob>(opts => opts.WithIdentity(notifJobKey));
        q.AddTrigger(opts => opts
            .ForJob(notifJobKey)
            .WithIdentity("SendingNotificationJob-trigger")
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(1)
                .RepeatForever()
                .WithMisfireHandlingInstructionNextWithExistingCount())
            //.WithCronSchedule(configuration["QuartzSettings:SendNotificationJobCron"]!)
            .StartNow());
    }

    //if (bool.Parse(configuration["QuartzSettings:IsActiveKeepServerAliveJob"]!))
    //{
    //    var keepAliveJobKey = new JobKey("KeepServerAliveJob");
    //    q.AddJob<KeepServerAliveJob>(opts => opts.WithIdentity(keepAliveJobKey));
    //    q.AddTrigger(opts => opts
    //        .ForJob(keepAliveJobKey)
    //        .WithIdentity("KeepServerAliveJob-trigger")
    //        .WithSimpleSchedule(x => x
    //            .WithIntervalInMinutes(1)
    //            .RepeatForever()
    //            .WithMisfireHandlingInstructionNextWithExistingCount())
    //        //.WithCronSchedule(configuration["QuartzSettings:KeepServerAliveJobCron"]!)
    //        .StartNow());
    //}

    if (bool.Parse(configuration["QuartzSettings:IsActiveUpdateContractTransactionsJob"]!))
    {
        var UpdateContractTransactionsJobKey = new JobKey("UpdateContractTransactionsJob");
        q.AddJob<UpdateContractTransactionsJob>(opts => opts.WithIdentity(UpdateContractTransactionsJobKey));
        q.AddTrigger(opts => opts
            .ForJob(UpdateContractTransactionsJobKey)
            .WithIdentity("UpdateContractTransactionsJob-trigger")
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(2)
                .RepeatForever()
                .WithMisfireHandlingInstructionNextWithExistingCount())
            //.WithCronSchedule(configuration["QuartzSettings:KeepServerAliveJobCron"]!)
            .StartNow());
    }
});

// ASP.NET Core hosting
builder.Services.AddQuartzServer(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
