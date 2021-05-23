using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class UMSerializer
    {
        public static byte[] SerializeUser(User user)
        {
            // https://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, user);
                return ms.ToArray();
            }
        }
        public static User DeserializeUser(byte[] userBytes)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();

                ms.Write(userBytes, 0, userBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return (Models.User)bf.Deserialize(ms);
            }
        }


        public static byte[] SerializeToken(ServiceToken token)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, token);
                return ms.ToArray();
            }
        }
        public static ServiceToken DeserializeToken(byte[] tokenBytes)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();

                ms.Write(tokenBytes, 0, tokenBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return (Models.ServiceToken)bf.Deserialize(ms);
            }
        }

    }
}
