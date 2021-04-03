using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilities
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName = bindingContext.ModelName;
            var val = bindingContext.ValueProvider.GetValue(propertyName);

            if(val == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            try
            {
                var valDeserialize = JsonConvert.DeserializeObject<T>(val.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valDeserialize);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(propertyName, "El valor no es del tipo adecuado");
            }

            return Task.CompletedTask;
        }
    }
}
