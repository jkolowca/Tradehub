﻿using AutoMapper;
using Buisness.Contracts.Models;
using Web.Portal.Models;

namespace Web.Portal.Code
{

    public class ToolsMapper
    {
        internal static IMapper Default = new MapperConfiguration( cfg =>
        {
            cfg.CreateMap<ToolModel, ToolViewModel>();
            cfg.CreateMap<ToolViewModel, ToolModel>();

            cfg.CreateMap<ToolInfoModel, ToolInfoViewModel>();
            cfg.CreateMap<ToolIndexModel, ToolIndexViewModel>();

            cfg.CreateMap<ToolPictureModel, ToolPictureViewModel>();

        } ).CreateMapper();
    }
}