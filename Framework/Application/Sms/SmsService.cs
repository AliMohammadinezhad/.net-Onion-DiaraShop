using IPE.SmsIrClient;
using IPE.SmsIrClient.Models.Results;
using Microsoft.Extensions.Configuration;

namespace Framework.Application.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public void Send(string number, string message)
        //{
        //    var token = GetToken();
        //    var lines = new SmsLine().GetSmsLines(token);
        //    if (lines == null) return;

        //    var line = lines.SMSLines.Last().LineNumber.ToString();
        //    var data = new MessageSendObject
        //    {
        //        Messages = new List<string>
        //            {message}.ToArray(),
        //        MobileNumbers = new List<string> {number}.ToArray(),
        //        LineNumber = line,
        //        SendDateTime = DateTime.Now,
        //        CanContinueInCaseOfError = true
        //    };
        //    var messageSendResponseObject = 
        //        new MessageSend().Send(token, data);

        //    if (messageSendResponseObject.IsSuccessful) return;

        //    line = lines.SMSLines.First().LineNumber.ToString();
        //    data.LineNumber = line;
        //    new MessageSend().Send(token, data);
        //}

        //private string GetToken()
        //{
        //    var smsSecrets = _configuration.GetSection("SmsSecrets");
        //    var tokenService = new Token();
        //    return tokenService.GetToken(smsSecrets["ApiKey"], smsSecrets["SecretKey"]);
        //}
        public void Send(string number, string message)
        {
            var smsSecrets = _configuration.GetSection("SmsSecrets");
            var apiKey = smsSecrets["ApiKey"];
            var sms = new SmsIr(apiKey);
            
            var lineNumbers = smsSecrets.GetSection("LineNumbers")
                .GetChildren()
                .Select(x => x.Value)
                .ToArray();
            var lineNumber = lineNumbers.Last();

            if (string.IsNullOrWhiteSpace(lineNumber))
                lineNumber = lineNumbers.First();

            DateTime currentTime = DateTime.UtcNow;
            int sendDateTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            var response = sms.BulkSend(long.Parse(lineNumber) , message, [number], sendDateTime);

            if (response.Status == 200) return;

            SendResult sendResult = response.Data;
            Guid packId = sendResult.PackId;
            int?[] messageIds = sendResult.MessageIds;
            decimal cost = sendResult.Cost;
        }
    }
}