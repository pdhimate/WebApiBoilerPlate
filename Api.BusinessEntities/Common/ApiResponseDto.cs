namespace Api.BusinessEntities.Common
{
    /// <summary>
    /// The generic responseDto from the Api, which wraps all the result and error Dtos.
    /// </summary>
    /// <typeparam name="TResult">The Dto representing the result.</typeparam>
    public class ApiResponseDto<TResult>
    {
        /// <summary>
        /// Represents the details of an error, if any occurs.
        /// </summary>
        public ErrorDto Error { get; set; }

        /// <summary>
        /// Represents the result of the Api call.
        /// In case there was an error, this will be null.
        /// </summary>
        public TResult Result { get; set; }
        
    }
}
