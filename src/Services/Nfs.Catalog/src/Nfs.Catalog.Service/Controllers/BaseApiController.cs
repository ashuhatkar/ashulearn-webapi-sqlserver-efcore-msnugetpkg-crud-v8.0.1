/*--****************************************************************************
  --* Project Name    : Nfs.Services
  --* Reference       : Microsoft.AspNetCore.Mvc
  --*                   Nfs.Web.Framework.Controllers
  --* Description     : Base api controller
  --* Configuration Record
  --* Review            Ver  Author           Date      Cr       Comments
  --* 001               001  A HATKAR         20/06/24  CR-XXXXX Original
  --****************************************************************************/
using Microsoft.AspNetCore.Mvc;
using Nfs.Web.Framework.Controllers;

namespace Nfs.Catalog.Service.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public abstract partial class BaseApiController : BaseController
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}