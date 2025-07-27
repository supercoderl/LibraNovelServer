using LibraNovel.Application.ViewModels.Room;
using LibraNovel.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Message
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public UserInformation FromUser { get; set; }
        public int ToRoomId { get; set; }
        public RoomResponse ToRoom { get; set; }
    }
}
