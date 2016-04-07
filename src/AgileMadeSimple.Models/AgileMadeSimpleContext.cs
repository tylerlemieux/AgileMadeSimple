using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace AgileMadeSimple.Models
{
    public partial class AgileMadeSimpleContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=Tyler-PC;Initial Catalog=AgileMadeSimple;integrated security=true;multipleactiveresultsets=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Epic>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");

                entity.HasOne(d => d.State).WithMany(p => p.Epic).HasForeignKey(d => d.StateID).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Team).WithMany(p => p.Epic).HasForeignKey(d => d.TeamID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar");

                entity.HasOne(d => d.Epic).WithMany(p => p.Feature).HasForeignKey(d => d.EpicID).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.State).WithMany(p => p.Feature).HasForeignKey(d => d.StateID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FeatureTag>(entity =>
            {
                entity.HasOne(d => d.Feature).WithMany(p => p.FeatureTag).HasForeignKey(d => d.FeatureID).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Tag).WithMany(p => p.FeatureTag).HasForeignKey(d => d.TagID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.Property(e => e.DefinitionOfDone)
                    .HasMaxLength(1000)
                    .HasColumnType("varchar");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.SprintGoals)
                    .HasMaxLength(2000)
                    .HasColumnType("varchar");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<States>(entity =>
            {
                entity.HasKey(e => e.StateID);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .HasColumnType("varchar");

                entity.HasOne(d => d.Team).WithMany(p => p.States).HasForeignKey(d => d.TeamID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Story>(entity =>
            {
                entity.Property(e => e.AcceptanceCriteria).HasColumnType("text");

                entity.Property(e => e.Blocked)
                    .HasMaxLength(1)
                    .HasColumnType("varchar");

                entity.Property(e => e.BlockedText).HasColumnType("text");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnType("varchar");

                entity.HasOne(d => d.Epic).WithMany(p => p.Story).HasForeignKey(d => d.EpicID).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Owner).WithMany(p => p.Story).HasForeignKey(d => d.OwnerID);

                entity.HasOne(d => d.Sprint).WithMany(p => p.Story).HasForeignKey(d => d.SprintID);

                entity.HasOne(d => d.State).WithMany(p => p.Story).HasForeignKey(d => d.StateID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StoryTag>(entity =>
            {
                entity.HasOne(d => d.Tag).WithMany(p => p.StoryTag).HasForeignKey(d => d.TagID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.Blocked)
                    .HasMaxLength(1)
                    .HasColumnType("varchar");

                entity.Property(e => e.BlockedMessage).HasColumnType("text");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar");

                entity.Property(e => e.ToDoHours).HasColumnType("decimal");

                entity.Property(e => e.TotalHours).HasColumnType("decimal");

                entity.HasOne(d => d.Story).WithMany(p => p.Task).HasForeignKey(d => d.StoryID);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            });

            modelBuilder.Entity<TeamUser>(entity =>
            {
                entity.HasOne(d => d.Team).WithMany(p => p.TeamUser).HasForeignKey(d => d.TeamID).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.User).WithMany(p => p.TeamUser).HasForeignKey(d => d.UserID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnType("varchar");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnType("varchar");

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .HasColumnType("varchar");

                entity.Property(e => e.Salt)
                    .HasMaxLength(200)
                    .HasColumnType("varchar");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            });
        }

        public virtual DbSet<Epic> Epic { get; set; }
        public virtual DbSet<Feature> Feature { get; set; }
        public virtual DbSet<FeatureTag> FeatureTag { get; set; }
        public virtual DbSet<Sprint> Sprint { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Story> Story { get; set; }
        public virtual DbSet<StoryTag> StoryTag { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaskTag> TaskTag { get; set; }
        public virtual DbSet<TaskUser> TaskUser { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamUser> TeamUser { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}