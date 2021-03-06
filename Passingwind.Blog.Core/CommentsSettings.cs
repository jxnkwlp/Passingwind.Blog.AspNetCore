﻿namespace Passingwind.Blog
{
    public class CommentsSettings : ISettings
    {
        /// <summary>
        ///  允许评论
        /// </summary>
        public bool EnableComments { get; set; } = true;

        /// <summary>
        ///  审核评论
        /// </summary>
        public bool EnableCommentsModeration { get; set; }

        /// <summary>
        ///  信任已通过验证的评论作者
        /// </summary>
        public bool TrustAuthenticatedUsers { get; set; }

        /// <summary>
        ///  嵌套评论
        /// </summary>
        public bool CommentNestingEnabled { get; set; } = true;

        /// <summary>
        ///  显示评论
        /// </summary>
        public bool ShowComments { get; set; } = true;

        /// <summary>
        ///  默认头像
        /// </summary>
        public string DefaultAvatar { get; set; }
    }
}