using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace event_client_app.Configure
{
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        // 接口的Apply方法
        public void Apply(ApplicationModel application)
        {
            // 遍历所有的controller
            foreach (var controller in application.Controllers)
            {
                ApplyMatchedRoute(controller);
                ApplyUnmatchedRoute(controller);
            }
        }


        private void ApplyMatchedRoute(ControllerModel controller)
        {
            // 标记 RouteAttribute 的 Controller
            var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();

            //.Any() is using to check a array is empty or not/
            if (matchedSelectors.Any())
            {
                foreach (var selectorModel in matchedSelectors)
                {
                    // 把路由器的前缀和后缀加在一起。后缀来源于 controller 中， 前缀来源于设置。
                    selectorModel.AttributeRouteModel =
                        AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            selectorModel.AttributeRouteModel);
                }
            }
        }

        private void ApplyUnmatchedRoute(ControllerModel controller)
        {
            // 没有标记 RouteAttribute 的 Controller
            var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();

            if (unmatchedSelectors.Any())
            {
                foreach (var selectorModel in unmatchedSelectors)
                {
                    // 添加一个路由器前缀
                    selectorModel.AttributeRouteModel = _centralPrefix;
                }
            }
        }
    }
}