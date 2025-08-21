using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class FinanceManagementApplicationContext : DbContext
{
    public FinanceManagementApplicationContext()
    {
    }

    public FinanceManagementApplicationContext(DbContextOptions<FinanceManagementApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BudgetItem> BudgetItems { get; set; }

    public virtual DbSet<ExpenseTransaction> ExpenseTransactions { get; set; }

    public virtual DbSet<FinanceRecord> FinanceRecords { get; set; }

    public virtual DbSet<IncomeSource> IncomeSources { get; set; }

    public virtual DbSet<IncomeTransaction> IncomeTransactions { get; set; }

    public virtual DbSet<SavingGoal> SavingGoals { get; set; }

    public virtual DbSet<SavingTransaction> SavingTransactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=MTHANG;Database=FinanceManagementApplication;uid=admin;pwd=160504;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BudgetItem>(entity =>
        {
            entity.ToTable("BudgetItem");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BudgetName)
                .HasMaxLength(50)
                .HasColumnName("budgetName");
            entity.Property(e => e.IsDelete)
                .HasDefaultValue(false)
                .HasColumnName("isDelete");
            entity.Property(e => e.IsOverBudget)
                .HasDefaultValue(false)
                .HasColumnName("isOverBudget");
            entity.Property(e => e.LimitAmount).HasColumnName("limitAmount");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.BudgetItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BudgetItem_User");
        });

        modelBuilder.Entity<ExpenseTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ExpenseTransaction", tb =>
                {
                    tb.HasTrigger("trg_UpdateBalanceAfterExpenseDelete");
                    tb.HasTrigger("trg_UpdateBalanceAfterExpenseInsert");
                });

            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.BudgetId).HasColumnName("budgetId");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("date");
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .HasColumnName("note");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Budget).WithMany()
                .HasForeignKey(d => d.BudgetId)
                .HasConstraintName("FK_ExpenseTransaction_BudgetItem");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ExpenseTransaction_User");
        });

        modelBuilder.Entity<FinanceRecord>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FinanceRecord");

            entity.Property(e => e.From).HasColumnName("from");
            entity.Property(e => e.To).HasColumnName("to");
            entity.Property(e => e.TotalExpense).HasColumnName("totalExpense");
            entity.Property(e => e.TotalIncome).HasColumnName("totalIncome");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_FinanceRecord");
        });

        modelBuilder.Entity<IncomeSource>(entity =>
        {
            entity.ToTable("IncomeSource");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDelete)
                .HasDefaultValue(false)
                .HasColumnName("isDelete");
            entity.Property(e => e.SourceName)
                .HasMaxLength(50)
                .HasColumnName("sourceName");
        });

        modelBuilder.Entity<IncomeTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IncomeTransaction", tb =>
                {
                    tb.HasTrigger("trg_UpdateBalanceAfterIncomeDelete");
                    tb.HasTrigger("trg_UpdateBalanceAfterIncomeInsert");
                });

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.SourceId).HasColumnName("sourceId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Source).WithMany()
                .HasForeignKey(d => d.SourceId)
                .HasConstraintName("FK_IncomeTransaction_IncomeSource");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IncomeTransaction_User");
        });

        modelBuilder.Entity<SavingGoal>(entity =>
        {
            entity.ToTable("SavingGoal");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CurrentAmount)
                .HasDefaultValue(0)
                .HasColumnName("currentAmount");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.GoalAmount).HasColumnName("goalAmount");
            entity.Property(e => e.GoalDate).HasColumnName("goalDate");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValue(false)
                .HasColumnName("isCompleted");
            entity.Property(e => e.IsDelete)
                .HasDefaultValue(false)
                .HasColumnName("isDelete");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.SavingGoals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SavingGoal_User");
        });

        modelBuilder.Entity<SavingTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SavingTransaction", tb =>
                {
                    tb.HasTrigger("trg_UpdateSavingGoalAfterDelete");
                    tb.HasTrigger("trg_UpdateSavingGoalAfterInsert");
                });

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("date");
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .HasColumnName("note");
            entity.Property(e => e.SavingGoalId).HasColumnName("savingGoalId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.SavingGoal).WithMany()
                .HasForeignKey(d => d.SavingGoalId)
                .HasConstraintName("FK_SavingTransaction_SavingGoal");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SavingTransaction_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", tb => tb.HasTrigger("trg_PreventUserDelete"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvatarPath).HasColumnName("avatarPath");
            entity.Property(e => e.Balance)
                .HasDefaultValue(0)
                .HasColumnName("balance");
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
