//using System.Linq;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace StudyTravel.Common.QueryFilter
//{
//    public class EDataFilterAttribute : ActionFilterAttribute
//    {
//        public override void OnActionExecuted(ActionExecutedContext context)
//        {

//            var data = context.Result;
//            var responseObject  = data as IQueryable<object>;
//            if (responseObject != null)
//            {
//                // apply EData
//            }
                
//            base.OnActionExecuted(context);

//        }
//    }

//}
