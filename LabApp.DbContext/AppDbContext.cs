using EntityFrameworkCore.Triggers;
using LabApp.Server.Data.EventOutbox;
using LabApp.Server.Data.Extensions;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Data.Models.Dictionaries;
using LabApp.Server.Data.Models.Interfaces;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Shared.Data.EF.EventOutbox;
using LabApp.Shared.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Data
{
    public class AppDbContext : DbContextWithTriggers, IContextWithEventOutbox
    {
        private readonly IHostingEnvironment _environment;

        private static readonly DeleteBehavior DefaultDeleteBehavior = DeleteBehavior.SetNull;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHostingEnvironment environment)
            : base(options)
        {
            _environment = environment;
        }

        #region User

        public virtual DbSet<UserIdentity> UserIdentities { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Teacher> Teachers { get; set; }

        public virtual DbSet<AcademicRank> AcademicRanks { get; set; }

        public virtual DbSet<UserPhoto> UserPhotos { get; set; }

        #endregion

        public virtual DbSet<PostPhoto> PostPhotos { get; set; }

        public virtual DbSet<PostLike> PostLikes { get; set; }

        public virtual DbSet<Post> Posts { get; set; }


        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<StudentGroup> StudentGroup { get; set; }

        public virtual DbSet<Assignment> Assignments { get; set; }

        public virtual DbSet<Subject> Subjects { get; set; }

        public virtual DbSet<AdditionalScore> AdditionalScores { get; set; }

        public virtual DbSet<Lesson> Lessons { get; set; }

        public virtual DbSet<GroupLesson> GroupLessons { get; set; }
        
        public virtual DbSet<GroupAssignment> GroupAssignment { get; set; }

        public virtual DbSet<StudentLesson> StudentLessons { get; set; }

        public virtual DbSet<StudentAssignment> StudentAssignment { get; set; }

        public virtual DbSet<StudentAssignmentAttachment> StudentAssignmentAttachment { get; set; }

        public virtual DbSet<AssignmentAttachment> AssignmentAttachments { get; set; }


        public virtual DbSet<Conversation> Conversations { get; set; }

        public virtual DbSet<UserConversation> UserConversation { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<MessageAttachment> MessageAttachments { get; set; }

        public virtual DbSet<LessonAttachment> LessonAttachments { get; set; }


        public virtual DbSet<UserLoginHistory> LoginHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetQueryFilterOnAllEntities<IJobDeleted>(x => !x.IsDeleted && !x.ToDelete);
            modelBuilder.SetQueryFilterOnAllEntities<ISoftDeletable>(x => !x.IsDeleted);
            modelBuilder.SetInsertedTrackableDefaults(Database);

            modelBuilder.ApplyConfiguration(new EventOutboxConfiguration());
            
            modelBuilder.Entity<AssignmentAttachment>(e =>
            {
                e.HasOne(x => x.Assignment).WithMany(x => x.Attachments);
            });
            modelBuilder.Entity<MessageAttachment>(e => { e.HasOne(x => x.Message).WithMany(x => x.Attachments); });
            modelBuilder.Entity<LessonAttachment>(e => { e.HasOne(x => x.Lesson).WithMany(x => x.Attachments); });
            modelBuilder.Entity<StudentAssignmentAttachment>(e =>
            {
                e.HasOne(x => x.Assignment).WithMany(x => x.Attachments);
            });
            modelBuilder.Entity<AcademicRank>(e => { });


            // modelBuilder.Entity<Attachment>()
            //     .HasDiscriminator("type", typeof(byte))
            //     .HasValue<AssignmentAttachment>((byte)0)
            //     .HasValue<StudentAssignmentAttachment>((byte)1)
            //     .HasValue<MessageAttachment>((byte)2);

            // modelBuilder.Entity<Image>()
            //     .HasDiscriminator("type", typeof(byte))
            //     .HasValue<PostPhoto>((byte)0)
            //     .HasValue<UserPhoto>((byte)1);

            modelBuilder.Entity<PostPhoto>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostPhotos)
                    .HasForeignKey(d => d.PostId);
            });

            modelBuilder.Entity<UserPhoto>(entity =>
            {
                //entity.Property(e => e.Uploaded).HasDefaultValueSql(GetDefaultValueForDateTime(Database));

                // entity.HasOne(d => d.User)
                //     .WithMany(p => p.Photos)
                //     .HasForeignKey(d => d.UserId)
                //     .OnDelete(DefaultDeleteBehavior);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                //entity.Property(e => e.Datetime).HasDefaultValueSql(GetDefaultValueForDateTime(Database));

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DefaultDeleteBehavior);
            });


            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.HasKey(e => new {UserPostId = e.PostId, e.UserId});

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DefaultDeleteBehavior)
                    ;
            });


            modelBuilder.Entity<Post>(entity =>
            {
                //entity.Property(e => e.Datetime).HasDefaultValueSql(GetDefaultValueForDateTime(Database));

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DefaultDeleteBehavior);
            });
            modelBuilder.Entity<UserIdentity>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.PasswordHash).IsUnicode(false);

                entity.Property(e => e.PasswordSalt).IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasDiscriminator(x => x.UserType)
                    .HasValue<Student>(UserType.Student)
                    .HasValue<Teacher>(UserType.Teacher);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.MainPhoto)
                    .WithOne()
                    .HasForeignKey<User>(x => x.PhotoId)
                    .IsRequired(false)
                    .OnDelete(DefaultDeleteBehavior);

                entity.HasMany(d => d.Photos)
                    .WithOne(p => p.User)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.UserIdentity)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Assignment>(e => { e.Property(x => x.FineAfterDeadline).HasDefaultValueSql("0"); });

            modelBuilder.Entity<StudentGroup>()
                .HasOne<Student>(sc => sc.Student)
                .WithMany(s => s.Groups)
                .HasForeignKey(sc => sc.StudentId).OnDelete(DeleteBehavior.Cascade);
            

            modelBuilder.Entity<GroupAssignment>()
                .HasOne(sc => sc.Assignment)
                .WithMany(s => s.Groups)
                .HasForeignKey(sc => sc.AssignmentId).OnDelete(DeleteBehavior.Cascade);
            
            #region CompositeKeys

            modelBuilder.Entity<GroupLesson>().HasKey(x => new {x.GroupId, x.LessonId});
            modelBuilder.Entity<GroupAssignment>().HasKey(x => new {x.GroupId, x.AssignmentId});
            modelBuilder.Entity<StudentGroup>().HasKey(x => new {x.GroupId, x.StudentId});
            modelBuilder.Entity<UserConversation>().HasKey(x => new {x.ConversationId, x.UserId});
            modelBuilder.Entity<PostLike>().HasKey(x => new {x.PostId, x.UserId});

            #endregion

            #region Indexes

            modelBuilder.Entity<UserIdentity>().HasIndex(x => x.Email);
            modelBuilder.Entity<UserIdentity>().HasIndex(x => x.Phone);
            modelBuilder.Entity<UserLoginHistory>().HasIndex(x => x.Address);

            #endregion
        }

        public DbSet<EventMessage> EventOutbox { get; set; }
    }
}