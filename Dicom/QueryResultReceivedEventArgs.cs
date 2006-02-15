
namespace ClearCanvas.Dicom.Network
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Argument type that encapsulates data pertinent to a
    /// query result having received.
    /// </summary>
    public class QueryResultReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Mandatory constructor.
        /// </summary>
        /// <param name="result">The new result.</param>
        public QueryResultReceivedEventArgs(QueryResultDictionary result)
        {
            _result = result;
        }

        /// <summary>
        /// Gets the query result object.
        /// </summary>
        /// <returns>The result object.</returns>
        public QueryResultDictionary GetResult()
        {
            return _result;
        }

        private QueryResultDictionary _result;
    }
}
