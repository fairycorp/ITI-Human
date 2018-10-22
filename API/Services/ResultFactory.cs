using Stall.Guard.System;

namespace API.Services
{
    /// <summary>
    /// Enables to build success/failure result.
    /// </summary>
    internal static class ResultFactory
    {
        /// <summary>
        /// Builds a success result.
        /// </summary>
        /// <param name="result">Object to give to its claimer.</param>
        /// <returns>A success GuardResult.</returns>
        internal static GuardResult Success(object result)
            => new GuardResult(Status.Success, string.Empty, result);

        /// <summary>
        /// Builds a failure result.
        /// </summary>
        /// <param name="info">Information to send to the unvailable-content claimer.</param>
        /// <returns>A failure GuardResult.</returns>
        internal static GuardResult Failure(string info)
            => new GuardResult(Status.Failure, info);
    }
}
