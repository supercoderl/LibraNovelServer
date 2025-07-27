using LibraNovel.Application.ViewModels.Message;
using LibraNovel.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Room
{
    public class RoomResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserInformation Admin { get; set; }
        public ICollection<MessageResponse> Messages { get; set; }
    }
}
