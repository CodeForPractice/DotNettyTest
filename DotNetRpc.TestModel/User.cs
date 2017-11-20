using DotNetRpc.Message;

namespace DotNetRpc.TestModel
{
    public interface IUser
    {
        ResponseMessage GetResponseMesage();

        ResponseMessage GetResponseMesage(int id);

        ResponseMessage GetResponseMesage(string name);

        ResponseMessage GetResponseMesage(int id, string name);
    }

    public class User : IUser
    {
        public ResponseMessage GetResponseMesage()
        {
            return new ResponseMessage();
        }

        public ResponseMessage GetResponseMesage(int id)
        {
            return new ResponseMessage() { MessageId = id.ToString() };
        }
        public ResponseMessage GetResponseMesage(string name)
        {
            return new ResponseMessage() { MessageId = name.ToString() };
        }

        public ResponseMessage GetResponseMesage(int id, string name)
        {
            return new ResponseMessage() { MessageId = id.ToString(), JsonResult = name.ToString() };
        }
    }
}