namespace OPS.Application.Constants;

public static class ValidationConstants
{
    public const string EmailRegex = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$";

    // Minimum min 8 chars and at least 1x (lowercase, uppercase, digit, special char)
    public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$";
}