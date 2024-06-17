using Microsoft.Extensions.Options;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Infrastructure.Common.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace OfficeSync.Infrastructure.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly TwilioOptions _twilioOptions;
        public TwilioService(IOptions<TwilioOptions> twilioOptions)
        {
            _twilioOptions = twilioOptions.Value;
            TwilioClient.Init(_twilioOptions.AccountId, _twilioOptions.Token);
        }

        public async Task SendSMSAsync(string receiver, string content)
        {
            if (_twilioOptions.IsTestMode)
                receiver = _twilioOptions.ReceiverPhoneNumber;

            var messageResource = await MessageResource.CreateAsync(body: content,
                                                                    from: new Twilio.Types.PhoneNumber(_twilioOptions.PhoneNumber),
                                                                    to: new Twilio.Types.PhoneNumber(receiver));

            Console.WriteLine($"SID: {messageResource.Sid} \n STATUS: {messageResource.Status}");
        }
    }
}
