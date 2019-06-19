using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailDTO>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => (DateTime.Parse(d.Claims.FirstOrDefault(c => c.ClaimType == "dob").ClaimValue)).CalculateAge());
                })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.City, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "city").ClaimValue);
                })
                .ForMember(dest => dest.Country, opt =>
                 {
                     opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "country").ClaimValue);
                 })
                .ForMember(dest => dest.Gender, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "gender").ClaimValue);
                })
                .ForMember(dest => dest.Interests, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "interests").ClaimValue);
                })
                .ForMember(dest => dest.Introduction, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "introduction").ClaimValue);
                })
                .ForMember(dest => dest.LookingFor, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "lookingfor").ClaimValue);
                })
                .ForMember(dest => dest.KnownAs, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "knownas").ClaimValue);
                });
            CreateMap<User, UserForListDTO>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => (DateTime.Parse(d.Claims.FirstOrDefault(c => c.ClaimType == "dob").ClaimValue)).CalculateAge());
                })
                .ForMember(dest => dest.Gender, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "gender").ClaimValue);
                })
                .ForMember(dest => dest.City, opt =>
                {
                    opt.ResolveUsing(d => d.Claims.FirstOrDefault(c => c.ClaimType == "city").ClaimValue);
                })
                .ForMember(dest => dest.Country, opt =>
                {
                    opt.ResolveUsing(d => d.Claims.FirstOrDefault(c => c.ClaimType == "country").ClaimValue);
                })
                .ForMember(dest => dest.KnownAs, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType == "knownas").ClaimValue);
                });
            CreateMap<Photo, PhotoForDetailDTO>();
            CreateMap<List<UserClaim>, User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDTO>();
        }
    }
}