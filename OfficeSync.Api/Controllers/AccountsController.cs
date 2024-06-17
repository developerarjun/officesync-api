using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeSync.Application.Accounts.Commands;
using OfficeSync.Application.Accounts.Queries;
using OfficeSync.Application.Common.Exceptions;

namespace OfficeSync.Api.Controllers
{
    public class AccountsController : BaseController
    {
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginAccountResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAccountCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (AccountLockedException ex)
            {
                return StatusCode(423, ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("request-change-password")]
        public async Task<IActionResult> RequestPasswordChange([FromBody] RequestPasswordChangeCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("accept-invitation")]
        public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Mfa Endpoint")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(VerifyMfaTokenResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("verify-mfa-token")]
        public async Task<IActionResult> VerifyMfaToken([FromBody] VerifyMfaTokenCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.CurrentUserEmail = CurrentUserEmail;
                command.Provider = MfaProvider;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Mfa Endpoint")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResendMfaTokenResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("resend-mfa-token")]
        public async Task<IActionResult> ResendMfaToken([FromBody] ResendMfaTokenCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                command.CurrentUserEmail = CurrentUserEmail;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Mfa Endpoint")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResendMfaTokenResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpGet("mfa-providers")]
        public async Task<IActionResult> GetMfaProviders(CancellationToken cancellationToken)
        {
            try
            {
                var query = new ListAccountMfaProvidersQuery { CurrentUserEmail = CurrentUserEmail };
                var response = await Mediator.Send(query, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPost("request-phonenumber-verification-code")]
        public async Task<IActionResult> RequestVerificationCode([FromBody] RequestVerificationCodeCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                command.CurrentUserEmail = CurrentUserEmail;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPost("verify-phonenumber")]
        public async Task<IActionResult> VerifyPhoneNumber([FromBody] VerifyPhoneNumberCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                command.CurrentUserEmail = CurrentUserEmail;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpDelete("remove-phonenumber/{phoneNumber}")]
        public async Task<IActionResult> RemovePhoneNumber([FromRoute] string phoneNumber, CancellationToken cancellationToken)
        {
            try
            {
                var command = new RemovePhoneNumberCommand
                {
                    PhoneNumber = phoneNumber,
                    ClientUrl = ClientUrl,
                    CurrentUserEmail = CurrentUserEmail
                };
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPost("enable-mfa")]
        public async Task<IActionResult> EnableMfa([FromBody] EnableMfaCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                command.CurrentUserEmail = CurrentUserEmail;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPost("disable-mfa")]
        public async Task<IActionResult> DisableMfa([FromBody] DisableMfaCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientUrl = ClientUrl;
                command.CurrentUserEmail = CurrentUserEmail;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAccountSettingsResponse), 200)]
        [ProducesResponseType(404)]
        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings(CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetAccountSettingsQuery { CurrentUserEmail = CurrentUserEmail };
                var response = await Mediator.Send(query, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
