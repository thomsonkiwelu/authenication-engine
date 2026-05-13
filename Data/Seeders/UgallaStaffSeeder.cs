using authentication_engine.Config;
using authentication_engine.Features.Auth.Interfaces;
using authentication_engine.Features.Departments;
using authentication_engine.Features.Sections;
using authentication_engine.Features.Staffs;
using authentication_engine.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class UgallaStaffSeeder(IPasswordService passwordService) : IBaseSeeder
    {
        private readonly IPasswordService _passwordService = passwordService;

        private record UgallaStaffSeedItem(
            string FirstName,
            string LastName,
            string Email,
            string PhoneNumber,
            string RankCode
        );

        private static readonly UgallaStaffSeedItem[] UgallaStaffs =
        [
            new UgallaStaffSeedItem("Enoce", "Mayilla Majige", "enoce.majige@tanzaniaparks.go.tz", "753763339", "CR-II"),
            new UgallaStaffSeedItem("Deogratius", "Protas Sudi", "deogratius.sudi@tanzanianationalparks.go.tz", "657008772", "CR-III"),
            new UgallaStaffSeedItem("Hamisi", "Ismail Mahamudu", "hamisi.mahamudu@tanzanianationalparks.go.tz", "623653451", "CR-III"),
            new UgallaStaffSeedItem("Idrisa", "Mussa Hamisi", "idrisa.hamisi@tanzanianationalparks.go.tz", "655854550", "CR-III"),
            new UgallaStaffSeedItem("Jimmy", "James Maganga", "jimmy.maganga@tanzanianationalparks.go.tz", "742686933", "CR-III"),
            new UgallaStaffSeedItem("Joachim", "Joseph Christopher", "joachim.christopher@tanzanianationalparks.go.tz", "710492772", "CR-III"),
            new UgallaStaffSeedItem("Vidatto", "Bosco Goliama", "vidatto.goliama@tanzanianationalparks.go.tz", "716333653", "CR-III"),
            new UgallaStaffSeedItem("Felician", "Gabriel Malley", "felician.malley@tanzanianationalparks.go.tz", "678150962", "CR-III"),
            new UgallaStaffSeedItem("Daudi", "Grant Bugari", "daudi.bugali@tanzanianationalparks.go.tz", "674946554", "CR-III"),
            new UgallaStaffSeedItem("Baltazary", "Gitamwa Boa", "baltazary.boa@tanzaniaparks.go.tz", "653144779", "CR-I"),
            new UgallaStaffSeedItem("Boniphace", "William Mikwabe", "boniphace.mikwabe@tanzaniaparks.go.tz", "768530797", "CR-II"),
            new UgallaStaffSeedItem("Augustine", "Alex Mkaruka", "augustine.mkaruka@tanzanianationalparks.go.tz", "674879068", "CR-III"),
            new UgallaStaffSeedItem("Jofrey", "Stepano Mnkeni", "jofrey.mnkeni@tanzanianationalparks.go.tz", "657094655", "CR-III"),
            new UgallaStaffSeedItem("Mwita", "John Madodi", "mwaita.madodi@tanzanianationalparks.go.tz", "754465488", "CR-III"),
            new UgallaStaffSeedItem("Samwel", "Lesikary Meiseye", "samweli.meiseye@tanzanianationalparks.go.tz", "755959249", "CR-III"),
            new UgallaStaffSeedItem("Kisandu", "Mazoya Matwajo", "kisandu.mazoya@tanzanianationalparks.go.tz", "612725155", "CR-III"),
            new UgallaStaffSeedItem("Innocent", "Steven Chambalo", "innocent.chambalo@tanzanianationalparks.go.tz", "711536499", "CR-III"),
            new UgallaStaffSeedItem("Ndobo", "Maximillian Ndobo", "ndombo.ndobo@tanzanianationalparks.go.tz", "789466743", "CR-III"),
            new UgallaStaffSeedItem("Heaven", "Joshua Lesirwa", "heaven.lesirwa@tanzanianationalparks.go.tz", "778693253", "CR-III"),
            new UgallaStaffSeedItem("Lidya", "Lameck Thobias", "lidya.thobias@tanzanianationalparks.go.tz", "693040975", "CR-III"),
            new UgallaStaffSeedItem("Mugeta", "Laurence Mugeta", "mugeta.mugeta@tanzanianationalparks.go.tz", "778424913", "CR-III"),
            new UgallaStaffSeedItem("Joshua", "Ahimidiwe Palangyo", "joshua.palangyo@tanzanianationalparks.go.tz", "696980507", "CR-III"),
            new UgallaStaffSeedItem("Rigobert", "Cornel Joseph", "rigobert.joseph@tanzaniaprks.go.tz", "657632108", "CO-II"),
            new UgallaStaffSeedItem("Erick", "Theophil Ishengoma", "erick.ishemgoma@tanzaniaparks.go.tz", "624077064", "CO-II"),
            new UgallaStaffSeedItem("William", "Merata Mahembora", "wailliam.mahembora@tanzaniaparks.go.tz", "783469866", "PCR-I"),
            new UgallaStaffSeedItem("Ramadhan", "Mbiga Mwita", "ramadhani.mwaita@tanzaniaparks.go.tz", "716385264", "CR-II"),
            new UgallaStaffSeedItem("Jackson", "John Shirima", "jackson.shirima@tanzanianationalparks.go.tz", "680579477", "CR-II"),
            new UgallaStaffSeedItem("Suzana", "Gibasa Molla", "suzana.molla@taznzaniaparks.go.tz", "689786007", "CR-II"),
            new UgallaStaffSeedItem("Edward", "Johanes Okello", "edward.okello@tanzanianationalparks.go.tz", "679012145", "CR-III"),
            new UgallaStaffSeedItem("Dotto", "Magesa Daudi", "dotto.daudi@tanzanianationalparks.go.tz", "764530440", "CR-III"),
            new UgallaStaffSeedItem("Cosmas", "Juliua Majenga", "cosmas.majenga@tanzanianationalparks.go.tz", "692599574", "CR-III"),
            new UgallaStaffSeedItem("Sadick", "Salim Ramadhan", "sadick.ramadhani@tanzanianationalparks.go.tz", "769833761", "CR-III"),
            new UgallaStaffSeedItem("Said", "Issa Msabwa", "said.msabwa@tanzanianationalparks.go.tz", "712794413", "CR-III"),
            new UgallaStaffSeedItem("Loveness", "Exudus Swai", "loveness.swai@tanzanianationalparks.go.tz", "766766232", "CR-III"),
            new UgallaStaffSeedItem("Timotheo", "Musa Masunga", "timotheo.masunga@tanzanianationalparks.go.tz", "778281398", "CR-III"),
            new UgallaStaffSeedItem("Macdonald", "Faustine Lugaila", "mcdonald.lugaila@tanzanianationalparks.go.tz", "687611779", "CR-III"),
            new UgallaStaffSeedItem("Gabriel", "Chacha Chokera", "gabriel.chokera@tanzaniaparks.go.tz", "0786768807", "CR-I"),
            new UgallaStaffSeedItem("Iman", "Jotham Mwanganda", "iman.mwanganda@tanzaniaparks.com", "0687660567", "CR-I"),
            new UgallaStaffSeedItem("Kahema", "Kasbert Mdee", "kahema.mdee@tanzaniaparks.go.tz", "0783243710", "SCR"),
            new UgallaStaffSeedItem("Edmund", "Francis Morashani", "edmund.morashani@tanzaniaparks.go.tz", "0767186660", "PC"),
            new UgallaStaffSeedItem("Emmanuel", "William Zedekia", "emmanuel.zedekia@tanzaniaparks.go.tz", "0785963498", "CR-III"),
            new UgallaStaffSeedItem("Khery", "Masegese Kamulika", "kherimasegese@tanzaniaparks.go.tz", "772884570", "CR-III"),
            new UgallaStaffSeedItem("Clement", "Onesmo Temu", "clement.onesmo@tanzaniaparks.go.tz", "672388077", "CR-III"),
            new UgallaStaffSeedItem("Musa", "Shaban Shauri", "mussa.shauri@tanzaniaparks.go.tz", "712498655", "CR-III"),
            new UgallaStaffSeedItem("Anjera", "Binamungu Balengayabo", "anjera.balengayabo@tanzaniaparks.go.tz", "757960224", "CR-III")
        ];

        private static string NormalizeEmail(string value)
        {
            return (value ?? string.Empty).Trim().ToLowerInvariant();
        }

        private static string NormalizePhone(string value)
        {
            var digits = new string((value ?? string.Empty).Where(char.IsDigit).ToArray());

            if (digits.Length == 9)
                return "0" + digits;

            return digits;
        }

        private static string GenerateUsernameFromEmail(string email)
        {
            var atIndex = email.IndexOf('@');
            var username = atIndex > 0 ? email[..atIndex] : email;
            username = username.Trim().ToLowerInvariant();
            return username;
        }

        public async Task SeedAsync(AppDBContext context)
        {
            var seedUserId = (await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu")
                             ?? await context.Users.AsNoTracking().FirstOrDefaultAsync())
                ?.Id;

            var existingUsernames = new HashSet<string>(
                await context.Users.AsNoTracking().Select(u => u.Username).ToListAsync(),
                StringComparer.OrdinalIgnoreCase
            );

            var ugallaPark = await context.Parks.AsNoTracking().FirstOrDefaultAsync(p => p.Name == "Ugalla River National Park");
            if (ugallaPark is null)
                throw new KeyNotFoundException("Park not found: Ugalla River National Park");

            var ugallaOffice = await context.Offices.FirstOrDefaultAsync(o => o.ParkId == ugallaPark.Id);
            if (ugallaOffice is null)
                ugallaOffice = await context.Offices.FirstOrDefaultAsync(o => o.Name == ugallaPark.Name);

            if (ugallaOffice is null)
                throw new KeyNotFoundException("Office not found for Ugalla River National Park");

            var department = await context.Departments.FirstOrDefaultAsync(d =>
                d.OfficeId == ugallaOffice.Id && d.Name.ToLower() == "park commanding officer");
            if (department is null)
            {
                department = new Department
                {
                    Id = Guid.NewGuid(),
                    Name = "Park commanding officer",
                    Code = "PCO",
                    OfficeId = ugallaOffice.Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = seedUserId
                };

                await context.Departments.AddAsync(department);
                await context.SaveChangesAsync();
            }

            var section = await context.Sections.FirstOrDefaultAsync(s =>
                s.OfficeId == ugallaOffice.Id && s.DepartmentId == department.Id && s.Name.ToLower() == "less");
            if (section is null)
            {
                section = new Section
                {
                    Id = Guid.NewGuid(),
                    Name = "LESS",
                    Code = "LESS",
                    DepartmentId = department.Id,
                    OfficeId = ugallaOffice.Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = seedUserId
                };

                await context.Sections.AddAsync(section);
                await context.SaveChangesAsync();
            }

            foreach (var item in UgallaStaffs)
            {
                var email = NormalizeEmail(item.Email);
                var phone = NormalizePhone(item.PhoneNumber);

                var rank = await context.Ranks.AsNoTracking().FirstOrDefaultAsync(r => r.Code == item.RankCode);
                if (rank is null)
                    throw new KeyNotFoundException($"Rank not found for code '{item.RankCode}'.");

                var staff = await context.Staffs.FirstOrDefaultAsync(s => s.Email == email);
                if (staff is null)
                {
                    staff = new Staff
                    {
                        Id = Guid.NewGuid(),
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = email,
                        PhoneNumber = phone,
                        Status = "1",
                        RankId = rank.Id,
                        CreatedAt = DateTime.Now,
                        CreatedBy = seedUserId
                    };

                    await context.Staffs.AddAsync(staff);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var shouldUpdate = false;

                    if (!string.Equals(staff.FirstName, item.FirstName, StringComparison.Ordinal))
                    {
                        staff.FirstName = item.FirstName;
                        shouldUpdate = true;
                    }

                    if (!string.Equals(staff.LastName, item.LastName, StringComparison.Ordinal))
                    {
                        staff.LastName = item.LastName;
                        shouldUpdate = true;
                    }

                    if (!string.Equals(staff.PhoneNumber, phone, StringComparison.Ordinal))
                    {
                        staff.PhoneNumber = phone;
                        shouldUpdate = true;
                    }

                    if (staff.RankId != rank.Id)
                    {
                        staff.RankId = rank.Id;
                        shouldUpdate = true;
                    }

                    if (shouldUpdate)
                    {
                        staff.UpdatedAt = DateTime.Now;
                        staff.UpdatedBy = seedUserId;
                        await context.SaveChangesAsync();
                    }
                }

                var existingAssignment = await context.DepartmentStaffs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ds => ds.DepartmentId == section.Id && ds.StaffId == staff.Id);

                if (existingAssignment is null)
                {
                    await context.DepartmentStaffs.AddAsync(new DepartmentStaff
                    {
                        ModelType = "section",
                        DepartmentId = section.Id,
                        StaffId = staff.Id,
                        OfficeId = ugallaOffice.Id,
                        CreatedAt = DateTime.Now,
                        CreatedBy = seedUserId
                    });

                    await context.SaveChangesAsync();
                }

                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.StaffId == staff.Id || u.Email == email);
                User? userForParkAccess = existingUser;

                if (existingUser is null)
                {
                    var baseUsername = GenerateUsernameFromEmail(email);
                    var username = baseUsername;
                    var suffix = 1;

                    while (existingUsernames.Contains(username))
                    {
                        username = $"{baseUsername}.{suffix}";
                        suffix++;
                    }

                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = username,
                        Email = email,
                        Password = _passwordService.HashPassword("Tanapa@2026"),
                        StaffId = staff.Id,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = seedUserId
                    };

                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                    existingUsernames.Add(username);

                    userForParkAccess = user;
                }

                // Ensure Ugalla park access exists for the user (idempotent)
                if (userForParkAccess is not null)
                {
                    var existingUserPark = await context.UserParks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(up => up.UserId == userForParkAccess.Id && up.ParkId == ugallaPark.Id);

                    if (existingUserPark is null)
                    {
                        await context.UserParks.AddAsync(new UserPark
                        {
                            UserId = userForParkAccess.Id,
                            ParkId = ugallaPark.Id,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = seedUserId
                        });

                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
