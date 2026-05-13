using authentication_engine.Config;
using authentication_engine.Features.Departments;
using authentication_engine.Features.Sections;
using authentication_engine.Features.Staffs;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders;

public class TanapaHqLessStaffSeeder : IBaseSeeder
{
    private record TanapaHqStaffSeedItem(
        string FirstName,
        string SecondName,
        string LastName,
        string Email,
        string PhoneNumber,
        string RankCode
    );

    private static readonly TanapaHqStaffSeedItem[] TanapaHqStaffs =
    [
        new("Lydia", "Ezekiel", "Chenge", "lydia.chenge@tanzaniaparks.go.tz", "0766514842", "CR-III"),
        new("Ponsian", "Cyprian", "Magoda", "ponsian.magoda@tanzaniaparks.go.tz", "0786520832", "SCR"),
        new("Happiness", "Elimringi", "Maro", "happiness.maro@tanzaniaparks.go.tz", "0766996312", "CR-II"),
        new("Bruno", "Bonventura", "Mbunda", "bruno.mbunda@tanzaniaparks.go.tz", "0763857748", "PCR-I"),
        new("Musa", "Dawido", "Bayo", "musa.bayo@tanzaniaparks.go.tz", "0688384462", "PCR-II"),
        new("Balina", "Mboyi", "Mzumbi", "balina.mboyi@tanzaniaparks.go.tz", "0784518311", "PCR-II"),
        new("Josseph", "Jimmy", "Mhole", "josseph.jimy@taznzaniaparks.go.tz", "0754207047", "PCR-II"),
        new("Oscar", "Joseph", "Katuli", "oscar.katuli@tanzaniaparks.go.tz", "0754448975", "PCR-II"),
        new("Paulo", "Mgoroba", "Lyaro", "paul.mgorba@tanzaniaparks.go.tz", "0784318678", "PCR-II"),
        new("Kalid", "Mohamed", "Mkuya", "khalid.mkuya@tanzaniaparks.go.tz", "0768679597", "SCR"),
        new("Ephrem", "Tiberius", "Lusuva", "ephrem.lusuva@tanzaniapaks.go.tz", "0768599394", "SCR"),
        new("Abdillah", "Ally", "Makinga", "abdillah.makinga@tanzaniaparks.go.tz", "0768614377", "SCR"),
        new("Ally", "Abdallah", "Luchega", "ally.luchega@tanzaniaparks.go.tz", "0622996464", "SCR"),
        new("Zarau", "Hassan", "Kipangule", "zarau.kipangule@tanzaniaparks.go.tz", "0763624144", "SCR"),
        new("Gregory", "Josseph", "Mshana", "gregory.mshana@tanzaniaparks.go.tz", "0767296661", "SCR"),
        new("Francis", "Samwel", "Kamote", "francis.kamote@tanzaniaparks.go.tz", "0754406373", "SCR"),
        new("Ponsiano", "Cyprian", "Magoda", "ponsiano.magoda@tanzaniaparks.go.tz", "0757384950", "SCR"),
        new("Daniel", "Lewis", "Kuboja", "daniel.kuboja@tanzaniaparks.go.tz", "0757422137", "CR-I"),
        new("James", "Mutyama", "Masatu", "james.masatu@tanzaniaparks.go.tz", "069300443", "CR-I"),
        new("Amos", "Musa", "Masubugu", "amos.masubugu@tanzaniaparks.go.tz", "0784829683", "CR-I"),
        new("Deusdedith", "William", "Chacha", "deusdedith.chacha@tanzaniaparks.go.tz", "0755459152", "CR-I"),
        new("Dickson", "George", "Muro", "dickson.muro@tanzaniaparks.go.tz", "0755287528", "CR-I"),
        new("Hawa", "Abas", "Kajwangya", "hawa.kajwangwa@tanzaniaparks.go.tz", "0753636758", "CR-I"),
        new("Hassan", "Karim", "Mdungu", "hassan.mdungu@tanzaniaparks.go.tz", "0676594265", "CR-I"),
        new("Wanduta", "Kadele", "Ackim", "wanduta.kadele@tanzaniaparks.go.tz", "0784910762", "CR-I"),
        new("Yohana", "Mpanda", "Nange", "yohana.nange@tanzaniaparks.go.tz", "0789555504", "CR-I"),
        new("Amen", "Losinyari", "Metili", "ameni.metili@tanzaniaparks.go.tz", "0765316527", "CR-I"),
        new("Fanuel", "Joshua", "Mmbuji", "fanuel.mmbuji@tanzaniaparks.go.tz", "0687684355", "CR-I"),
        new("Chacha", "Thomas", "Collins", "chacha.erans@tanzaniaparks.go.tz", "0693016220", "CR-I"),
        new("Rajab", "Jafari", "Nzowa", "rajab.nzowa@tanzaniaparks.go.tz", "0764603691", "CR-II"),
        new("Stella", "Kasimir", "Mandawa", "stella.mandawa@tanzaniaparks.go.tz", "0754450099", "CR-II"),
        new("Rashid", "Mussa", "Mungah", "rashid.munga@tanzaniaparks.go.tz", "0757637010", "CR-II"),
        new("Mwanamkuu", "Izadin", "Msofe", "mwanamkuu.msofe@tanzaniaparks.go.tz", "0768124645", "CR-II"),
        new("Christian", "Andreas", "Charaji", "christian.charaji@tanzaniaparks.go.tz", "0798674496", "CR-II"),
        new("Mkandawile", "Salum", "Ramadhani", "ramadhan.mkandawile@tanzaniaparks.go.tz", "0753702374", "CR-II"),
        new("Said", "Athuman", "Hassan", "said.athuman@tanzaniaparks.go.tz", "0785551510", "CR-II"),
        new("Salma", "Yusuf", "Aengo", "salma.aengo@tanzaniaparks.go.tz", "0758658408", "CR-III"),
        new("Alfred", "Nhungulu", "Lazaro", "alfre.lazaro@tanzaniaparks.go.tz", "0743310927", "CR-III"),
        new("Simiyu", "Msingi", "Simiyu", "simiyu.msingi@tanzaniaparks.go.tz", "0784925253", "CR-III"),
        new("Ibrahim", "Ziddiheri", "Msangi", "ibrahim.msangi@tanzaniaparks.go.tz", "0685366524", "CR-III"),
        new("Gido", "Gerald", "Mushi", "gido.mushi@tanzaniaparks.go.tz", "0675051959", "CR-III"),
        new("Emmanuel", "Daudi", "Masalu", "emmanuel.msalu@tanzaniaparks.go.tz", "0747484310", "CR-III"),
        new("John", "Joseph", "Matondo", "john.matondo@tanzaniaparks.go.tz", "0693939504", "CR-III"),
        new("Mohamed", "Maulid", "Buweta", "mohamed.buweta@tanzaniaparks.go.tz", "0615971329", "CR-III"),
        new("Martha", "Francis", "Lucas", "martha.francis@tanzaniaparks.go.tz", "06785837171", "CR-III"),
        new("Paulo", "George", "Gabriel", "paul.gabriel@tanzaniaparks.go.tz", "0625908006", "CR-III"),
        new("Simon", "Abdul", "Mfaume", "simon.mfaume@tanzaniaparks.go.tz", "0713329416", "CR-III"),
        new("Idrisa", "Athuman", "Jumanne", "idrisa.athuman@tanzaniaparks.go.tz", "0783853660", "CR-III"),
        new("Hassan", "Mbwana", "Kanyemka", "hassan.kanyemka@tanzaniaparks.go.tz", "0780445543", "CR-III"),
        new("Jasiri", "Oscar", "Henerico", "jasiri.henerico@tanzaniaparks.go.tz", "0623154020", "CR-III"),
        new("Collins", "Venance", "Mselle", "collins.mselle@tanzaniaparks.go.tz", "0612235764", "CR-III"),
        new("Regius", "", "Komba", "regius.komba@tanzaniaparks.go.tz", "0768426864", "PC")
    ];

    private static string NormalizeEmail(string value)
    {
        return (value ?? string.Empty).Trim().ToLowerInvariant();
    }

    private static string NormalizePhone(string value)
    {
        var digits = new string((value ?? string.Empty).Where(char.IsDigit).ToArray());

        if (digits.Length == 9 && !digits.StartsWith('0'))
            return "0" + digits;

        return digits;
    }

    public async Task SeedAsync(AppDBContext context)
    {
        var seedUserId = (await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu")
                         ?? await context.Users.AsNoTracking().FirstOrDefaultAsync())
            ?.Id;

        var hqOffice = await context.Offices.FirstOrDefaultAsync(o => o.Code == "HQ")
                      ?? await context.Offices.FirstOrDefaultAsync(o => o.Name.ToLower() == "tanapa-hq arusha");

        if (hqOffice is null)
            throw new KeyNotFoundException("Office not found: TANAPA-HQ Arusha");

        var department = await context.Departments.FirstOrDefaultAsync(d =>
            d.OfficeId == hqOffice.Id &&
            (d.Code.ToLower() == "dccb" || d.Name.ToLower() == "dccb"));
        if (department is null)
        {
            department = new Department
            {
                Id = Guid.NewGuid(),
                Name = "DCCB",
                Code = "DCCB",
                OfficeId = hqOffice.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = seedUserId
            };

            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();
        }

        var section = await context.Sections.FirstOrDefaultAsync(s =>
            s.OfficeId == hqOffice.Id &&
            s.DepartmentId == department.Id &&
            (s.Code.ToLower() == "less" || s.Name.ToLower() == "less"));

        if (section is null)
        {
            section = new Section
            {
                Id = Guid.NewGuid(),
                Name = "LESS",
                Code = "LESS",
                DepartmentId = department.Id,
                OfficeId = hqOffice.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = seedUserId
            };

            await context.Sections.AddAsync(section);
            await context.SaveChangesAsync();
        }

        var ranks = await context.Ranks.AsNoTracking().ToListAsync();
        var rankByCode = ranks
            .GroupBy(r => r.Code)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        foreach (var item in TanapaHqStaffs)
        {
            var email = NormalizeEmail(item.Email);
            var phone = NormalizePhone(item.PhoneNumber);

            if (!rankByCode.TryGetValue(item.RankCode, out var rank))
                throw new KeyNotFoundException($"Rank not found for code '{item.RankCode}'.");

            var staff = await context.Staffs.FirstOrDefaultAsync(s => s.Email == email);
            var lastName = string.Join(' ', new[] { item.SecondName, item.LastName }.Where(v => !string.IsNullOrWhiteSpace(v))).Trim();

            if (staff is null)
            {
                staff = new Staff
                {
                    Id = Guid.NewGuid(),
                    FirstName = item.FirstName,
                    LastName = lastName,
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

                if (!string.Equals(staff.LastName, lastName, StringComparison.Ordinal))
                {
                    staff.LastName = lastName;
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
                    OfficeId = hqOffice.Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = seedUserId
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
