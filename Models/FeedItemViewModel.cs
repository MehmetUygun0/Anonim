namespace Anonim.Models
{
    public enum FeedType
    {
        Itiraf,
        Hikaye
    }

    public class FeedItemViewModel
    {
        public int Id { get; set; }
        public FeedType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string AuthorNickName { get; set; } = "Anonim";
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }
}
