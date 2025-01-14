using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5.WPF_Client
{
    public class ChatMessage
    {
        //Id - користувача в чаті
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;

        public static ChatMessage Desserialize(byte[] data)
        {
            ChatMessage msg = new ChatMessage();
            using (var m = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(m))
                {
                    msg.UserId = br.ReadString();
                    msg.Name = br.ReadString();
                    msg.Text = br.ReadString();
                    msg.Photo = br.ReadString();
                }
            }
            return msg;
        }

        public byte[] Serialize()
        {
            using (var m = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(m))
                {
                    bw.Write(UserId);
                    bw.Write(Name);
                    bw.Write(Text);
                    bw.Write(Photo);
                }
                return m.ToArray();
            }
        }
    }
}
