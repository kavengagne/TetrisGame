using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace GameModel.Models
{
    public class UserCreationResultModel : BaseBindingResultModel
    {
        public bool IsUserCreated { get; set; }

        public UserCreationResultModel(bool isUserCreated, IEnumerable<string> errors = null) : base(errors)
        {
            IsUserCreated = isUserCreated;
        }

        public UserCreationResultModel(bool isUserCreated, ModelStateDictionary modelState) : base(modelState)
        {
            IsUserCreated = isUserCreated;
        }
    }
}