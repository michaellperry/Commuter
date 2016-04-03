using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Validation;

namespace Commuter.FeedJob
{
    static class ContextExtensions
    {
        public static void ValidateAndSaveChanges(this DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException x)
            {
                var errors = x.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"Error in {e.PropertyName}: {e.ErrorMessage}")
                    .ToArray();
                throw new InvalidOperationException(string.Join("; ", errors));
            }
        }
    }
}
