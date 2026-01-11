using EkotaNibash.Domain;
using Microsoft.EntityFrameworkCore;

namespace EkotaNibash.Infrastructure
{
    public class BoniyadiContext : DbContext
    {
        public BoniyadiContext(DbContextOptions<BoniyadiContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }

        public virtual DbSet<EkotaMember> EkotaMembers { get; set; }
        public virtual DbSet<MembershipPayment> MembershipPayments { get; set; }
        public virtual DbSet<Nominee> Nominees { get; set; }
        public virtual DbSet<MemberDocument> MemberDocuments { get; set; }
        public virtual DbSet<MemberDocumentSlices> MemberDocumentSlices { get; set; }

        #region Lookup Tables        
        public virtual DbSet<ExpenseType> ExpenseType { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
