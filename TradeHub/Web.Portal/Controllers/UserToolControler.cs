﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using Buisness.Contracts.Models;
//using Buisness.Core.Services;
//using Common.Enums;
//using Common.Filters;
//using Web.Portal.Code;
//using Web.Portal.Models;

//namespace Web.Portal.Controllers
//{
//    //kontroler do przegladania uzytkownikow w systemie i wyswietlania ich profili
//    public class UserToolController : BaseController
//    {
//        private ToolService ToolService = new ToolService();

//        //both Get when going first time and POST when submitting filters
//        [Route( template: "User/Tool", Name = "UserTools" )]
//        public ActionResult Index(ToolFilters filters)
//        {
//            var response = this.ToolService.GetUserTools( filters, this.CurrentUser.Id);
//            return this.View( ToolsMapper.Default.Map<ToolIndexViewModel>( response.Data ) );
//        }

//        [HttpGet]
//        [Route( template: "User/Tool/Add", Name = "AddUserTool" )]
//        public ActionResult Create()
//        {
//            return this.View("Create");
//        }


//        [HttpPost]
//        //[Route( template: "User/Tool/Add", Name = "AddUserTool" )]
//        public ActionResult Create(ToolViewModel toolModel, string returnUrl)
//        {
//            if (!this.ModelState.IsValid)
//            {
//                return this.View(toolModel);
//            }

//            toolModel.UserId = this.CurrentUser.Id;

//            var response = this.ToolService.
//                AddUserTool(ToolsMapper.Default.Map<ToolModel>(toolModel), this.CurrentUser.Id);

//            if (response.Status == ValidationStatus.Failed)
//            {
//                foreach (var err in response.Errors)
//                    this.ModelState.AddModelError("", err);

//                return this.View(toolModel);
//            }
//            return this.RedirectToAction("Index");
//        }

//        [HttpGet]
//        [Route( template: "User/Tool/{toolId}", Name = "UserTool" )]
//        //wszystkie parametry w Url powinny byc nullowalne bo zawsze mozna wpisac urla bez nich
//        public ActionResult View(long? Id)
//        {
//            if(Id == null)
//            {
//                this.RedirectToAction( "Index" );
//            }

//            var response = this.ToolService.GetById( Id.Value );
//            if (response.Status == ValidationStatus.Failed)
//            {
//                //narazie tylko powrot do przegladania, trzeba by dodac jakiegos modala z info ze cos poszlo nie tak
//                return this.Redirect( this.Url.Action() );
//            }

//            return this.View( ToolsMapper.Default.Map<ToolViewModel>( response.Data ) );
//        }

//        [HttpPost]
//        [Route( template: "User/Tool/{toolId}/Dlete", Name = "DeleteUserTool" )]
//        public ActionResult Delete(long id)
//        {
//            var response = this.ToolService.Delete( id );
//            return this.RedirectToAction( "Index" );
//        }

//        [HttpGet]
//        [Route( template: "User/Tool/{toolId}/Edit", Name = "EditUserTool" )]
//        public ActionResult Edit( long? id)
//        {
//            if ( id == null )
//            {
//                this.RedirectToAction( "Index" );
//            }

//            var response = this.ToolService.GetById( id.Value );
//            if ( response.Status == ValidationStatus.Failed )
//            {
//                //narazie tylko powrot do przegladania, trzeba by dodac jakiegos modala z info ze cos poszlo nie tak
//                return this.Redirect( this.Url.Action() );
//            }

//            return this.View( ToolsMapper.Default.Map<ToolViewModel>( response.Data ) );
//        }

//        [HttpPost]
//        //[Route( template: "User/Tool/{toolId}/Edit", Name = "EditUserTool" )]
//        public ActionResult Edit( ToolViewModel toolModel, string returnUrl )
//        {
//            if ( !this.ModelState.IsValid )
//            {
//                return this.View( toolModel );
//            }

//            toolModel.UserId = this.CurrentUser.Id;
//            var response = this.ToolService.Update( ToolsMapper.Default.Map<ToolModel>( toolModel ) );
            
//            if ( response.Status == ValidationStatus.Failed )
//            {
//                foreach ( var err in response.Errors )
//                    this.ModelState.AddModelError( "", err );

//                return this.View( toolModel );
//            }
//            return this.RedirectToAction( "Index" );
//        }

//    }
//}