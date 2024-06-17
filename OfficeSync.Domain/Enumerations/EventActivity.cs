namespace OfficeSync.Domain.Enumerations
{
    public enum UserEventActivity
    {
        RequestChangePassword = 1,
        ChangePassword = 2,
        Invite = 3,
        AcceptInvitation = 4,
        MfaTokenToPhone = 5,
        MfaTokenToEmail = 6,
        ChangePhoneNumber = 7,
        VerifyPhoneNumber = 8,
        Enable2FA = 9,
        Disable2FA = 10,
        RemovePhoneNumber = 11
    }
}
