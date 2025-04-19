using OPS.Domain.Entities.User;

namespace OPS.Persistence.Seeding;

public static class UserData
{
    public static void SeedUserData(AppDbContext context)
    {
        // password: P@ss9999
        const string hash = "$2a$11$BbRseq/imsqMpBMycI1w7ejk2s2iozFl7S2xEgCjC2roTwp1wmO2y";
        const string salt = "xKoyKYJ+AwisxsMjM1v8aZicH5/r7cee/YwoNBpvxb8=";

        var accounts = new List<Account>
        {
            new()
            {
                Username = "rawfin",
                Email = "hello@rawfin.net",
                PasswordHash = hash,
                Salt = salt,
                AccountRoles = new List<AccountRole> { new() { RoleId = 1 }, new() { RoleId = 3 } },
                Profile = new Profile
                {
                    FirstName = "Zaid Amin",
                    LastName = "Rawfin",
                    Bio = "Visit rawfin.net :)",
                    InstituteName = "AIUB",
                    PhoneNumber = "+01234567890",
                    ProfileLinks = new List<ProfileLinks>
                    {
                        new()
                        {
                            Name = "GitHub",
                            Link = "https://github.com/Raofin",
                        }
                    }
                }
            },
            new()
            {
                Username = "akhi",
                Email = "akhi@truetest.tech",
                PasswordHash = hash,
                Salt = salt,
                AccountRoles = new List<AccountRole> { new() { RoleId = 1 }, new() { RoleId = 3 } },
            },
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Username = "admin",
                Email = "admin@truetest.tech",
                PasswordHash = hash,
                Salt = salt,
                AccountRoles = new List<AccountRole> { new() { RoleId = 1 }, new() { RoleId = 3 } },
            }
        };

        context.Accounts.AddRange(accounts);

        context.SaveChanges();
    }
}