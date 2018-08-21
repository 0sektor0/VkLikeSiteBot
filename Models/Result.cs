namespace VkLikeSiteBot.Models
{
    public class Result<T>
    {
        private int _code;
        public int Code
        {
            get
            {
                return _code;
            }
        }

        private string[] _errors;
        public string[] Errors
        {
            get
            {
                return _errors;
            }
        }

        private T _data;
        public T Data
        {
            get 
            {
                return _data;
            }
        }

        private bool _state;
        public bool Success
        {
            get
            {
                return _state;
            }
        }


        public Result()
        {
            _state = true;
        }


        public Result(int code, params string[] errors)
        {
            _code = code;
            _errors = errors;
        }


        public Result(params string[] errors) : this(0, errors)
        {
            
        }
    }
}