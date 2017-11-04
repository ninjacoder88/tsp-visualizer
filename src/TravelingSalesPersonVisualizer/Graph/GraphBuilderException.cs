using System;
using System.Runtime.Serialization;

namespace TravelingSalesPersonVisualizer
{
    [Serializable]
    public class GraphBuilderException : Exception
    {
        public GraphBuilderException()
        {
        }

        public GraphBuilderException(string message)
            : base(message)
        {
        }

        public GraphBuilderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected GraphBuilderException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}