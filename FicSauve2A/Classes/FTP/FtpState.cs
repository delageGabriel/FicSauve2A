using System;
using System.Net;
using System.Threading;

namespace FicSauve2A
{
    /// <summary>
    /// 
    /// </summary>
    public class FtpState
    {
        private ManualResetEvent wait;
        private FtpWebRequest request;
        private string fileName;
        private Exception operationException = null;
        string status;

        /// <summary>
        /// 
        /// </summary>
        public FtpState()
        {
            wait = new ManualResetEvent(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public ManualResetEvent OperationComplete
        {
            get { return wait; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FtpWebRequest Request
        {
            get { return request; }
            set { request = value; }
        }

        /// <summary>
        /// t
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Exception OperationException
        {
            get { return operationException; }
            set { operationException = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StatusDescription
        {
            get { return status; }
            set { status = value; }
        }
    }
}