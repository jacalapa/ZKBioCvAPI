using System.Runtime.CompilerServices;

namespace ZkbioCvApi.Model
{
    public class Result<T>
    {
        public int code { get; set; }

        public string message { get; set; }
        public T data { get; set; }

        public static Result<T> Success(T data)
        {
            return new Result<T>()
            {
                code = 0,
                message = "success",
                data = data
            };
        }
    }
}
