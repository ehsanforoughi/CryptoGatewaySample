using Bat.Core;
using Kavenegar;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;
using CryptoGateway.Infrastructure;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Strategies.Notification.SmsGateway;

public class KaveNegarAdapter// : ISmsGatewayAdapter
{
    private readonly KavenegarApi _smsGateWay;
    private static readonly string ApiKey = GlobalVariables.SmsGatewaySettings.KaveNegar.ApiKey;
    private static readonly string SenderId = GlobalVariables.SmsGatewaySettings.KaveNegar.Sender;
    public KaveNegarAdapter()
    {
        _smsGateWay = new KavenegarApi(ApiKey);
    }

    private async Task<SendResult> SendByTemplate(Domain.Entities.Notification.Notification notification)
    {
        // Kavenegar does not allow empty space in a parameter.
        var tokens = notification.Text.Value.Replace(" ", ".").Split("|");
        switch (notification.ActionType)
        {
            case NotificationActionType.Register:
            case NotificationActionType.Login:
            case NotificationActionType.ChangeBalance:
            case NotificationActionType.VerifyEmail:
            case NotificationActionType.Withdraw:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], KavenegarTemplate.Verify_Ramzplus.GetDescription());
            case NotificationActionType.VerifiedAuthLevelOne:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], KavenegarTemplate.KYC_One_Done.GetDescription());
            case NotificationActionType.VerifiedAuthLevelTwo:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], KavenegarTemplate.KYC_Two_Done.GetDescription());
            case NotificationActionType.UnVerifiedAuthentication:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], KavenegarTemplate.KYC_Issue.GetDescription());
            case NotificationActionType.Referral:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], tokens[1], tokens[2], KavenegarTemplate.Referral.GetDescription());
            case NotificationActionType.NobitexTokenExpire:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], KavenegarTemplate.Nobitex_Token_Expire.GetDescription());
            case NotificationActionType.NobitexBuyError:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], tokens[1], tokens[2], KavenegarTemplate.Nobitex_Buy_Error.GetDescription());
            case NotificationActionType.Deposit:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], tokens[1], tokens[2], KavenegarTemplate.Crypto_Deposit.GetDescription());
            case NotificationActionType.UpdateUserLevel:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], KavenegarTemplate.User_Level.GetDescription());
            case NotificationActionType.BackOfficeEvents:
                return await _smsGateWay.VerifyLookup(notification.Receiver, tokens[0], KavenegarTemplate.Withdrawal_Fee_Exceeded.GetDescription());
            case NotificationActionType.LogOut:
            case NotificationActionType.VerifiedAuthentication:
            case NotificationActionType.RecoverPassword:
            case NotificationActionType.UpdatePassword:
            case NotificationActionType.Buy:
            case NotificationActionType.Sell:
            default:
                throw new ArgumentException();
        }
    }
    public async Task<IResponse<bool>> SendAsync(string receiver, string text, bool isFlash = false)
    {
        var response = new Response<bool>();
        try
        {
            var sendResult = await _smsGateWay.Send(
                GlobalVariables.SmsGatewaySettings.KaveNegar.Sender,
                receiver,
                text, MessageType.Flash, DateTime.Now);
            if (sendResult.Status is 4 or 5)
            {
                response.Result = true;
                response.IsSuccess = true;
                response.Message = $"Status: Success | ID: {sendResult.Messageid}";
            }
            else
            {
                response.Result = false;
                response.Message = $"Status: {sendResult.StatusText} | ID: {0}";
            }
        }
        catch (Exception e)
        {
            FileLoger.Error(e);
            response.Message = e.Message;
            return response;
        }

        return response;
    }

    public async Task<IResponse<bool>> SendByTemplateAsync(Domain.Entities.Notification.Notification notification, bool isFlash = false)
    {
        var response = new Response<bool>();
        try
        {
            var sendResult = await SendByTemplate(notification);

            if (sendResult.Status is 4 or 5)
            {
                response.Result = true;
                response.IsSuccess = true;
                response.Message = $"Status: Success | ID: {sendResult.Messageid}";
            }
            else
            {
                response.Result = false;
                response.Message = $"Status: {sendResult.StatusText} | ID: {0}";
            }
        }
        catch (Exception e)
        {
            FileLoger.Error(e);
            response.Message = e.Message;
            return response;
        }

        return response;
    }
}