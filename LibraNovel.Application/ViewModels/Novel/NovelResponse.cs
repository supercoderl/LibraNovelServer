﻿using LibraNovel.Application.ViewModels.Chapter;
using LibraNovel.Application.ViewModels.User;
using LibraNovel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Novel
{
    public class NovelResponse
    {
        public int NovelID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? OtherName { get; set; }
        public int TotalPages { get; set; }
        public string? CoverImage { get; set; }
        public int? ViewCount { get; set; }
        public int? FavoriteCount { get; set; }
        public DateTime PublishedDate { get; set; }
        public Guid? PublisherID { get; set; }
        
        public UserInformation? User { get; set; }
        public IReadOnlyList<ChapterResponse>? Chapter { get; set; }

        public string? Status { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }

        public IReadOnlyList<string>? Genres { get; set; }
        public IReadOnlyList<NovelGenre>? Mappings { get; set; }
    }
}