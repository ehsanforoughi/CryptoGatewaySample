using Bat.Core;
using Bat.Http;
using System.Text.Json;
using MailKit.Net.Smtp;
using CryptoGateway.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace CryptoGateway.Infrastructure.EmailGateway;

public class PakatEmailGatewayAdapter// : IEmailGatewayAdapter
{
    private static (bool, string) SendMailViaPakat(IConfiguration configuration, string receiver, string subject, string text, List<string> attachmentsLink = null)
    {
        try
        {
            #region Get Mail Template
            var template = @"<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table style='width:100%; direction:rtl;'>" +
                                        "<tr>" +
                                            "<p style='text-align:center; font-size:xx-large; height:100px; background-color:#bcd7e8;'>" +
                                                "DGBlocks.com" +
                                            "</p>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<p style='text-align:right; direction:rtl; font-size:large; height:200px;'>" +
                                                "@@@" +
                                            "</p>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<p style='text-align:center; height: 100px; padding-top:20px; background-color:#adaaaa;'>" +
                                                "<a href='https://dgblocks.com'>Copyright © DGBlocks.com 2024</a>" +
                                            "</p>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";
            #endregion

            FileLoger.Info($"SendMail From:{configuration["EmailSettings:Username"]}, To:{receiver}, Subject:{subject}");

            var url = GlobalVariables.EmailGatewaySettings.Pakat.Url;
            var header = new Dictionary<string, string>();
            header.Add("api-key", GlobalVariables.EmailGatewaySettings.Pakat.ApiKey);

            var body = new
            {
                sender = new { name = configuration["EmailSettings:SenderName"], email = configuration["EmailSettings:Sender"] },
                to = new[] { new { name = receiver, email = receiver } },
                subject = subject,
                htmlContent = template.Replace("@@@", text),
                //templateId = 10
            };

            var sendMailResult = HttpRequestTools.PostAsync(url, body, header).Result.response;
            //var resultMessage = sendMailResult.DeSerializeJson();
            var resultMessage = JsonDocument.Parse(sendMailResult).RootElement;
            if (sendMailResult.Contains("messageId"))
            {
                string messageId = resultMessage.GetProperty("messageId").Deserialize<string>();
                return (true, messageId?.Split("@")[0]?.Substring(1));
            }
            else
                return (false, resultMessage.GetRawText());
        }
        catch (SmtpCommandException ex)
        {
            FileLoger.Error(ex);
            return (false, "SmtpException");
        }
        catch (Exception e)
        {
            FileLoger.Error(e);
            return (false, "Exception");
        }
    }
    private static (bool, string) SendMailViaPakatWithTemplate(IConfiguration configuration, string actionType, string receiver, string subject, string text, List<string> attachmentsLink = null)
    {
        var url = GlobalVariables.EmailGatewaySettings.Pakat.Url;
        var header = new Dictionary<string, string>
        {
            { "api-key", GlobalVariables.EmailGatewaySettings.Pakat.ApiKey }
        };

        var templateId = 0;
        var parameters = new object();
        switch (actionType)
        {
            case "Login":
                {
                    templateId = 17;
                    parameters = new { VERIFICATION_CODE = text };
                    break;
                }
            case "VerifyEmail":
                {
                    templateId = 16;
                    parameters = new { VERIFICATION_CODE = text };
                    break;
                }
            case "Withdraw":
                {
                    templateId = 15;
                    parameters = new { VERIFICATION_CODE = text };
                    break;
                }
            case "RecoverPassword":
                {
                    templateId = 14;
                    parameters = new { PASSWORD_RESET_TEXT = text.Split("|")[0], PASSWORD_RESET_LINK = text.Split("|")[1] };
                    break;
                }
            case "Referral":
                {
                    templateId = 18;
                    parameters = new { REFFERAL_TEXT = text.Split("|")[0], REFFERAL_LINK = text.Split("|")[1] };
                    break;
                }
            case "UpdatePassword":
                {
                    templateId = 19;
                    parameters = new { UPDATE_PASSWORD_TEXT = text.Split("|")[0], NEW_PASSWORD = text.Split("|")[1] };
                    break;
                }
            case "SystemError":
            case "CreateSettlementFile":
            case "AddTransactionAfterSettlement":
            case "ProcessingSettlementFileResponse":
                {
                    templateId = 51;
                    parameters = new { TITLE = string.Join(" ", actionType.SplitCamelCase()), MESSAGE = text };
                    break;
                }
        }

        var body = "{";
        body += "\"sender\":{" + $"\"name\":\"{configuration["EmailSettings:SenderName"]}\",\"email\":\"{configuration["EmailSettings:Sender"]}\"" + "},";
        body += "\"to\":[{" + $"\"name\":\"{receiver}\",\"email\":\"{receiver}\"" + "}],";
        body += $"\"subject\":\"{receiver}\",";
        body += $"\"htmlContent\":\"10\",";
        body += $"\"templateId\":{templateId},";
        body += $"\"params\":{parameters.SerializeToJson()}";
        body += "}";

        var sendMailResult = HttpRequestTools.PostAsync(url, body, header).Result.response;
        var resultMessage = JsonDocument.Parse(sendMailResult).RootElement;
        if (sendMailResult.Contains("messageId"))
        {
            string messageId = resultMessage.GetProperty("messageId").Deserialize<string>();
            return (true, messageId?.Split("@")[0]?.Substring(1));
        }

        return (false, resultMessage.GetRawText());
    }

    public static IResponse<bool> Send(IConfiguration configuration, string actionType, string receiver, string subject, string text)
    {
        var response = new Response<bool>();
        (bool, string) sendResult;
        if (actionType == "FreeTemplate")
            sendResult = SendMailViaPakat(configuration, receiver, subject, text);
        else
             sendResult = SendMailViaPakatWithTemplate(configuration, actionType, receiver, subject, text);
        if (sendResult.Item1)
        {
            response.Result = true;
            response.IsSuccess = true;
            response.Message = $"Status: Success | ID: {sendResult.Item2}";
        }
        else
        {
            response.Result = false;
            response.Message = $"Status: Fail | ID: {sendResult.Item2}";
        }

        return response;
    }
}