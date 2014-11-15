using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using $safeprojectname$.Models;

namespace $safeprojectname$.Controllers
{
    public class HomeController : Controller
    {
        static RubiksCube rc;

        public ActionResult Index(int size = 2)
        {
            ViewBag.Size = size;
            ViewBag.Message = "Rubiks Cube Simulator";

            rc = new RubiksCube(size);
            ViewBag.RubiksCubeFaces = getRubiksCubeString();           
            return View();
        }

        public HtmlString Rotate(int index, string direction)
        {
            rc.rotate(index, direction);
            return getRubiksCubeString();
        }

        public HtmlString Turn(string direction)
        {
            rc.turn(direction);
            return getRubiksCubeString();
        }

        HtmlString getRubiksCubeString()
        {
            return new HtmlString("front</br>" + rc._front.getHtmlString() +
                "left</br>" + rc._front._left.getHtmlString() +
                "right</br>" + rc._front._right.getHtmlString() +
                "top</br>" + rc._front._top.getHtmlString() +
                "bottom</br>" + rc._front._bottom.getHtmlString() +
                "back</br>" + rc._front._back.getHtmlString());
        }

    }
}



