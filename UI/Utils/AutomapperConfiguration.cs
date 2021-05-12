﻿using AutoMapper;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.Models;

namespace UI.Utils
{
    internal class AutomapperConfiguration : Profile
    {
        public static readonly List<string> difficultyLevel = new List<string>()
        {
            "Easy",
            "Beginner",
            "Normal",
            "Hard",
            "Professional"
        };

        public static readonly List<string> horrorLevel = new List<string>()
        {
            "Peaceful",
            "Strange",
            "Normal",
            "Scary",
            "Horror"
        };

        public AutomapperConfiguration()
        {
            CreateMap<QuestRoom, RoomViewModel>()
                .ForMember(x => x.Id, s => s.MapFrom(z => z.Id))
                .ForMember(x => x.DecorationType, s => s.MapFrom(z => z.DecorationType.Name))
                .ForMember(x => x.ImagesUrl, s => s.MapFrom(z => z.ImagesUrl.Split(',')))
                .ForMember(x => x.ImagesUrlForForm, s => s.MapFrom(z => z.ImagesUrl))
                .ForMember(x => x.CompanyName, s => s.MapFrom(z => z.Company.Name))
                .ForMember(x => x.PassingTime, s => s.MapFrom(z => z.PassingTime.ToString("hh:mm")))
                .ForMember(x => x.DifficultyLevel, s => s.MapFrom(z => difficultyLevel.ElementAt(z.DifficultyLevel - 1) ))
                .ForMember(x => x.HorrorLevel, s => s.MapFrom(z => horrorLevel.ElementAt(z.HorrorLevel - 1) ));

            CreateMap<RoomViewModel, QuestRoom>()
                .ForMember(x => x.Id, s => s.MapFrom(z => z.Id))
                .ForMember(x => x.DecorationType, s => s.MapFrom(z => new DecorationType { Name = z.DecorationType }))
                .ForMember(x => x.ImagesUrl, s => s.MapFrom(z => z.ImagesUrlForForm))
                .ForMember(x => x.Company, s => s.MapFrom(z => new Company { Name = z.CompanyName }))
                .ForMember(x => x.PassingTime, s => s.MapFrom(z => DateTime.Parse(z.PassingTime)))
                .ForMember(x => x.DifficultyLevel, s => s.MapFrom(z => difficultyLevel.IndexOf(z.DifficultyLevel) + 1 ))
                .ForMember(x => x.HorrorLevel, s => s.MapFrom(z => horrorLevel.IndexOf(z.HorrorLevel) + 1 ));

            CreateMap<DecorationType, DecorationTypeViewModel>()
                .ForMember(x => x.Name, s => s.MapFrom(z => z.Name));

            CreateMap<DecorationTypeViewModel, DecorationType>()
                .ForMember(x => x.Name, s => s.MapFrom(z => z.Name));

            CreateMap<Company, CompanyViewModel>()
               .ForMember(x => x.Name, s => s.MapFrom(z => z.Name))
               .ForMember(x => x.Phone, s => s.MapFrom(z => z.Phone));

            CreateMap<CompanyViewModel, Company>()
               .ForMember(x => x.Name, s => s.MapFrom(z => z.Name))
               .ForMember(x => x.Phone, s => s.MapFrom(z => z.Phone));
        }
    }
}