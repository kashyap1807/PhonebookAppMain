using Microsoft.EntityFrameworkCore;
using PhonebookAPI.Dtos;

namespace PhonebookAPI.Data
{
    public partial class AppDbContext
    {
        private IAppDbContextProcedures _procedures;

        public virtual IAppDbContextProcedures Procedures
        {
            get
            {
                if (_procedures == null) _procedures = new AppDbContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IAppDbContextProcedures GetProcedures()
        {
            return Procedures;
        }

        protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllContactSP>().HasNoKey().ToView(null);
            modelBuilder.Entity<GetAllContactByCountryDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<GetAllContactByGenderDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<GetAllContactByStateIdDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<GetAllContactByMonthDto>().HasNoKey().ToView(null);
        }
    }

    public partial class AppDbContextProcedures : IAppDbContextProcedures
    {
        private readonly IAppDbContext _context;

        public AppDbContextProcedures(IAppDbContext context)
        {
            _context = context;
        }

        public virtual List<AllContactSP> GetAllContact()
        {
            var report = _context.Set<AllContactSP>()
                .FromSqlRaw("EXEC GetAllContacts")
                .ToList();

            return report;
        }

        public virtual List<GetAllContactByCountryDto> GetAllContactByCountry()
        {
            var report = _context.Set<GetAllContactByCountryDto>()
                .FromSqlRaw("EXEC GetAllContactByCountry")
                .ToList();

            return report;
        }

        public virtual List<GetAllContactByGenderDto> GetAllContactByGender()
        {
            var report = _context.Set<GetAllContactByGenderDto>()
                .FromSqlRaw("EXEC GetAllContactByGender")
                .ToList();

            return report;
        }

        public virtual List<GetAllContactByStateIdDto> GetAllContactByStateId(int stateId)
        {
            var report = _context.Set<GetAllContactByStateIdDto>()
                .FromSqlRaw("EXEC GetContactByStateIdReport @StateId=" + stateId)
                .ToList();

            return report;
        }

        public virtual List<GetAllContactByMonthDto> GetAllContactByMonth(int month)
        {
            var report = _context.Set<GetAllContactByMonthDto>()
                .FromSqlRaw("EXEC GetContactsByMonth @Month=" + month)
                .ToList();

            return report;
        }
    }
}
