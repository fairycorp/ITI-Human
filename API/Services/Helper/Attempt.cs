using System;
using API.ViewModels.Order;
using Stall.Guard.System;
using System.Threading.Tasks;

namespace API.Services.Helper
{
    /// <summary>
    /// Contains any action that is operable on any gateway. It enables to avoid 
    /// executing stored procedures for nothing, such as an order deletion when the 
    /// mentionned <see cref="BasicDataOrder.OrderId"/> doesn't return any existing Order.
    /// </summary>
    public static class Attempt
    {
        /// <summary>
        /// Attempts to get the element whose id is the method second argument, through the method first argument.
        /// </summary>
        /// <param name="getElement">Method that gets the element by its id.</param>
        /// <param name="id">Element id.</param>
        /// <param name="hasToExist">Does the element need to exist so that the method returns success ?</param>
        /// <returns></returns>
        public static async Task<GuardResult> ToGetElement<T>(Func<int, Task<T>> getElement, int id, bool hasToExist) where T : class
        {
            var attempt = await getElement(id);

            if (hasToExist)
            {
                if (!(attempt is null))
                    return new GuardResult(Status.Success, string.Empty);
                else
                    return new GuardResult(Status.Failure, "Element does not exist in database.");
            }
            else
            {
                if (attempt is null)
                    return new GuardResult(Status.Success, string.Empty);
                else
                    return new GuardResult(Status.Failure, "Element already exists in database.");
            }
        }

        /// <summary>
        /// Attempts to get the element whose identifier is the method second argument, through the method first argument.
        /// </summary>
        /// <param name="getElement">Method that gets the element by its id.</param>
        /// <param name="identifier">Element identifier.</param>
        /// <param name="hasToExist">Does the element need to exist so that the method returns success ?</param>
        /// <returns></returns>
        public static async Task<GuardResult> ToGetElement<T>(Func<string, Task<T>> getElement, string identifier, bool hasToExist) where T : class
        {
            var attempt = await getElement(identifier);

            if (hasToExist)
            {
                if (!(attempt is null))
                    return new GuardResult(Status.Success, string.Empty);
                else
                    return new GuardResult(Status.Failure, "Element does not exist in database.");
            }
            else
            {
                if (attempt is null)
                    return new GuardResult(Status.Success, string.Empty);
                else
                    return new GuardResult(Status.Failure, "Element already exists in database.");
            }
        }
    }
}
