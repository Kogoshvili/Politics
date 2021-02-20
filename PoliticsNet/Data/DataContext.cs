using Microsoft.EntityFrameworkCore;
using PoliticsNet.Models;

namespace Politics.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Candidate> Canditates { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<ProviderType> ProviderType { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<RatingTopic> RatingTopics { get; set; }
        public DbSet<RatingComment> RatingComments { get; set; }
        public DbSet<RatingLike> RatingLikes { get; set; }
        public DbSet<RatingCommentLike> RatingCommentLikes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityComment> ActivityComments { get; set; }
        public DbSet<ActivityLike> ActivityLikes { get; set; }
        public DbSet<ActivityCommentLike> ActivityCommentLikes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();

            // Post
            builder.Entity<Post>()
                .HasMany(p => p.Images)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // PostLikes
            builder.Entity<PostLike>()
                .HasKey(k => new {k.UserId, k.PostId});

            builder.Entity<PostLike>()
                .HasOne(u => u.Post)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(u => u.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostLike>()
                .HasOne(u => u.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rating
            builder.Entity<RatingTopic>()
                .HasMany(p => p.RatingComment)
                .WithOne(p => p.Topic)
                .HasForeignKey(p => p.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rating Likes
            builder.Entity<RatingLike>()
                .HasKey(k => new {k.UserId, k.TopicId});

            builder.Entity<RatingLike>()
                .HasOne(u => u.Topic)
                .WithMany(u => u.RatingLikes)
                .HasForeignKey(u => u.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RatingLike>()
                .HasOne(u => u.User)
                .WithMany(u => u.RatingLikes)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rating Comment Like
            builder.Entity<RatingCommentLike>()
                .HasKey(k => new {k.UserId, k.CommentId});

            builder.Entity<RatingCommentLike>()
                .HasOne(u => u.Comment)
                .WithMany(u => u.Likes)
                .HasForeignKey(u => u.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RatingCommentLike>()
                .HasOne(u => u.User)
                .WithMany(u => u.RatingCommentLikes)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Activity
            builder.Entity<Activity>()
                .HasMany(p => p.ActivityComments)
                .WithOne(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rating Likes
            builder.Entity<ActivityLike>()
                .HasKey(k => new {k.UserId, k.ActivityId});

            builder.Entity<ActivityLike>()
                .HasOne(u => u.Activity)
                .WithMany(u => u.ActivityLikes)
                .HasForeignKey(u => u.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ActivityLike>()
                .HasOne(u => u.User)
                .WithMany(u => u.ActivityLike)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rating Comment Like
            builder.Entity<ActivityCommentLike>()
                .HasKey(k => new {k.UserId, k.CommentId});

            builder.Entity<ActivityCommentLike>()
                .HasOne(u => u.Comment)
                .WithMany(u => u.Likes)
                .HasForeignKey(u => u.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ActivityCommentLike>()
                .HasOne(u => u.User)
                .WithMany(u => u.ACtivityCommentLike)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}