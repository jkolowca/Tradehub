﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Filters;

namespace Buisness.Contracts.Models
{
    //Index model jest pokazywany na glownej stronie i mozna wyswietlic wszystkich uzytkownikow, filtrowac sortowac i kliknac na kazdego by zobaczyc jego profil
    //potrzebne Id w InfoModel by moc przejsc do odpowiedniej strony
    public class ToolIndexModel
    {
        public ToolFilters Filters { get; set; }
        public List<ToolInfoModel> Tools { get; set; }
    }
}