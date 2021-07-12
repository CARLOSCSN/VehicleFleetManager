using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace AspNetCoreDapper.Tests.Util
{
    public class NotificationUtil
    {
        /// <summary>
        /// Verique se a propriedade deu erro
        /// </summary>
        /// <param name="property"></param>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static bool ExistError(string property, IActionResult actionResult)
        {
            BadRequestObjectResult badRequest = actionResult as BadRequestObjectResult;

            //Não tem erros
            if (badRequest == null)
                return false;

            var json = JsonConvert.SerializeObject(badRequest.Value);
            var response = JsonConvert.DeserializeObject<Response>(json);

            return response.errors.Any(e => e.Key == property);
        }

        public static bool Sucesso(IActionResult actionResult)
        {
            OkObjectResult okRequest = actionResult as OkObjectResult;

            //Tem erros
            if (okRequest == null)
                return false;

            var json = JsonConvert.SerializeObject(okRequest.Value);
            var response = JsonConvert.DeserializeObject<Response>(json);

            return response.success;
        }
    }
}
