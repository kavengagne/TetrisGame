using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace GameData.Models
{
    public class BaseBindingResultModel
    {
        public IEnumerable<string> Errors { get; private set; }

        public BaseBindingResultModel(IDictionary<string, ModelState> modelState)
        {
            if (modelState != null)
            {
                var modelErrors = modelState.Values.SelectMany(v => v.Errors);
                Errors = modelErrors.Select(e => e.ErrorMessage);
            }
        }

        public BaseBindingResultModel(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}