namespace RestClientTemplate
{
    public class ApiErrorData
    {
        private string errMessage;
        private string errCode;

        public ApiErrorData(string errMessage, string errCode)
        {
            this.errMessage = errMessage;
            this.errCode = errCode;
        }
    }
}