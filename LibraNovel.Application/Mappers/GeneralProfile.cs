using AutoMapper;
using LibraNovel.Application.ViewModels.Bookmark;
using LibraNovel.Application.ViewModels.Chapter;
using LibraNovel.Application.ViewModels.Comment;
using LibraNovel.Application.ViewModels.Genre;
using LibraNovel.Application.ViewModels.Menu;
using LibraNovel.Application.ViewModels.Novel;
using LibraNovel.Application.ViewModels.Permission;
using LibraNovel.Application.ViewModels.Rating;
using LibraNovel.Application.ViewModels.Role;
using LibraNovel.Application.ViewModels.User;
using LibraNovel.Application.ViewModels.UsersRoles;
using LibraNovel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Mappers
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            //Mapping user
            CreateMap<RegisterViewModel, User>();
            CreateMap<User, UserInformation>();
            CreateMap<UpdateUserViewModel, User>();

            //Mapping novel
            CreateMap<CreateNovelViewModel, Novel>();
            CreateMap<Novel, NovelResponse>();
            CreateMap<UpdateNovelViewModel, Novel>();

            //Mapping chapter
            CreateMap<CreateChapterViewModel, Chapter>();
            CreateMap<Chapter, ChapterResponse>();
            CreateMap<UpdateChapterModel, Chapter>();

            //Mapping genre
            CreateMap<CreateGenreViewModel, Genre>();
            CreateMap<Genre, GenreResponse>();
            CreateMap<UpdateGenreViewModel, Genre>();

            //Mapping bookmark
            CreateMap<CreateBookmarkViewModel, Bookmark>();
            CreateMap<Bookmark, BookmarkResponse>();
            CreateMap<UpdateBookmarkViewModel, Bookmark>();

            //Mapping comment
            CreateMap<CreateCommentViewModel, Comment>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<UpdateCommentViewModel, Bookmark>();

            //Mapping rating
            CreateMap<CreateRatingViewModel, Rating>();
            CreateMap<Rating, RatingResponse>();

            //Mapping role
            CreateMap<CreateRoleViewModel, Role>();
            CreateMap<Role, RoleResponse>();
            CreateMap<UpdateRoleViewModel, Role>();

            //Mapping users roles
            CreateMap<CreateUsersRolesViewModel, UsersRole>();

            //Mapping menu
            CreateMap<CreateMenuViewModel, Menu>();
            CreateMap<Menu, MenuResponse>();
            CreateMap<UpdateMenuViewModel, Menu>();

            //Mapping permission
            CreateMap<CreatePermissionViewModel, Permission>();
            CreateMap<Permission, PermissionResponse>();
            CreateMap<UpdatePermissionViewModel, Permission>();
        }
    }
}
